//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿// ***********************************************************************
// Assembly         : FusionDirectorPlugin.Core
// Author           : yayun
// Created          : 01-04-2019
//
// Last Modified By : yayun
// Last Modified On : 01-04-2019
// ***********************************************************************
// <copyright file="ServerConnector.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FusionDirectorPlugin.Core.Const;
using FusionDirectorPlugin.Core.Model;
using FusionDirectorPlugin.LogUtil;
using FusionDirectorPlugin.Model;
using FusionDirectorPlugin.Model.Event;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement.Common;
using MPObject = Microsoft.EnterpriseManagement.Common.CreatableEnterpriseManagementObject;

namespace FusionDirectorPlugin.Core
{
    /// <summary>
    /// Class ServerConnector.
    /// </summary>
    /// <seealso cref="FusionDirectorPlugin.Core.BaseConnector" />
    public class ServerConnector : BaseConnector
    {
        #region Fields
        /// <summary>
        /// Gets the instance.
        /// </summary>
        private static ServerConnector instance;
        #endregion

        #region Properties

        public ManagementPackClass ServerClass { get; set; }

        /// <summary>
        /// Gets or sets the server key.
        /// </summary>
        /// <value>The server key.</value>
        public ManagementPackProperty ServerKey { get; set; }

        #endregion

        public static ServerConnector Instance => instance ?? (instance = new ServerConnector());

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerConnector"/> class.
        /// </summary>
        public ServerConnector()
        {
            this.ServerClass = MGroup.Instance.GetManagementPackClass(EntityTypeConst.Server.MainName);

            this.ServerKey = this.ServerClass.PropertyCollection["UnionId"];

            this.MontioringConnector = MGroup.Instance.GetConnector(MGroup.Instance.ServerConnectorGuid);
            if (!this.MontioringConnector.Initialized)
            {
                this.MontioringConnector.Initialize();
            }
        }

        #region Public Methods

        /// <summary>
        /// Synchronizes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public Task Sync(Server model)
        {
            return Task.Run(() =>
            {
                // 存在则更新
                if (this.ExsitsUnionId(model.UnionId, this.ServerClass))
                {
                    this.Update(model, true);
                }
                else
                {
                    this.Insert(model);
                }
            });
        }

