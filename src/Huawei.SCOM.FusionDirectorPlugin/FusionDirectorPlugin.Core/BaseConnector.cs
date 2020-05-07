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
// Author           : panwei
// Created          : 01-04-2019
//
// Last Modified By : panwei
// Last Modified On : 01-05-2019
// ***********************************************************************
// <copyright file="BaseConnector.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using FusionDirectorPlugin.Core.Model;
using FusionDirectorPlugin.LogUtil;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.Monitoring;
using Newtonsoft.Json;
using MPObject = Microsoft.EnterpriseManagement.Common.CreatableEnterpriseManagementObject;

namespace FusionDirectorPlugin.Core
{

    /// <summary>
    /// The base connector.
    /// </summary>
    public class BaseConnector
    {
        #region Fields

        /// <summary>
        /// The base entity class.
        /// </summary>
        private ManagementPackClass baseEntityClass;

        /// <summary>
        /// The part group class
        /// </summary>
        private ManagementPackClass partGroupClass;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the base entity class.
        /// </summary>
        /// <value>The base entity class.</value>
        public ManagementPackClass BaseEntityClass => this.baseEntityClass ?? (this.baseEntityClass = MGroup.Instance.GetManagementPackClass("System.Entity"));

        /// <summary>
        /// The display name field.
        /// </summary>
        public ManagementPackProperty DisplayNameField => this.BaseEntityClass.PropertyCollection["DisplayName"];

        /// <summary>
        /// Gets or sets the montioring connector.
        /// </summary>
        /// <value>The montioring connector.</value>
        public MonitoringConnector MontioringConnector { get; set; }

        /// <summary>
        /// Gets the part group class.
        /// </summary>
        public ManagementPackClass PartGroupClass => this.partGroupClass ?? (this.partGroupClass = MGroup.Instance.GetManagementPackClass("FusionDirector.BaseGroup"));

        /// <summary>
        /// The part group key.
        /// </summary>
        public ManagementPackProperty PartGroupKey => this.PartGroupClass.PropertyCollection["ID"];

        #endregion

        #region Event

        /// <summary>
        /// The _close state
        /// </summary>
        private MonitoringAlertResolutionState closeState;

        /// <summary>
        /// Gets the state of the close.
        /// </summary>
        /// <value>The state of the close.</value>
        public MonitoringAlertResolutionState CloseState
        {
            get
            {
                return this.closeState ?? (this.closeState = MGroup.Instance.OperationalData.GetMonitoringAlertResolutionStates().ToList().FirstOrDefault(x => x.Name == "Closed"));
            }
        }


        /// <summary>
        /// The new state
        /// </summary>
        private MonitoringAlertResolutionState newState;

