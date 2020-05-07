using CommonUtil;
using FusionDirectorPlugin.Core;
using FusionDirectorPlugin.Core.Const;
using FusionDirectorPlugin.Core.Model;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.LogUtil;
using FusionDirectorPlugin.Model;
using FusionDirectorPlugin.Model.Event;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Monitoring;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Huawei.SCOM.ESightPlugin.Const.Constants;
using static Huawei.SCOM.ESightPlugin.Const.Constants.ESightEventeLogSource;

namespace FusionDirectorPlugin.Service
{
    /// <summary>
    ///     Class FusionDirectorAlarmProcessor.
    /// </summary>
    public partial class FusionDirectorSyncInstance
    {

        /// <summary>
        /// 
        /// </summary>
        public static EventLogEntryType[] ShouldProcessedAlarmLevels = new EventLogEntryType[] { EventLogEntryType.Error, EventLogEntryType.Warning };

        /// <summary>
        /// Gets or sets the alarm datas.
        /// </summary>
        /// <value>The alarm datas.</value>
        private Queue<AlarmData> AlarmQueue { get; set; }

        /// <summary>
        /// synchronize lock object for "Alarm" Queue
        /// </summary>
        private readonly object locker = new object();

        /// <summary>
        /// processor working thread
        /// </summary>
        private Thread AlarmProcessor;

        /// <summary>
        /// The new alarm received handle
        /// </summary>
        private readonly AutoResetEvent ReceiveAlarmEvent = new AutoResetEvent(false);


        private int RateLimitQueueSize = 7;
        private TimeSpan RateLimitTimeSpan = TimeSpan.FromSeconds(10);


        private Version Version;

        /// <summary>
        /// key is: event-category_event-id
        /// value is: Fixed size queue records latest n alarm processed time
        /// </summary>
        private readonly Dictionary<string, FixedSizedQueue<ProcessedOn>> ProcessedOnQueueMap = new Dictionary<string, FixedSizedQueue<ProcessedOn>>();


        #region 处理告警队列
        /// <summary>
        /// 处理告警队列
        /// </summary>
        private void StartAlarmEventProcessor()
        {
            Version = MGroup.Instance.Version;
            logger.Polling.Info("Current Management Server version is: " + Version.ToString());

            RateLimitQueueSize = this.pluginConfig.RateLimitQueueSize;
            RateLimitTimeSpan = TimeSpan.FromSeconds(this.pluginConfig.RateLimitTimeSpan);

            if (AlarmProcessor == null)
            {
                AlarmProcessor = new Thread(delegate ()
                {
                    while (this.IsRunning) // TODO(turnbig) 假如Queue里面还有未处理的数据，直接丢弃？
                    {
                        logger.Polling.Info($"Current Alarm Processing Queue amount: {AlarmQueue.Count}.");

                        if (AlarmQueue.Count > 0 || ReceiveAlarmEvent.WaitOne())
                        {
                            AlarmData alarm = null;
                            lock (this.locker)
                            {
                                if (AlarmQueue.Count > 0)
                                {
                                    alarm = AlarmQueue.Dequeue();
                                }
                            }

                            if (alarm != null)
                            {
                                EventData eventObject = new EventData(alarm, this.FusionDirectorIp);
                                logger.Polling.Info($"[{alarm.Sn}] Start processing alarm:: Source:{alarm.DeviceId}, Category:{alarm.EventCategory}, Status: {alarm.Status}.");

                                var objectId = eventObject.UnionId;
                                string mpClazzName = EventCategory.BMC.Equals(alarm.EventCategory) ? EntityTypeConst.Server.MainName : EntityTypeConst.Enclosure.MainName;
                                ManagementPackClass mpClazz = MGroup.Instance.GetManagementPackClass(mpClazzName);
                                MonitoringDeviceObject monitoringObject = BaseConnector.GetDeviceByObjectId(mpClazz, objectId);
                                if (monitoringObject == null)
                                {
                                    // TODO(turnbig.net) should we trigger an update server task, and retry later?
                                    logger.Polling.Warn($"[{alarm.Sn}] No MonitoringObject({objectId}) exists, alarm will be ignored.");
                                    continue;
                                }

                                // waiting for monitoring-object ready.
                                WaitForDeviceMonitored(monitoringObject);


                                var now = DateTime.Now;
                                ProcessedOn previewNProcessedOn = null;
                                FixedSizedQueue<ProcessedOn> processedOnQueue = GetProcessedOnQueue(eventObject);
                                if (EventStatus.Cleared.Equals(alarm.Status))
                                {
                                    // Close SCOM alert
                                    CloseSCOMAlert(eventObject, monitoringObject);
                                }
                                else if (ShouldProcessedAlarmLevels.Contains(eventObject.LevelId))
                                {
                                    // Create New EventLog for new alarms, and generate SCOM alert through associated rule
                                    CreateNewEventLogForAlarm(eventObject);
                                    // use a seperated process on tracker
                                    previewNProcessedOn = processedOnQueue.Enqueue(new ProcessedOn(now));
                                }

                                if (previewNProcessedOn != null)
                                {
                                    TimeSpan timeSpan = now - previewNProcessedOn.Timestamp;
                                    // do not know why system time was changed to yestoday.
                                    if (now >= previewNProcessedOn.Timestamp && timeSpan < RateLimitTimeSpan)
                                    {
                                        TimeSpan timeout = RateLimitTimeSpan - timeSpan;
                                        logger.Polling.Info($"Alarm processing reach rate limit, {processedOnQueue.Size} alarms have been processed during time span {timeSpan}, will sleep {timeout} now.");
                                        Thread.Sleep(timeout);
                                    }
                                }
                            }
                        }
                    }
                });
            }

            this.AlarmProcessor.Start();
            logger.Polling.Info("Alarm processor starts successfully.");
        }