        /// <summary>
        /// Inserts the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        private void Insert(Server model)
        {
            try
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Debug($"Insert Server:{model.UnionId}");
                var discoveryData = new IncrementalDiscoveryData();

                #region Server

                var server = this.CreateServer(model);
                discoveryData.Add(server);

                #endregion

                discoveryData.Commit(this.MontioringConnector);
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Debug($"Insert Server Finish:{model.UnionId}");
            }
            catch (Exception e)
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Error(e, $"Insert Server Error:{model.UnionId}");
            }
        }

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isPolling">if set to <c>true</c> [is polling].</param>
        /// <exception cref="System.Exception"></exception>
        public void Update(Server model, bool isPolling)
        {
            try
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Debug($"Start UpdateServer.[{model.UnionId}] [isPolling:{isPolling}]");
                var exsitServer = this.GetObject($"UnionId = '{model.UnionId}'", this.ServerClass);
                if (exsitServer == null)
                {
                    throw new Exception($"Can not find the server:{model.UnionId}");
                }
                var isChange = CompareServer(model, exsitServer);
                if (isChange)
                {
                    var discoveryData = new IncrementalDiscoveryData();
                    UpdateServer(model, exsitServer);
                    discoveryData.Add(exsitServer);
                    discoveryData.Overwrite(this.MontioringConnector);
                    HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Info($"Update server finish:{model.UnionId}");
                }

            }
            catch (Exception e)
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Error(e, $"Update server error.[{model.UnionId}] [isPolling:{isPolling}]");
            }
        }

        #endregion

        #region Create And Update

        private MPObject CreateServer(Server model)
        {
            var propertys = this.ServerClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.ServerClass);
            obj[propertys["UnionId"]].Value = model.UnionId;
            obj[propertys["UUID"]].Value = model.UUID;
            obj[propertys["HostName"]].Value = model.HostName;
            obj[propertys["Model"]].Value = model.Model;
            obj[propertys["BMCVersion"]].Value = model.BMCVersion;
            obj[propertys["BiosVersion"]].Value = model.BiosVersion;
            obj[propertys["TotalSystemMemoryGiB"]].Value = model.TotalSystemMemoryGiB;
            obj[propertys["TotalSystemStorageGiB"]].Value = model.TotalSystemStorageGiB;
            obj[propertys["PowerState"]].Value = model.PowerState;
            obj[propertys["ProcessorInfo"]].Value = model.ProcessorInfo;
            obj[propertys["SerialNumber"]].Value = model.SerialNumber;
            obj[propertys["ServerState"]].Value = model.ServerState;
            obj[propertys["StateReason"]].Value = model.StateReason;
            obj[propertys["iBMCIPv4Address"]].Value = model.iBMCIPv4Address;
            obj[propertys["Tag"]].Value = model.Tag;
            obj[propertys["ProfileModel"]].Value = model.ProfileModelName;
            obj[propertys["AssetTag"]].Value = model.AssetTag;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = $"{model.FusionDirectorIp}_{model.iBMCIPv4Address}";
            return obj;
        }

        private void UpdateServer(Server model, MonitoringObject existObj)
        {
            var propertys = this.ServerClass.PropertyCollection;
            existObj[propertys["UUID"]].Value = model.UUID;
            existObj[propertys["HostName"]].Value = model.HostName;
            existObj[propertys["Model"]].Value = model.Model;
            existObj[propertys["BMCVersion"]].Value = model.BMCVersion;
            existObj[propertys["BiosVersion"]].Value = model.BiosVersion;
            existObj[propertys["TotalSystemMemoryGiB"]].Value = model.TotalSystemMemoryGiB;
            existObj[propertys["TotalSystemStorageGiB"]].Value = model.TotalSystemStorageGiB;
            existObj[propertys["PowerState"]].Value = model.PowerState;
            existObj[propertys["ProcessorInfo"]].Value = model.ProcessorInfo;
            existObj[propertys["SerialNumber"]].Value = model.SerialNumber;
            existObj[propertys["ServerState"]].Value = model.ServerState;
            existObj[propertys["StateReason"]].Value = model.StateReason;
            existObj[propertys["iBMCIPv4Address"]].Value = model.iBMCIPv4Address;
            existObj[propertys["Tag"]].Value = model.Tag;
            existObj[propertys["ProfileModel"]].Value = model.ProfileModelName;
            existObj[propertys["AssetTag"]].Value = model.AssetTag;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = $"{model.FusionDirectorIp}_{model.iBMCIPv4Address}";
        }

        private bool CompareServer(Server model, MonitoringObject existObj)
        {
            var propertys = this.ServerClass.PropertyCollection;
            if (existObj[propertys["UUID"]].ToString() != model.UUID)
            {
                return true;
            }
            if (existObj[propertys["HostName"]].ToString() != model.HostName)
            {
                return true;
            }
            if (existObj[propertys["Model"]].ToString() != model.Model)
            {
                return true;
            }
            if (existObj[propertys["BMCVersion"]].ToString() != model.BMCVersion)
            {
                return true;
            }
            if (existObj[propertys["BiosVersion"]].ToString() != model.BiosVersion)
            {
                return true;
            }
            if (existObj[propertys["TotalSystemMemoryGiB"]].ToString() != model.TotalSystemMemoryGiB.ToString())
            {
                return true;
            }
            if (existObj[propertys["TotalSystemStorageGiB"]].ToString() != model.TotalSystemStorageGiB.ToString())
            {
                return true;
            }
            if (existObj[propertys["PowerState"]].ToString() != model.PowerState)
            {
                return true;
            }
            if (existObj[propertys["ProcessorInfo"]].ToString() != model.ProcessorInfo)
            {
                return true;
            }
            if (existObj[propertys["SerialNumber"]].ToString() != model.SerialNumber)
            {
                return true;
            }
            if (existObj[propertys["ServerState"]].ToString() != model.ServerState)
            {
                return true;
            }
            if (existObj[propertys["StateReason"]].ToString() != model.StateReason)
            {
                return true;
            }
            if (existObj[propertys["iBMCIPv4Address"]].ToString() != model.iBMCIPv4Address)
            {
                return true;
            }
            if (existObj[propertys["Tag"]].ToString() != model.Tag)
            {
                return true;
            }
            if (existObj[propertys["ProfileModel"]].ToString() != model.ProfileModelName)
            {
                return true;
            }
            if (existObj[propertys["AssetTag"]].ToString() != model.AssetTag)
            {
                return true;
            }
            if (existObj[propertys["Health"]].ToString() != model.Health.ToString())
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Remove

        /// <summary>
        /// Removes the server from m group.
        /// </summary>
        /// <param name="fdIp">The fd ip.</param>
        public void RemoveServerByFd(string fdIp)
        {
            try
            {
                HWLogger.GetFdSdkLogger(fdIp).Info($"RemoveServerByFd.[{fdIp}]");
                MGroup.Instance.CheckConnection();
                var criteria = new MonitoringObjectCriteria($"Name like '%{fdIp}%'", ServerClass);
                var devices = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(criteria, ObjectQueryOptions.Default).ToList();
                if (devices.Any())
                {
                    var discovery = new IncrementalDiscoveryData();
                    devices.ForEach(device => discovery.Remove(device));
                    discovery.Commit(this.MontioringConnector);
                }
            }
            catch (Exception ex)
            {
                HWLogger.GetFdSdkLogger(fdIp).Error(ex, "RemoveServerByFd");
            }
        }

        /// <summary>
        /// Removes all server.
        /// </summary>
        public void RemoveAllServer()
        {
            try
            {
                HWLogger.Service.Info($"RemoveAllServer.");
                MGroup.Instance.CheckConnection();
                var devices = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(ServerClass, ObjectQueryOptions.Default).ToList();
                if (devices.Any())
                {
                    var discovery = new IncrementalDiscoveryData();
                    devices.ForEach(device => discovery.Remove(device));
                    discovery.Commit(this.MontioringConnector);
                }
            }
            catch (Exception ex)
            {
                HWLogger.Service.Error(ex, "RemoveAllServer");
            }
        }

        /// <summary>
        /// Deletes the kun lun on synchronize.
        /// </summary>
        /// <param name="fdIp">The e sight ip.</param>
        /// <param name="newIds">The new device ids.</param>
        public void CompareDataOnSync(string fdIp, List<string> newIds)
        {
            try
            {
                MGroup.Instance.CheckConnection();
                HWLogger.GetFdSdkLogger(fdIp).Debug($"Compare Data On Polling.[curQueryResult:{string.Join(",", newIds)}]");
                var criteria = new MonitoringObjectCriteria($"Name like '%{fdIp}%'", ServerClass);

                var exsitObjects = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(criteria, ObjectQueryOptions.Default).ToList();
                var deleteObjects = exsitObjects.Where(x => newIds.All(newDeviceId => newDeviceId != x[ServerKey].Value.ToString())).ToList();
                var newObjects = newIds.Where(x => exsitObjects.All(y => x != y[ServerKey].Value.ToString())).ToList();

                HWLogger.GetFdSdkLogger(fdIp).Info($"Compare Servers Result:[new:{newObjects.Count}] [Delete:{deleteObjects.Count}]");

                var discovery = new IncrementalDiscoveryData();
                deleteObjects.ForEach(deleteDevice =>
                {
                    discovery.Remove(deleteDevice);
                });
                discovery.Commit(this.MontioringConnector);
                HWLogger.GetFdSdkLogger(fdIp).Debug($"Remove Server Polling:[Count:{deleteObjects.Count}].[{string.Join(",", deleteObjects.Select(x => x[ServerKey].Value.ToString()))}]");
            }
            catch (Exception e)
            {
                HWLogger.GetFdSdkLogger(fdIp).Error(e, $"Compare Servers Data On Sync.");
            }
        }

        #endregion

        #region Event
        public void InsertEvent(EventData eventData)
        {
            this.InsertEvent(this.ServerClass, eventData);
        }

        public List<MonitoringEvent> GetExistEvents(string fdIp)
        {
            //https://docs.microsoft.com/en-us/previous-versions/system-center/developer/bb423658(v=msdn.10)
            var list = MGroup.Instance.OperationalData.GetMonitoringEvents(
                new MonitoringEventCriteria($"LoggingComputer = '{fdIp}'"),
                this.ServerClass, TraversalDepth.OneLevel).ToList();
            return list;
        }

        /// <summary>
        /// Gets the history alarm datas.
        /// </summary>
        /// <param name="fdIp">The fd ip.</param>
        /// <returns>List&lt;AlarmData&gt;.</returns>
        public List<AlarmData> GetExistAlarmDatas(string fdIp)
        {
            var existEvents = GetExistEvents(fdIp);
            return existEvents.Select(x => x.GetAlarmData()).ToList();
        }

        /// <summary>
        /// 检查未关闭的告警，在本次历史告警查询中是否存在，不存在则关闭
        /// 防止遗漏
        /// </summary>
        /// <param name="fusionDirectorIp">The fusion director ip.</param>
        /// <param name="sns">The SNS.</param>
        public void CheckUnclosedAlert(string fusionDirectorIp, List<string> sns)
        {
            //读取所有的未关闭的告警
            var unCloseEnclosureAlarm = MGroup.Instance.OperationalData.GetMonitoringAlerts(
                new MonitoringAlertCriteria($"ResolutionState = '0' And CustomField1 like '%{fusionDirectorIp}%'"),
                this.ServerClass, TraversalDepth.OneLevel, null).ToList();
            //判断这些未关闭的告警是否在本次的查询结果中
            var needManualCloseAlert = unCloseEnclosureAlarm.Where(x => sns.All(y => y != x.CustomField5));
            foreach (var monitoringAlert in needManualCloseAlert)
            {
                monitoringAlert.ResolutionState = this.CloseState.ResolutionState;
                monitoringAlert.Update("Manaul Close By SDK");
            }
        }
        #endregion

        #region Performance

        /// <summary>
        /// Inserts the performance data.
        /// </summary>
        /// <param name="unionId">The union identifier.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="realTimeData">The real time data.</param>
        public void InsertPerformanceData(string unionId, string objectName, ServerRealTimePerformance realTimeData)
        {
            PartialMonitoringObject obj = this.GetObjectByUnionId(this.ServerClass, unionId);
            if (obj == null)
            {
                return;
            }
            var cpuUsage = realTimeData.PercentItems.CPUUsagePercent.FirstOrDefault();
            if (cpuUsage?.Value != null)
            {
                var cpuUsageData = new CustomMonitoringPerformanceData(objectName, "CPUUsagePercent", cpuUsage.Value.Value);
                obj.InsertCustomMonitoringPerformanceData(cpuUsageData);
            }

            var powerConsumedWatts = realTimeData.PercentItems.PowerConsumedWatts.FirstOrDefault();
            if (powerConsumedWatts?.Value != null)
            {
                var powerConsumedWattsData = new CustomMonitoringPerformanceData(objectName, "PowerConsumedWatts", powerConsumedWatts.Value.Value);
                obj.InsertCustomMonitoringPerformanceData(powerConsumedWattsData);
            }

            var inletTemp = realTimeData.PercentItems.InletTemp.FirstOrDefault();
            if (inletTemp?.Value != null)
            {
                var inletTempData = new CustomMonitoringPerformanceData(objectName, "InletTemp", inletTemp.Value.Value);
                obj.InsertCustomMonitoringPerformanceData(inletTempData);
            }
        }

        #endregion
    }
}