        /// <summary>
        /// Gets the state of the New.
        /// </summary>
        /// <value>The state of the close.</value>
        public MonitoringAlertResolutionState NewState
        {
            get
            {
                return this.newState ?? (this.newState = MGroup.Instance.OperationalData.GetMonitoringAlertResolutionStates().ToList().FirstOrDefault(x => x.Name == "New"));
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// 创建逻辑Group
        /// </summary>
        /// <param name="mpClass">The class.</param>
        /// <param name="parentKey">The parent key.</param>
        /// <returns>Microsoft.EnterpriseManagement.Common.CreatableEnterpriseManagementObject.</returns>
        public MPObject CreateLogicalGroup(ManagementPackClass mpClass, string parentKey)
        {
            var obj = new MPObject(MGroup.Instance, mpClass); // 实例化一个class
            obj[this.PartGroupKey].Value = parentKey + "-" + mpClass.DisplayName;
            obj[this.DisplayNameField].Value = mpClass.DisplayName;
            return obj;
        }

        /// <summary>
        /// UnionId是否存在
        /// </summary>
        /// <param name="unionId">The device identifier.</param>
        /// <param name="mpClass">The mp class.</param>
        /// <returns>PartialMonitoringObject.</returns>
        public bool ExsitsUnionId(string unionId, ManagementPackClass mpClass)
        {
            MGroup.Instance.CheckConnection();
            var criteria = new MonitoringObjectCriteria($"UnionId = '{unionId}'", mpClass);
            var reader =
                MGroup.Instance.EntityObjects.GetObjectReader<PartialMonitoringObject>(criteria, ObjectQueryOptions.Default);
            return reader.Any();
        }

        /// <summary>
        /// The get object.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="mpClass">The class.</param>
        /// <returns>The <see cref="MonitoringObject" />.</returns>
        public MonitoringObject GetObject(string expression, ManagementPackClass mpClass)
        {
            MGroup.Instance.CheckConnection();
            var criteria = new MonitoringObjectCriteria(expression, mpClass);
            var reader =
                MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(criteria, ObjectQueryOptions.Default);
            return reader.FirstOrDefault();
        }

        #endregion

        /// <summary>
        /// Gets the object by union identifier.
        /// </summary>
        /// <param name="mpClass">The mp class.</param>
        /// <param name="unionId">The union identifier.</param>
        /// <returns>PartialMonitoringObject.</returns>
        public PartialMonitoringObject GetObjectByUnionId(ManagementPackClass mpClass, string unionId)
        {
            var criteria = new MonitoringObjectCriteria($"UnionId = '{unionId}'", mpClass);
            var reader = MGroup.Instance.EntityObjects.GetObjectReader<PartialMonitoringObject>(criteria, ObjectQueryOptions.Default);
            if (!reader.Any())
            {
                return null;
                //throw new Exception($"cannot find unionId :'{unionId}'");
            }
           return reader.First();
        }

        /// <summary>
        /// Get monitoring device object by "SCOM Object Id"
        /// </summary>
        /// <param name="objectId">SCOM Object Id</param>
        /// <returns></returns>
        public static MonitoringDeviceObject GetDeviceByObjectId(ManagementPackClass mpClass, string objectId)
        {
            var criteria = new MonitoringObjectCriteria($"UnionId = '{objectId}'", mpClass);
            var objectReader = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(criteria, ObjectQueryOptions.Default);
            if (!objectReader.Any())
            {
                return null;
            }

            return new MonitoringDeviceObject(objectId, mpClass, objectReader.FirstOrDefault());
        }

        /// <summary>
        /// Gets the parent object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>PartialMonitoringObject.</returns>
        protected PartialMonitoringObject GetParentObject(PartialMonitoringObject obj)
        {
            var group = obj.GetParentPartialMonitoringObjects();
            if (group.Any())
            {
                var parent = group.First();
                var t = parent.GetParentPartialMonitoringObjects();
                if (t.Any())
                {
                    return t.First();
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the full parent object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>PartialMonitoringObject.</returns>
        protected MonitoringObject GetFullParentObject(PartialMonitoringObject obj)
        {
            var group = obj.GetParentPartialMonitoringObjects();
            if (group.Any())
            {
                var parent = group.First();
                var t = parent.GetParentMonitoringObjects();
                if (t.Any())
                {
                    return t.First();
                }
            }
            return null;
        }

        #region Event

        /// <summary>
        /// Inserts the event.
        /// </summary>
        /// <param name="mpClass">The mp class.</param>
        /// <param name="eventData">The event data.</param>
        public void InsertEvent(ManagementPackClass mpClass, EventData eventData)
        {
            var logger = HWLogger.GetFdSdkLogger(eventData.FusionDirectorIp);
            try
            {
                var sn = eventData.AlarmSn.ToString();
                MGroup.Instance.CheckConnection();
                var logPre = $"[Sn={sn}] [OptType={eventData.OptType}] [LevelId={eventData.LevelId}] ";
                //logger.Info($"[Sn:{sn}] Start Deal Event.[{JsonConvert.SerializeObject(eventData)}]");
                var obj = GetObjectByUnionId(mpClass, eventData.UnionId);
                if (obj == null)
                {
                    logger.Warn($"{logPre} Can not find the MonitoringObject:{eventData.UnionId}");
                    return;
                }
                var isReady = CheckAndWaitHealthStateReady(mpClass, obj, eventData);
                if (!isReady)
                {
                    logger.Warn($"{logPre} The MonitoringObject state is uninitialized.Drop the event.");
                    return;
                }
                var eventHistory = obj.GetMonitoringEvents().ToList();
                switch (eventData.OptType)
                {
                    case "1":
                        #region 告警
                        //如果不存在，则插入
                        //如果上次安装时的事件未清除，本次同步后，一个sn会存在两条数据，需要取最新添加的一条
                        var existEvent = eventHistory.OrderByDescending(x => x.TimeAdded).FirstOrDefault(x => x.GetAlarmData().Sn.ToString() == sn);
                        if (existEvent == null || existEvent.TimeAdded < MGroup.Instance.MpInstallTime)
                        {
                            obj.InsertCustomMonitoringEvent(eventData.ToCustomMonitoringEvent());
                            logger.Info($"{logPre}Insert new Event.");
                            if (eventData.LevelId == EventLogEntryType.Error || eventData.LevelId == EventLogEntryType.Warning)
                            {
                                if (eventData.AlarmData.Status == "Cleared")//如果告警是清除状态
                                {
                                    logger.Info($"{logPre}Need to close Event when insert.");
                                    Task.Run(() =>
                                    {
                                        int i = 0;
                                        while (i < 10)
                                        {
                                            i++;
                                            Thread.Sleep(TimeSpan.FromMinutes(1));
                                            var alertToClose = obj.GetMonitoringAlerts().FirstOrDefault(x => x.CustomField5 == sn);
                                            if (alertToClose != null)
                                            {
                                                alertToClose.ResolutionState = this.CloseState.ResolutionState;
                                                var comment = !string.IsNullOrEmpty(eventData.AlarmData.ClearType) ? eventData.AlarmData.ClearType : eventData.AlarmData.Additional;
                                                alertToClose.Update(comment);
                                                logger.Info($"{logPre}Close Event success.");
                                                break;
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        else
                        {
                            #region 存在则更新
                            var alertHistory = obj.GetMonitoringAlerts();
                            var alertToUpdate = alertHistory.FirstOrDefault(x => x.CustomField5 == sn);
                            if (alertToUpdate != null)
                            {
                                alertToUpdate.CustomField2 = eventData.AlarmData.AlarmId;
                                alertToUpdate.CustomField3 = eventData.AlarmData.AlarmName.Split('#').Last();
                                alertToUpdate.CustomField4 = eventData.AlarmData.ResourceId;
                                alertToUpdate.CustomField5 = eventData.AlarmData.Sn.ToString();
                                alertToUpdate.CustomField7 = eventData.AlarmData.Additional;
                                alertToUpdate.CustomField8 = eventData.AlarmData.Suggestion;
                                alertToUpdate.CustomField8 = eventData.AlarmData.OccurTime;
                                alertToUpdate.CustomField9 = eventData.AlarmData.PossibleCause;
                                alertToUpdate.CustomField10 = eventData.AlarmData.Effect;

                                alertToUpdate.Update(eventData.AlarmData.Additional);
                                logger.Debug($"{logPre}Update Event.");
                                if (eventData.AlarmData.Status == "Cleared")//如果告警是清除状态
                                {
                                    alertToUpdate.ResolutionState = this.CloseState.ResolutionState;
                                    alertToUpdate.Update(eventData.AlarmData.Additional);
                                    logger.Info($"{logPre}Close Alert On Update Event.");
                                }
                                else
                                {
                                    //如果原来的告警是关闭状态，本次是Open,则重新打开告警
                                    if (alertToUpdate.ResolutionState == this.CloseState.ResolutionState)
                                    {
                                        alertToUpdate.ResolutionState = this.NewState.ResolutionState;
                                        alertToUpdate.Update(eventData.AlarmData.Additional);
                                        logger.Info($"{logPre}Reopen Alert On Update Event.");
                                    }
                                }
                            }
                            else
                            {
                                logger.Warn($"{logPre}Ingore Event.Can not find the alert.");
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    case "2":
                        #region 清除告警
                        if (eventData.LevelId == EventLogEntryType.Error || eventData.LevelId == EventLogEntryType.Warning)//清除告警
                        {
                            var alertHistory = obj.GetMonitoringAlerts();
                            var alertToClose = alertHistory.FirstOrDefault(x => x.CustomField5 == sn);
                            if (alertToClose != null)
                            {
                                alertToClose.ResolutionState = this.CloseState.ResolutionState;
                                var comment = !string.IsNullOrEmpty(eventData.AlarmData.ClearType) ? eventData.AlarmData.ClearType : eventData.AlarmData.Additional;
                                alertToClose.Update(comment);
                                logger.Info($"{logPre}Close Event.");
                            }
                            else
                            {
                                logger.Warn($"{logPre}Ingore Event.Can not find the alert.");
                            }
                        }
                        else
                        {
                            logger.Warn($"{logPre}Ignore Event."); //忽略事件
                        }
                        #endregion
                        break;
                    case "3"://肯定是事件
                        #region 插入事件
                        if (eventData.LevelId == EventLogEntryType.Information)
                        {
                            var existAlarmDatas = obj.GetMonitoringEvents().Select(x => x.GetAlarmData()).ToList();
                            //插入事件
                            if (existAlarmDatas.All(x => x.Sn.ToString() != sn))
                            {
                                logger.Info($"{logPre}Insert new Event.");
                                obj.InsertCustomMonitoringEvent(eventData.ToCustomMonitoringEvent());
                            }
                            else
                            {
                                logger.Warn($"{logPre}Ignore Event.The event is exist."); //忽略已存在
                            }
                        }
                        else
                        {
                            logger.Warn($"{logPre}Ignore Event."); //忽略非事件
                        }
                        #endregion
                        break;
                    default:
                        logger.Error($"{logPre}Unknown OptType.");
                        break;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        /// <summary>
        /// 插入告警时，等待新增的对象的healthState不再是Not Monitor
        /// </summary>
        /// <param name="mpClass">The mp class.</param>
        /// <param name="obj">The object.</param>
        /// <param name="eventData">The event data.</param>
        /// <returns>PartialMonitoringObject.</returns>
        public bool CheckAndWaitHealthStateReady(ManagementPackClass mpClass, PartialMonitoringObject obj, EventData eventData)
        {
            var logger = HWLogger.GetFdSdkLogger(eventData.FusionDirectorIp);
            if (obj.StateLastModified == null)
            {
                //如果对象添加超过5分钟，仍然没有健康状态，防止阻塞只查询一次
                if ((DateTime.Now - obj.TimeAdded).TotalMinutes > 5)
                {
                    obj = GetObjectByUnionId(mpClass, eventData.UnionId); ;
                    if (obj.HealthState != HealthState.Uninitialized)
                    {
                        logger.Info($"{eventData.UnionId} first healthState is {obj.HealthState}.");
                        return true;
                    }
                    return false;
                }
                #region 新增对象
                logger.Info($"New Object:{eventData.UnionId}");
                int i = 0;
                while (i < 48)
                {
                    i++;
                    // 重新查询obj状态
                    obj = GetObjectByUnionId(mpClass, eventData.UnionId); ;
                    if (obj.HealthState != HealthState.Uninitialized)
                    {
                        logger.Info($"{eventData.UnionId} first healthState is {obj.HealthState}.");
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        return true;
                    }
                    logger.Info($"wait {eventData.UnionId} first Initialized...");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                return false;
                #endregion
            }
            return true;
        }
        #endregion
    }
}