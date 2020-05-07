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
// Last Modified On : 01-07-2019
// ***********************************************************************
// <copyright file="EnclosureConnector.cs" company="Huawei Technologies Co. Ltd">
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
    /// Class EnclosureConnector.
    /// </summary>
    /// <seealso cref="FusionDirectorPlugin.Core.BaseConnector" />
    public class EnclosureConnector : BaseConnector
    {
        #region Fields
        /// <summary>
        /// Gets the instance.
        /// </summary>
        private static EnclosureConnector instance;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the enclosure class.
        /// </summary>
        /// <value>The enclosure class.</value>
        public ManagementPackClass EnclosureClass { get; set; }

        /// <summary>
        /// Gets or sets the enclosure key.
        /// </summary>
        /// <value>The enclosure key.</value>
        public ManagementPackProperty EnclosureKey { get; set; }
        #endregion

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static EnclosureConnector Instance => instance ?? (instance = new EnclosureConnector());

        /// <summary>
        /// Initializes a new instance of the <see cref="EnclosureConnector" /> class.
        /// </summary>
        public EnclosureConnector()
        {
            this.EnclosureClass = MGroup.Instance.GetManagementPackClass(EntityTypeConst.Enclosure.MainName);

            this.EnclosureKey = this.EnclosureClass.PropertyCollection["UnionId"];

            this.MontioringConnector = MGroup.Instance.GetConnector(MGroup.Instance.EnclosureConnectorGuid);
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
        public Task Sync(Enclosure model)
        {
            return Task.Run(() =>
            {
                // 存在则更新
                if (this.ExsitsUnionId(model.UnionId, this.EnclosureClass))
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
        public void Insert(Enclosure model)
        {
            try
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Debug($"Insert Enclosure:{model.UnionId}");
                var discoveryData = new IncrementalDiscoveryData();

                #region Enclosure

                var enclosure = this.CreateEnclosure(model);
                discoveryData.Add(enclosure);

                #endregion

                discoveryData.Commit(this.MontioringConnector);
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Debug($"InsertEnclosure finish.[{model.UnionId}]");
            }
            catch (Exception e)
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Error(e, $"Insert Enclosure Error:{model.UnionId}");
            }
        }

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isPolling">if set to <c>true</c> [is polling].</param>
        /// <exception cref="System.Exception"></exception>
        public void Update(Enclosure model, bool isPolling)
        {
            try
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Debug($"Start UpdateEnclosure.[{model.UnionId}] [isPolling:{isPolling}]");
                var exsitEnclosure = this.GetObject($"UnionId = '{model.UnionId}'", this.EnclosureClass);
                if (exsitEnclosure == null)
                {
                    throw new Exception($"Can not find the server:{model.UnionId}");
                }
                var isChange = CompareEnclosure(model, exsitEnclosure);
                if (isChange)
                {
                    var discoveryData = new IncrementalDiscoveryData();
                    this.UpdateEnclosure(model, exsitEnclosure);
                    discoveryData.Add(exsitEnclosure);
                    discoveryData.Overwrite(this.MontioringConnector);
                    HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Info($"Update enclosure finish.[{model.UnionId}]");
                }
            }
            catch (Exception e)
            {
                HWLogger.GetFdSdkLogger(model.FusionDirectorIp).Error(e, $"Update enclosure error.[{model.UnionId}] [isPolling:{isPolling}]");
            }
        }

        #endregion

        #region Create And Update

        private MPObject CreateEnclosure(Enclosure model)
        {
            var propertys = this.EnclosureClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.EnclosureClass);
            obj[propertys["UnionId"]].Value = model.UnionId;
            obj[propertys["Name"]].Value = model.Name;
            obj[propertys["Type"]].Value = model.Type.ToString();
            obj[propertys["FirmwareVersion"]].Value = model.FirmwareVersion;
            obj[propertys["Hostname"]].Value = model.Hostname;
            obj[propertys["SerialNumber"]].Value = model.SerialNumber;
            obj[propertys["PartNumber"]].Value = model.PartNumber;
            obj[propertys["ProductName"]].Value = model.ProductName;
            obj[propertys["EnclosureState"]].Value = model.EnclosureState.ToString();
            obj[propertys["StateReason"]].Value = model.StateReason;
            obj[propertys["FanSpeedAdjustmentMode"]].Value = model.FanSpeedAdjustmentMode;
            obj[propertys["HMMFloatIPv4Address"]].Value = model.HMMFloatIPv4Address;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = model.Name;
            return obj;
        }

        private void UpdateEnclosure(Enclosure model, MonitoringObject existObj)
        {
            var propertys = this.EnclosureClass.PropertyCollection;
            existObj[propertys["Name"]].Value = model.Name;
            existObj[propertys["Type"]].Value = model.Type.ToString();
            existObj[propertys["FirmwareVersion"]].Value = model.FirmwareVersion;
            existObj[propertys["Hostname"]].Value = model.Hostname;
            existObj[propertys["SerialNumber"]].Value = model.SerialNumber;
            existObj[propertys["PartNumber"]].Value = model.PartNumber;
            existObj[propertys["ProductName"]].Value = model.ProductName;
            existObj[propertys["EnclosureState"]].Value = model.EnclosureState.ToString();
            existObj[propertys["StateReason"]].Value = model.StateReason;
            existObj[propertys["FanSpeedAdjustmentMode"]].Value = model.FanSpeedAdjustmentMode;
            existObj[propertys["HMMFloatIPv4Address"]].Value = model.HMMFloatIPv4Address;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.Name;
        }

        private bool CompareEnclosure(Enclosure model, MonitoringObject existObj)
        {
            var propertys = this.EnclosureClass.PropertyCollection;
            if (existObj[propertys["Name"]].Value.ToString() != model.Name)
            {
                return true;
            }
            if (existObj[propertys["Type"]].Value.ToString() != model.Type.ToString())
            {
                return true;
            }
            if (existObj[propertys["FirmwareVersion"]].Value.ToString() != model.FirmwareVersion.ToString())
            {
                return true;
            }
            if (existObj[propertys["Hostname"]].Value.ToString() != model.Hostname.ToString())
            {
                return true;
            }
            if (existObj[propertys["SerialNumber"]].Value.ToString() != model.SerialNumber.ToString())
            {
                return true;
            }
            if (existObj[propertys["PartNumber"]].Value.ToString() != model.PartNumber.ToString())
            {
                return true;
            }
            if (existObj[propertys["ProductName"]].Value.ToString() != model.ProductName.ToString())
            {
                return true;
            }
            if (existObj[propertys["EnclosureState"]].Value.ToString() != model.EnclosureState.ToString())
            {
                return true;
            }
            if (existObj[propertys["StateReason"]].Value.ToString() != model.StateReason.ToString())
            {
                return true;
            }
            if (existObj[propertys["FanSpeedAdjustmentMode"]].Value.ToString() != model.FanSpeedAdjustmentMode.ToString())
            {
                return true;
            }
            if (existObj[propertys["HMMFloatIPv4Address"]].Value.ToString() != model.HMMFloatIPv4Address.ToString())
            {
                return true;
            }
            if (existObj[propertys["Health"]].Value.ToString() != model.Health.ToString())
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
        public void RemoveEnclosureByFd(string fdIp)
        {
            try
            {
                HWLogger.GetFdSdkLogger(fdIp).Info($"RemoveEnclosureByFd.[{fdIp}]");
                MGroup.Instance.CheckConnection();
                var criteria = new MonitoringObjectCriteria($"Name like '%{fdIp}%'", EnclosureClass);
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
                HWLogger.GetFdSdkLogger(fdIp).Error(ex, "RemoveEnclosureByFd");
            }
        }

        /// <summary>
        /// Removes all enclosure.
        /// </summary>
        public void RemoveAllEnclosure()
        {
            try
            {
                HWLogger.Service.Info($"RemoveAllEnclosure.");
                MGroup.Instance.CheckConnection();
                var devices = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(EnclosureClass, ObjectQueryOptions.Default).ToList();
                if (devices.Any())
                {
                    var discovery = new IncrementalDiscoveryData();
                    devices.ForEach(device => discovery.Remove(device));
                    discovery.Commit(this.MontioringConnector);
                }
            }
            catch (Exception ex)
            {
                HWLogger.Service.Error(ex, "RemoveAllEnclosure");
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
                HWLogger.GetFdSdkLogger(fdIp).Info($"Check And Removed Enclosure On Polling.[curQueryResult:{string.Join(",", newIds)}]");
                var criteria = new MonitoringObjectCriteria($"Name like '%{fdIp}%'", EnclosureClass);
                var exsitObjects = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(criteria, ObjectQueryOptions.Default).ToList();
                var deleteObjects = exsitObjects.Where(x => newIds.All(newDeviceId => newDeviceId != x[EnclosureKey].Value.ToString())).ToList();
                var newObjects = newIds.Where(x => exsitObjects.All(y => x != y[EnclosureKey].Value.ToString())).ToList();

                HWLogger.GetFdSdkLogger(fdIp).Info($"Compare Enclosures Result:[new:{newObjects.Count}] [Delete:{deleteObjects.Count}]");

                var discovery = new IncrementalDiscoveryData();
                deleteObjects.ForEach(deleteDevice =>
                {
                    discovery.Remove(deleteDevice);
                });
                discovery.Commit(this.MontioringConnector);
                HWLogger.GetFdSdkLogger(fdIp).Debug($"Remove Enclosure On Polling:[Count:{deleteObjects.Count}].[{string.Join(",", deleteObjects.Select(x => x[EnclosureKey].Value.ToString()))}]");
            }
            catch (Exception e)
            {
                HWLogger.GetFdSdkLogger(fdIp).Error(e, $"Compare Enclosures Data On Sync.");
            }
        }

        #endregion

        #region Event

        public void InsertEvent(EventData eventData)
        {
            this.InsertEvent(this.EnclosureClass, eventData);
        }

        private PartialMonitoringObject GetParentEnclosure(PartialMonitoringObject obj)
        {
            return this.GetParentObject(obj);
        }

        public List<MonitoringEvent> GetExistEvents(string fdIp)
        {
            //https://docs.microsoft.com/en-us/previous-versions/system-center/developer/bb423658(v=msdn.10)
            var list = MGroup.Instance.OperationalData.GetMonitoringEvents(
                new MonitoringEventCriteria($"LoggingComputer = '{fdIp}'"),
                this.EnclosureClass, TraversalDepth.OneLevel).ToList();
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
                this.EnclosureClass, TraversalDepth.OneLevel, null).ToList();
            //判断这些未关闭的告警是否在本次的查询结果中
            var needManualCloseAlert = unCloseEnclosureAlarm.Where(x => sns.All(y => y != x.CustomField5));
            foreach (var monitoringAlert in needManualCloseAlert)
            {
                monitoringAlert.ResolutionState = this.CloseState.ResolutionState;
                monitoringAlert.Update("Manaul Close By SDK");
            }
        }

        #endregion
    }
}