        private FixedSizedQueue<ProcessedOn> GetProcessedOnQueue(EventData eventObject)
        {
            // for SCOM below 2019(10.x.x.x), Rule Rate limit has bug, we use single ProcessOnQueue for all rules
            var key = "Common";
            if (Version.Major >= 10)
            {
                key = $"{eventObject.AlarmData.EventCategory}_{eventObject.EventId}"; // group by rule
            }

            bool exists = ProcessedOnQueueMap.ContainsKey(key);
            if (!exists)
            {
                FixedSizedQueue<ProcessedOn> ProcessedOnQueue = new FixedSizedQueue<ProcessedOn>(RateLimitQueueSize);
                ProcessedOnQueueMap.Add(key, ProcessedOnQueue);
            }

            return ProcessedOnQueueMap[key];
        }
        #endregion


        #region Create EventLog For Alarm
        /// <summary>
        /// Create New EventLog according to the esight alarm
        /// </summary>
        /// <param name="eventObject"></param>
        private void CreateNewEventLogForAlarm(EventData eventObject)
        {
            try
            {
                var alarm = eventObject.AlarmData;
                logger.Polling.Info($"[{alarm.Sn}] Persist alarm to window EventLog now.");

                /** 
                    we do not care about whether device's health status is "not monitoring",
                    we can just insert alarm to window EventLog even it may be droped.
                 */

                // Create new event log instance
                EventInstance instance = new EventInstance(int.Parse(eventObject.EventId), 0, eventObject.LevelId);
                // EventInstance instance = new EventInstance(eventObject.AlarmSn, 0, eventObject.LevelId);
                object[] values = new object[] {
                    eventObject.Description,
                    this.FusionDirector.UniqueId,
                    eventObject.UnionId,        // SCOM monitor object Id
                    eventObject.AlarmSn,        // Alarm Serial Number
                    eventObject.AlarmData.AlarmName.Split('#').Last(),  //index::4 channel
                    eventObject.Priority,
                    eventObject.Severity,                   // Alert Severity
                    eventObject.AlarmData.EventSubject      // index::7
                };

                // return $@"Alert ""{AlarmData.AlarmName.Split('#').Last()}"" was reported for ""{AlarmData.EventSubject}({AlarmData.ResourceIdName})"" at {eventTimeString}.
                // It's caused by ""{AlarmData.Additional ?? string.Empty}"". Suggested resolution is: {Environment.NewLine}{AlarmData.Suggstion ?? string.Empty}.";

                EventLog.WriteEvent(EVENT_SOURCE, instance, values);
                this.logger.Polling.Info($"[{alarm.Sn}] Persist alarm to window EventLog successfully.");
            }
            catch (Exception e)
            {
                this.logger.Polling.Error(e, "[{alarm.Sn}] Failed to persist alarm to window EventLog");
            }
        }

