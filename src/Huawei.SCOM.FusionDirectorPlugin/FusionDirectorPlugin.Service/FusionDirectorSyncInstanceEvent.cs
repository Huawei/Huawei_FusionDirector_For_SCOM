//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FusionDirectorPlugin.Core;
using FusionDirectorPlugin.Core.Model;
using FusionDirectorPlugin.Dal;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.Model;
using FusionDirectorPlugin.Model.Event;
using System.Web;
using FusionDirectorPlugin.Api;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace FusionDirectorPlugin.Service
{
    public partial class FusionDirectorSyncInstance
    {

        #region Property
        /// <summary>
        /// Gets or sets the alarm datas.
        /// </summary>
        /// <value>The alarm datas.</value>
        public Queue<AlarmData> AlarmDatas { get; set; }

        /// <summary>
        /// Gets or sets the update server tasks.
        /// </summary>
        /// <value>The update server tasks.</value>
        public List<UpdateTask<Server>> UpdateServerTasks { get; set; }

        /// <summary>
        /// Gets or sets the update enclosure tasks.
        /// </summary>
        /// <value>The update enclosure tasks.</value>
        public List<UpdateTask<Enclosure>> UpdateEnclosureTasks { get; set; }
        #endregion

        #region 启用插入事件的任务
        /// <summary>
        ///启用插入事件的任务
        /// </summary>
        /// <returns>Task.</returns>
        private void RunInsertEventTask()
        {
            Task.Run(() =>
            {
                while (this.IsRunning)
                {
                    if (AlarmDatas.Count > 0)
                    {
                        var alarm = AlarmDatas.Dequeue();
                        try
                        {
                            var eventData = new EventData(alarm, this.FusionDirectorIp);
                            switch (alarm.EventCategory)
                            {
                                case "BMC":
                                    ServerConnector.Instance.InsertEvent(eventData);
                                    break;
                                case "Enclosure":
                                    EnclosureConnector.Instance.InsertEvent(eventData);
                                    break;
                                default:
                                    OnPollingError($"Unknown EventCategory: {alarm.EventCategory}.");
                                    break;
                            }
                        }
                        catch (Exception e)
                        {
                            OnPollingError($"Insert Event Error.AlarmId:{alarm.AlarmId}.", e);
                        }
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            });
        }
        #endregion

        #region 订阅和历史告警

        /// <summary>
        /// Synchronizes the alarm.
        /// </summary>
        /// <returns>Task.</returns>
        public Task SyncAlarm()
        {
            return Task.Run(async () =>
            {
                logger.Polling.Info($"Start Sync Alarm On Polling");
                Thread.Sleep(TimeSpan.FromMinutes(5));//等待5分钟再去同步历史告警
                await SyncHistoryAlarm();
                // 插入历史告警完成后调用订阅接口
                if (string.IsNullOrEmpty(this.FusionDirector.SubscribeStatus) || this.FusionDirector.SubscribeStatus != SubscribeStatus.Success)
                {
                    var existSubscriptions = await this.eventService.GetEventServiceSubscriptionsinformationAsync();
                    var existSubscription = existSubscriptions.Members.FirstOrDefault(x => x.IP == pluginConfig.InternetIp && x.Port == pluginConfig.InternetPort);
                    if (existSubscription != null)
                    {
                        logger.Subscribe.Debug($"Subscription is already exist. Cancel the subscription first.");
                        this.eventService.DeleteGivenSubscriptions(existSubscription.Id);
                    }
                    await Subscribe();
                }
                else
                {
                    logger.Subscribe.Debug($"Don't need subscribe:the SubscribeStatus is success");
                }
            });
        }

        private async Task Subscribe()
        {
            try
            {
                logger.Subscribe.Debug($"Start Subscribe");
                var result = await this.eventService.CreateSubscriptionAsync(new CreateSubscriptionBody()
                {
                    Destination = $"https://{pluginConfig.InternetIp}:{pluginConfig.InternetPort}/AlarmReciver.ashx",
                    EventTypes = new List<string> { EventType.Alert.ToString() },
                    Context = "Huawei SCOM plugin event subscription for Fusion Director",
                    Protocol = "Redfish",
                    Oem = new Oem
                    {
                        Huawei = new Model.Huawei
                        {
                            UserName = this.FusionDirector.EventUserName,
                            Resource = "plugin",
                            Password = this.FusionDirector.EventPd,
                        }
                    }
                });
                var subscribeId = result.ToString().Split('/').Last();
                logger.Subscribe.Info($"Subscribe Success: {result}");
                //订阅后更新实体
                FusionDirectorDal.Instance.UpdateSubscribeStatus(this.FusionDirectorIp, SubscribeStatus.Success, "success", subscribeId);
            }
            catch (Exception e)
            {
                logger.Subscribe.Error(e, $"Subscribe error: {this.FusionDirectorIp}");
                FusionDirectorDal.Instance.UpdateSubscribeStatus(this.FusionDirectorIp, SubscribeStatus.Error, e.Message, "");
            }
        }

        private Task SyncHistoryAlarm()
        {
            return Task.Run(async () =>
            {
                logger.Polling.Info($"Start Sync History Alarm");
                var existAlarmData = new List<AlarmData>();
                var existEnclosureAlarmData = EnclosureConnector.Instance.GetExistAlarmDatas(this.FusionDirectorIp);
                var existServerAlarmData = ServerConnector.Instance.GetExistAlarmDatas(this.FusionDirectorIp);
                existAlarmData.AddRange(existEnclosureAlarmData);
                existAlarmData.AddRange(existServerAlarmData);
                logger.Polling.Info($"Get exist events success:[Enclosure:{existAlarmData.Count}] Server:{existServerAlarmData.Count}");
                int totalPage = 1;
                int startPage = 0;
                var allVaildEvent = new List<EventSummary>();
                while (startPage < totalPage)
                {
                    try
                    {
                        startPage++;
                        var filter = "(EventView='CurrentAlert')";
                        //"(EventOrder='FirstOccurTime desc')"
                        var result = await eventService.GetEventListCollectionAsync(100, (startPage - 1) * 100, filter, "(EventOrder=First_Occur_Time asc)");
                        totalPage = (result.MembersCount - result.MembersCount % 100) / 100 + 1;
                        var validList = result.Members.Where(x => x.EventCategory == "BMC" || x.EventCategory == "Enclosure").ToList();
                        allVaildEvent.AddRange(validList);
                        logger.Polling.Info($"SyncHistoryAlarm Success:[TotalCount:{result.Members.Count}] VaildCount:{validList.Count} Enclosure:{validList.Count(x => x.EventCategory == "Enclosure")} BMC:{validList.Count(x => x.EventCategory == "BMC")} Switch:{validList.Count(x => x.EventCategory == "Switch")} ");
                        foreach (var eventSummary in validList)
                        {
                            try
                            {
                                var alarm = new AlarmData(eventSummary);
                                var existAlarm = existAlarmData.FirstOrDefault(x => x.Sn == alarm.Sn);
                                //对比本次列表中的告警和已存在的告警，如果不同再去插入
                                if (existAlarm == null || !CompareAlarmData(alarm, existAlarm))
                                {
                                    var info = await eventService.GetEventsInfoAsync(eventSummary.SerialNumber.ToString());
                                    AlarmDatas.Enqueue(new AlarmData(info));
                                }
                            }
                            catch (Exception e)
                            {
                                OnPollingError($"GetEventsInfoAsync Error. EventID:{eventSummary.EventID}.", e);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        OnPollingError($"SyncHistoryAlarm Error.pageNo:{startPage}.", ex);
                    }
                }
                //检查未关闭的告警，在本次历史告警查询中是否存在，不存在则关闭
                EnclosureConnector.Instance.CheckUnclosedAlert(this.FusionDirectorIp, allVaildEvent.Select(x => x.SerialNumber.ToString()).ToList());
                ServerConnector.Instance.CheckUnclosedAlert(this.FusionDirectorIp, allVaildEvent.Select(x => x.SerialNumber.ToString()).ToList());
            });
        }

        /// <summary>
        /// 对比告警
        /// </summary>
        /// <param name="preData">The pre data.</param>
        /// <param name="nowData">The now data.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CompareAlarmData(AlarmData preData, AlarmData nowData)
        {
            if (preData.AlarmName != nowData.AlarmName) { return false; }
            if (preData.AlarmId != nowData.AlarmId) { return false; }
            if (preData.AlarmName != nowData.AlarmName) { return false; }
            if (preData.ResourceId != nowData.ResourceId) { return false; }
            //if (preData.ResourceIdName != nowData.ResourceIdName) { return false; }
            if (preData.Sn != nowData.Sn) { return false; }
            if (preData.Category != nowData.Category) { return false; }
            if (preData.Severity != nowData.Severity) { return false; }
            if (preData.OccurTime != nowData.OccurTime) { return false; }
            if (preData.ClearTime != nowData.ClearTime) { return false; }
            if (preData.ClearType != nowData.ClearType) { return false; }
            if (preData.IsClear != nowData.IsClear) { return false; }
            if (preData.Additional != nowData.Additional) { return false; }
            if (preData.DeviceId != nowData.DeviceId) { return false; }
            if (preData.EventCategory != nowData.EventCategory) { return false; }
            if (preData.EventSubject != nowData.EventSubject) { return false; }
            //if (preData.EventDescriptionArgs != nowData.EventDescriptionArgs) { return false; }
            return true;
        }
        #endregion

        #region 处理新告警开启更新任务

        /// <summary>
        /// 处理推送过来的告警
        /// </summary>
        /// <param name="data">The data.</param>
        public void DealNewAlarmAsync(AlarmData data)
        {
            switch (data.EventCategory)
            {
                case "Enclosure":
                    this.CreateUpdateEnclosureTask(data.DeviceId, data.Sn);
                    break;
                case "BMC":
                    this.CreateUpdateServerTask(data.DeviceId, data.Sn);
                    break;
                default:
                    break;
            }

            // TODO(turnbig) how to mock this part?
            // var info = await eventService.GetEventsInfoAsync(data.Sn.ToString());
            // this.SubmitNewAlarm(new AlarmData(info));
            this.SubmitNewAlarm(data);
        }

        /// <summary>
        /// 开启一个更新Server的任务
        /// 刷新两次
        /// </summary>
        /// <param name="deviceId">The dn.</param>
        /// <param name="sn">The alarm sn.</param>
        private void CreateUpdateServerTask(string deviceId, int sn)
        {
            var logPre = $"Server [Sn:{sn}] [{deviceId}]";
            try
            {
                logger.NotifyProcess.Info($"{logPre} New Update Server Task.");
                #region DeviceId已存在
                var exsit = this.UpdateServerTasks.FirstOrDefault(x => x.DeviceId == deviceId);
                if (exsit != null)
                {
                    //如果首次刷新已经执行过，此处再执行一次
                    if (exsit.FirstRefreshTime < DateTime.Now)
                    {
                        exsit.FirstRefreshTime = DateTime.Now.AddSeconds(10);
                        StartFirstUpdateServer(exsit, true);
                    }
                    logger.NotifyProcess.Debug($"{logPre} [preSn:{ exsit.Sn}] Delay LastRefreshTime:{DateTime.Now.AddSeconds(60):HH:mm:ss}.");
                    exsit.Sn = sn;
                    exsit.LastRefreshTime = DateTime.Now.AddSeconds(60);//延长最后一次刷新时间
                    return;
                }
                #endregion

                var task = new UpdateTask<Server>(deviceId, sn);
                logger.NotifyProcess.Debug($"{logPre} Will StartUpdateTask [First:{task.FirstRefreshTime:HH:mm:ss}].[Last Nearby :{task.LastRefreshTime:HH:mm:ss}]");
                StartFirstUpdateServer(task, false);
                StartLastUpdateServer(task);
                this.UpdateServerTasks.Add(task);
            }
            catch (Exception e)
            {
                logger.NotifyProcess.Error(e, $"{logPre} StartUpdateTask Error.");
            }
        }

        /// <summary>
        /// 开启一个更新Enclosure的任务
        /// 刷新两次
        /// </summary>
        /// <param name="deviceId">The dn.</param>
        /// <param name="sn">The alarm sn.</param>
        private void CreateUpdateEnclosureTask(string deviceId, int sn)
        {
            var logPre = $"Enclosure [Sn:{sn}] [{deviceId}]";
            try
            {
                logger.NotifyProcess.Info($"{logPre} New Update Enclosure Task.");
                #region DeviceId已存在
                var exsit = this.UpdateEnclosureTasks.FirstOrDefault(x => x.DeviceId == deviceId);
                if (exsit != null)
                {
                    //如果首次刷新已经执行过，此处再执行一次
                    if (exsit.FirstRefreshTime < DateTime.Now)
                    {
                        exsit.FirstRefreshTime = DateTime.Now.AddSeconds(10);
                        StartFirstUpdateEnclosure(exsit, true);
                    }
                    logger.NotifyProcess.Debug($"{logPre} [preSn:{ exsit.Sn}] Delay LastRefreshTime:{exsit.LastRefreshTime:HH:mm:ss}.");

                    exsit.Sn = sn;
                    exsit.LastRefreshTime = DateTime.Now.AddSeconds(60);//延长最后一次刷新时间
                    return;
                }
                #endregion

                var task = new UpdateTask<Enclosure>(deviceId, sn);
                logger.NotifyProcess.Debug($"{logPre} Will StartUpdateTask [First:{task.FirstRefreshTime:HH:mm:ss}].[Last Nearby :{task.LastRefreshTime:HH:mm:ss}]");
                StartFirstUpdateEnclosure(task, false);
                StartLastUpdateEnclosure(task);
                this.UpdateEnclosureTasks.Add(task);
            }
            catch (Exception e)
            {
                logger.NotifyProcess.Error(e, $"{logPre} StartUpdateTask Error.");
            }
        }

        private void StartFirstUpdateEnclosure(UpdateTask<Enclosure> task, bool isResart)
        {
            var logPre = $"Enclosure [Sn:{task.Sn}] [{task.DeviceId}] [isResart:{isResart}] FirstRefreshTask";
            Task.Run(async () =>
            {
                try
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    logger.NotifyProcess.Debug($"{logPre} Start [{task.FirstRefreshTime:HH:mm:ss}]");
                    var enclosure = await QueryEnclosureDetailsById(task.DeviceId);

                    task.DeviceFirst = enclosure;
                    logger.NotifyProcess.Debug($"{logPre} Query Enclosure Finish:[{JsonConvert.SerializeObject(enclosure)}].");
                    await EnclosureConnector.Instance.Sync(enclosure);
                    logger.NotifyProcess.Debug($"{logPre} Sync Enclosure Success.");

                }
                catch (Exception e)
                {
                    logger.NotifyProcess.Error(e, $"{logPre} Error.");
                }
            });
        }

        /// <summary>
        /// Starts the last update enclosure.
        /// </summary>
        /// <param name="task">The task.</param>
        private void StartLastUpdateEnclosure(UpdateTask<Enclosure> task)
        {
            var logPre = $"Enclosure [Sn:{task.Sn}] [{task.DeviceId}] LastRefreshTask:";
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(60));
                        if (DateTime.Now > task.LastRefreshTime)
                        {
                            logger.NotifyProcess.Debug($"{logPre} Start [{task.LastRefreshTime:HH:mm:ss}]");
                            var enclosure = await QueryEnclosureDetailsById(task.DeviceId);

                            var isChange = task.CheckIsChange(enclosure);
                            logger.NotifyProcess.Debug($"{logPre} Query Enclosure Finish:[IsChange:{isChange}][{JsonConvert.SerializeObject(enclosure)}].");
                            if (isChange)
                            {
                                logger.NotifyProcess.Debug($"{logPre} Query Enclosure Finish:[{JsonConvert.SerializeObject(enclosure)}].");
                                await EnclosureConnector.Instance.Sync(enclosure);
                                logger.NotifyProcess.Debug($"{logPre} Sync Enclosure Success.");
                            }

                            this.UpdateEnclosureTasks.Remove(task);
                            break;
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(15));
                    }
                }
                catch (Exception e)
                {
                    logger.NotifyProcess.Error(e, $"{logPre} Error.");
                }
            });
        }

        /// <summary>
        /// Starts the first update server.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="isResart">The is resart.</param>
        private void StartFirstUpdateServer(UpdateTask<Server> task, bool isResart)
        {
            var logPre = $"Server [Sn:{task.Sn}] [{task.DeviceId}] [isResart:{isResart}] FirstRefreshTask";
            Task.Run(async () =>
            {
                try
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    logger.NotifyProcess.Debug($"{logPre} Start [{task.FirstRefreshTime:HH:mm:ss}]");
                    var server = await QueryServerDetailsById(task.DeviceId);
                    task.DeviceFirst = server;
                    logger.NotifyProcess.Debug($"{logPre} Query Server Finish:[{JsonConvert.SerializeObject(server)}].");
                    await ServerConnector.Instance.Sync(server);
                    logger.NotifyProcess.Debug($"{logPre} Sync Server Success.");
                }
                catch (Exception e)
                {
                    logger.NotifyProcess.Error(e, $"{logPre} Error.");
                }
            });
        }

        /// <summary>
        /// Starts the last update server.
        /// </summary>
        /// <param name="task">The task.</param>
        private void StartLastUpdateServer(UpdateTask<Server> task)
        {
            var logPre = $"Server [Sn:{task.Sn}] [{task.DeviceId}] LastRefreshTask ";
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(60));
                        if (DateTime.Now > task.LastRefreshTime)
                        {
                            logger.NotifyProcess.Debug($"{logPre} Start [{task.LastRefreshTime:HH:mm:ss}]");
                            var server = await QueryServerDetailsById(task.DeviceId);
                            var isChange = task.CheckIsChange(server);
                            logger.NotifyProcess.Debug($"{logPre} Query Server Finish:[IsChange:{isChange}][{JsonConvert.SerializeObject(server)}].");
                            if (isChange)
                            {
                                logger.NotifyProcess.Debug($"{logPre} Query Server Finish:[{JsonConvert.SerializeObject(server)}].");
                                await ServerConnector.Instance.Sync(server);
                                logger.NotifyProcess.Debug($"{logPre} Sync Server Success.");
                            }

                            this.UpdateServerTasks.Remove(task);
                            break;
                        }
                        Thread.Sleep(TimeSpan.FromSeconds(15));
                    }
                }
                catch (Exception e)
                {
                    logger.NotifyProcess.Error(e, $"{logPre} Error.");
                }
            });
        }
        #endregion

        #region 告警保活
        /// <summary>
        /// Runs the keep event task.
        /// </summary>
        private void RunKeepEventTask()
        {
            logger.Polling.Info($"Run EventSevice keep alive Task.");
            this.keepEventTimer = new Timer(15 * 60 * 1000)
            {
                Enabled = true,
                AutoReset = true,
            };
            this.keepEventTimer.Elapsed += async (s, e) =>
            {
                logger.Polling.Info($"keepAlive:Subscribe again.");
                if (this.pluginConfig.IsEnableAlert)
                {
                    await this.Subscribe();
                }
            };
            this.keepEventTimer.Start();
        }
        #endregion
    }
}