        #endregion


        #region Close SCOM Alert
        /// <summary>
        /// Close SCOM Alert associated with the Alarm
        /// </summary>
        /// <param name="eventObject"></param>
        /// <param name="monitoringObject"></param>
        private void CloseSCOMAlert(EventData eventObject, MonitoringDeviceObject monitoringObject)
        {
            var alarm = eventObject.AlarmData;
            logger.Polling.Info($"[{alarm.Sn}] alarm is cleared, close SCOM alert now.");

            // We will identify the alert using suppression rule.
            // Already closed alerts should be ignored.
            var criteria = $"ResolutionState != '255' and CustomField1 = '{this.FusionDirector.UniqueId}' " +
                            $"and CustomField3 = '{eventObject.AlarmSn}'";
            ReadOnlyCollection<MonitoringAlert> alerts = monitoringObject.Device.GetMonitoringAlerts(new MonitoringAlertCriteria(criteria));
            if (alerts.Count == 0)
            {
                logger.Polling.Warn($"[{alarm.Sn}] No un-closed SCOM alert is associated with current alarm.");
            }
            else
            {
                // It should be 1 in normal sutiation.
                logger.Polling.Info($"[{alarm.Sn}] Associated SCOM alerts count is: {alerts.Count}.");
            }

            foreach (MonitoringAlert alert in alerts)
            {
                alert.ResolutionState = EnclosureConnector.Instance.CloseState.ResolutionState;
                var reason = !string.IsNullOrEmpty(eventObject.AlarmData.ClearType) ? eventObject.AlarmData.ClearType : "Receive alarm cleared notification from subscription.";
                alert.Update(reason);
                logger.Polling.Info($"[{alarm.Sn}] Close SCOM alert successfully.");
            }
        }
        #endregion


        #region waiting monitored object ready
        /// <summary>
        /// Waiting for device object monitored.
        /// </summary>
        /// <param name="obj"></param>
        public bool WaitForDeviceMonitored(MonitoringDeviceObject obj)
        {
            // When an object is first created, it's status is "not monitoring". 
            // The status will changed when Monitor run with a configed interval.

            TimeSpan expectTimeLong = TimeSpan.FromMinutes(10);
            TimeSpan maxWaitTimeLong = TimeSpan.FromMinutes(15);

            if (obj.Device.StateLastModified == null)
            {
                while (obj.Device.HealthState == HealthState.Uninitialized)
                {
                    this.logger.Polling.Info($"MonitoringObject({obj.DeviceId}) is not monitoring.");
                    this.logger.Polling.Info($"     Device added time is: {obj.Device.TimeAdded}.");
                    this.logger.Polling.Info($"     Device last modified time is: {obj.Device.LastModified}.");
                    this.logger.Polling.Info($"     Device state last modified time is: {obj.Device.StateLastModified}.");
                    logger.Polling.Info($"Current health state for device `{obj.DeviceId}` is {obj.Device.HealthState}");

                    // Do not know why RecalculateMonitoringState will stop Service, So, we just wait the monitor run automate.
                    // obj.Device.RecalculateMonitoringState();
                    var stateLastModified = obj.Device.StateLastModified ?? obj.Device.LastModified;
                    TimeSpan stateNotChangedTimeLong = DateTime.UtcNow - stateLastModified;

                    // the interval of monitor for our object is 5 minutes. So we will wait 5m.
                    if (stateNotChangedTimeLong <= expectTimeLong)
                    {
                        Thread.Sleep(expectTimeLong - stateNotChangedTimeLong);
                    }
                    else if (stateNotChangedTimeLong <= maxWaitTimeLong)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                    else
                    {
                        logger.Polling.Info($"Max waiting time for device's monitoring status excess, will skip current device for now.");
                        return false;
                    }

                    obj.Reload();
                    logger.Polling.Info($"Current health state for device `{obj.DeviceId}` is {obj.Device.HealthState}");
                }
            }

            /**
            while (!device.IsAvailable)
            {
                this.logger.Polling.Info($"MonitoringObject({obj.DeviceId}) is not monitoring.");
                this.logger.Polling.Info($"     Device added time is: {device.TimeAdded}.");
                this.logger.Polling.Info($"     Device last modified time is: {device.LastModified}.");
                this.logger.Polling.Info($"     Device availability last modified time is: {device.AvailabilityLastModified}.");

                var availableLastModified = device.AvailabilityLastModified.HasValue ? device.AvailabilityLastModified.Value : device.LastModified;
                TimeSpan availableNotChangedTimeLong = DateTime.UtcNow - availableLastModified;
                // the interval of monitor for our object is 5 minutes. So we will wait 5m.
                if (availableNotChangedTimeLong <= expectTimeLong)
                {
                    Thread.Sleep(expectTimeLong - availableNotChangedTimeLong);
                }

                obj.Reload();
                logger.Polling.Info($"Current health state for device `{obj.DeviceId}` is {obj.Device.HealthState}");
            }*/

            return true;
        }
        #endregion


        #region sync current open alarms of fusion director
        /// <summary>
        /// Synchronizes open alarms from fusion director.
        /// </summary>
        public async void SyncFusionDirectorOpenAlarms()
        {
            // TODO(turnbig) should we unsubscribe before sync?
            logger.Polling.Info($"[SyncOpenAlarms] Start sync open alarms for fusion director `{FusionDirectorIp}`.");

            // We need to get all open alerts here, because we need to compare and close them later.
            // Get all not closed SCOM alerts
            var criteria = $"ResolutionState != '255' and CustomField1 = '{this.FusionDirector.UniqueId}'";
            var alerts = MGroup.Instance.OperationalData.GetMonitoringAlerts(new MonitoringAlertCriteria(criteria), null);
            logger.Polling.Info($"[SyncOpenAlarms] Unclosed SCOM alerts count of (`{FusionDirectorIp}`) is: {alerts.Count}");

            int totalPages = 1;
            int currentPage = 1;

            var allOpenAlarms = new List<EventSummary>();
            while (currentPage <= totalPages && IsRunning)
            {
                try
                {
                    Model.EventList eventView = await eventService.GetEventView(100, (currentPage - 1) * 100);
                    totalPages = (eventView.MembersCount / 100) + (eventView.MembersCount % 100 == 0 ? 0 : 1);

                    allOpenAlarms.AddRange(eventView.Members);
                    logger.Polling.Info($"[SyncOpenAlarms] Succeed fetching open alarms. Pagination:: page: {currentPage}, count:{eventView.Members.Count}.");

                    foreach (var item in eventView.Members)
                    {
                        // we does not care about whether the alert exists or not indeed.
                        // if we insert same alert, it will just increase repeat count.
                        // But we still keep the old logics here. :)

                        var alarm = new AlarmData(item);
                        var alert = alerts.FirstOrDefault(GetScomAlertSuppressionPredicator(alarm));

                        bool shouldProcess = false;
                        if (alert != null)
                        {
                            // only alarm EventSource or EventSubject or Severity may be modified
                            // shouldProcess = alert.CustomField7 != alarm.ResourceId;
                            shouldProcess = false;
                        }
                        else
                        {
                            lock (this.locker)
                            {
                                // find data from current alarm processing queue
                                shouldProcess = !AlarmQueue.Any(_alarm =>
                                {
                                    return _alarm.Sn.Equals(alarm.Sn);
                                });
                            }
                        }

                        if (shouldProcess)
                        {
                            logger.Polling.Info($"[SyncOpenAlarms] Alarm `{alarm.Sn}` has not been processed, submit to queue now.");
                            // Submit new alarm to update the alert
                            var info = await eventService.GetEventsInfoAsync(item.SerialNumber.ToString());
                            SubmitNewAlarm(new AlarmData(info));
                        }
                        else
                        {
                            logger.Polling.Info($"[SyncOpenAlarms] Alarm `{alarm.Sn}` exists, ignore.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Polling.Error(ex, $"[SyncOpenAlarms] Failed to sync open alarms from fusion director: {FusionDirectorIp}. Page number: {currentPage}.");
                }
                finally
                {
                    currentPage++;
                }
            }

            try
            {
                // find all alerts that is still open in SCOM but not in esight ope
                alerts.ToList().ForEach(alert =>
                {
                    bool exists = allOpenAlarms.Any(alarm =>
                    {
                        var data = new AlarmData(alarm);
                        return GetScomAlertSuppressionPredicator(data).Invoke(alert);
                    });

                    if (!exists)
                    {
                        alert.ResolutionState = EnclosureConnector.Instance.CloseState.ResolutionState;
                        alert.Update($"Closed by sync open alarms interval task.");
                        logger.Polling.Info($"[{alert.CustomField3}] Succeed closing SCOM alert when syncing open alarm task.");
                    }
                });
            }
            catch (Exception e)
            {
                logger.Polling.Error(e, $"[SyncOpenAlarms] Failed to close cleared alerts when sync open alarms.");
            }


            try
            {
                if (IsRunning)
                {
                    // 同步所有告警完成后,重新进行订阅
                    if (string.IsNullOrEmpty(this.FusionDirector.SubscribeStatus) || this.FusionDirector.SubscribeStatus != SubscribeStatus.Success)
                    {
                        var existSubscriptions = await this.eventService.GetEventServiceSubscriptionsinformationAsync();
                        var existSubscription = existSubscriptions.Members.FirstOrDefault(x => x.IP == pluginConfig.InternetIp && x.Port == pluginConfig.InternetPort);
                        if (existSubscription != null)
                        {
                            logger.Subscribe.Info($"[SyncOpenAlarms] Subscription for current SCOM is already exist. Will delete it now.");
                            this.eventService.DeleteGivenSubscriptions(existSubscription.Id);
                        }

                        await Subscribe();
                    }
                    else
                    {
                        logger.Subscribe.Info($"[SyncOpenAlarms] Subscription status is already success. No re-subscribe required.");
                    }
                }
            }
            catch (Exception e)
            {
                logger.Subscribe.Error(e, $"[SyncOpenAlarms] Try re-subscribe failed");
            }
        }

        private static Func<MonitoringAlert, bool> GetScomAlertSuppressionPredicator(AlarmData alarm)
        {
            return _alert =>
            {
                return _alert.CustomField3.Equals(alarm.Sn.ToString());
            };
        }
        #endregion


        /// <summary>
        /// Submit alarm to processing queue
        /// </summary>
        /// <param name="data">The data.</param>
        public void SubmitNewAlarm(AlarmData alarm)
        {
            switch (alarm.EventCategory)
            {
                case EventCategory.BMC:
                case EventCategory.Enclosure:
                    logger.Polling.Info($"[{alarm.Sn}] Submit alarm to processing queue.");
                    lock (locker)
                    {
                        AlarmQueue.Enqueue(alarm);
                    }
                    this.ReceiveAlarmEvent.Set();
                    break;
                default:
                    logger.Polling.Warn($"[{alarm.Sn}] Ignore alarm, unknown event category: {alarm.EventCategory}.");
                    break;
            }
        }
    }

    class ProcessedOn
    {
        public DateTime Timestamp { get; set; }

        public ProcessedOn(DateTime timestamp)
        {
            this.Timestamp = timestamp;
        }

    }

    public class FixedSizedQueue<T> where T : class
    {
        ConcurrentQueue<T> Queue = new ConcurrentQueue<T>();
        private readonly object LockObj = new object();

        public int Size { get; private set; }

        public FixedSizedQueue(int size)
        {
            Size = size;
        }

        public T Enqueue(T obj)
        {
            Queue.Enqueue(obj);
            T overflow = null;
            lock (LockObj)
            {
                while (Queue.Count > Size)
                {
                    Queue.TryDequeue(out overflow);
                }
            }
            return overflow;
        }

    }
}
