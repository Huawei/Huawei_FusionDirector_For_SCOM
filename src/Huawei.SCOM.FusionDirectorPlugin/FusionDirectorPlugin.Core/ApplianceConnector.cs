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
// Created          : 02-20-2019
//
// Last Modified By : yayun
// Last Modified On : 02-21-2019
// ***********************************************************************
// <copyright file="ApplianceConnector.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FusionDirectorPlugin.Core.Const;
using FusionDirectorPlugin.Core.Model;
using FusionDirectorPlugin.Core.Models;
using FusionDirectorPlugin.LogUtil;
using FusionDirectorPlugin.Model.Event;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement.Common;
using MPObject = Microsoft.EnterpriseManagement.Common.CreatableEnterpriseManagementObject;

namespace FusionDirectorPlugin.Core
{
    /// <summary>
    /// Class ApplianceConnector.
    /// </summary>
    /// <seealso cref="FusionDirectorPlugin.Core.BaseConnector" />
    public class ApplianceConnector : BaseConnector
    {
        #region Fields

        /// <summary>
        /// The instance
        /// </summary>
        private static ApplianceConnector instance;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the appliance class.
        /// </summary>
        /// <value>The appliance class.</value>
        public ManagementPackClass ApplianceClass { get; set; }

        /// <summary>
        /// Gets or sets the enclosure collection class.
        /// </summary>
        /// <value>The enclosure collection class.</value>
        public ManagementPackClass EnclosureCollectionClass { get; set; }

        /// <summary>
        /// Gets or sets the server collection class.
        /// </summary>
        /// <value>The server collection class.</value>
        public ManagementPackClass ServerCollectionClass { get; set; }

        /// <summary>
        /// Gets or sets the event collection class.
        /// </summary>
        /// <value>The event collection class.</value>
        public ManagementPackClass EventCollectionClass { get; set; }

        /// <summary>
        /// Gets or sets the performance collection class.
        /// </summary>
        /// <value>The performance collection class.</value>
        public ManagementPackClass PerformanceCollectionClass { get; set; }

        public ManagementPackClass FusionDirectorCollectionClass { get; set; }

        /// <summary>
        /// Gets or sets the appliance key.
        /// </summary>
        /// <value>The appliance key.</value>
        public ManagementPackProperty ApplianceKey { get; set; }
        #endregion

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ApplianceConnector Instance => instance ?? (instance = new ApplianceConnector());


        /// <summary>
        /// Initializes a new instance of the <see cref="ApplianceConnector"/> class.
        /// </summary>
        public ApplianceConnector()
        {
            this.ApplianceClass = MGroup.Instance.GetManagementPackClass(ApplianceConst.MainName);
            this.EnclosureCollectionClass = MGroup.Instance.GetManagementPackClass(ApplianceConst.EnclosureCollection);
            this.ServerCollectionClass = MGroup.Instance.GetManagementPackClass(ApplianceConst.ServerCollection);
            this.EventCollectionClass = MGroup.Instance.GetManagementPackClass(ApplianceConst.EventCollection);
            this.PerformanceCollectionClass = MGroup.Instance.GetManagementPackClass(ApplianceConst.PerformanceCollection);
            this.FusionDirectorCollectionClass = MGroup.Instance.GetManagementPackClass(ApplianceConst.FusionDirectorCollection);

            this.ApplianceKey = this.ApplianceClass.PropertyCollection["HostName"];

            this.MontioringConnector = MGroup.Instance.GetConnector(MGroup.Instance.ApplianceConnectorGuid);
            if (!this.MontioringConnector.Initialized)
            {
                this.MontioringConnector.Initialize();
            }
        }

        #region Public Methods

        /// <summary>
        /// Updates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="isPolling">if set to <c>true</c> [is polling].</param>
        /// <exception cref="System.Exception"></exception>
        public void Sync(Appliance model, bool isPolling)
        {
            try
            {
                HWLogger.Service.Debug($"Start UpdateAppliance. [isPolling:{isPolling}]");
                var devices = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(ApplianceClass, ObjectQueryOptions.Default).ToList();
                var exsitAppliance = devices.FirstOrDefault();
                if (exsitAppliance == null)
                {
                    throw new Exception($"Can not find the Appliance ");
                }
                var discoveryData = new IncrementalDiscoveryData();
                this.UpdateAppliance(model, exsitAppliance);
                discoveryData.Add(exsitAppliance);

                #region EnclosureCollection
                var enclosureCollections = exsitAppliance.GetRelatedMonitoringObjects(this.EnclosureCollectionClass);
                if (enclosureCollections.Any())
                {
                    var exsit = enclosureCollections.First();
                    this.UpdateEnclosureCollection(model.EnclosureCollection, exsit);
                    discoveryData.Add(exsit);
                }
                else
                {
                    var enclosureCollection = this.CreateEnclosureCollection(model.EnclosureCollection);
                    discoveryData.Add(enclosureCollection);
                }
                #endregion

                #region ServerCollection
                var serverCollections = exsitAppliance.GetRelatedMonitoringObjects(this.ServerCollectionClass);
                if (serverCollections.Any())
                {
                    var exsit = serverCollections.First();
                    this.UpdateServerCollection(model.ServerCollection, exsit);
                    discoveryData.Add(exsit);
                }
                else
                {
                    var serverCollection = this.CreateServerCollection(model.ServerCollection);
                    discoveryData.Add(serverCollection);
                }
                #endregion

                #region EventCollection
                var eventCollections = exsitAppliance.GetRelatedMonitoringObjects(this.EventCollectionClass);
                if (eventCollections.Any())
                {
                    var exsit = eventCollections.First();
                    this.UpdateEventCollection(model.EventCollection, exsit);
                    discoveryData.Add(exsit);
                }
                else
                {
                    var eventCollection = this.CreateEventCollection(model.EventCollection);
                    discoveryData.Add(eventCollection);
                }
                #endregion

                #region PerformanceCollection
                var performanceCollections = exsitAppliance.GetRelatedMonitoringObjects(this.PerformanceCollectionClass);
                if (performanceCollections.Any())
                {
                    var exsit = performanceCollections.First();
                    this.UpdatePerformanceCollection(model.PerformanceCollection, exsit);
                    discoveryData.Add(exsit);
                }
                else
                {
                    var performanceCollection = this.CreatePerformanceCollection(model.PerformanceCollection);
                    discoveryData.Add(performanceCollection);
                }
                #endregion

                #region FusionDirectorCollection
                var fusionDirectorCollections = exsitAppliance.GetRelatedMonitoringObjects(this.FusionDirectorCollectionClass);
                if (fusionDirectorCollections.Any())
                {
                    var exsit = fusionDirectorCollections.First();
                    this.UpdateFusionDirectorCollection(model.FusionDirectorCollection, exsit);
                    discoveryData.Add(exsit);
                }
                else
                {
                    var fusionDirectorCollection = this.CreateFusionDirectorCollection(model.FusionDirectorCollection);
                    discoveryData.Add(fusionDirectorCollection);
                }
                #endregion

                discoveryData.Overwrite(this.MontioringConnector);
                HWLogger.Service.Info($"UpdateAppliance finish.");
            }
            catch (Exception e)
            {
                HWLogger.Service.Error(e, $"Update Appliance Error. [isPolling:{isPolling}]");
            }
        }

        #endregion

        #region Create And Update

        /// <summary>
        /// Updates the appliance.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existObj">The exist object.</param>
        private void UpdateAppliance(Appliance model, MonitoringObject existObj)
        {
            var propertys = this.ApplianceClass.PropertyCollection;
            existObj[propertys["HostName"]].Value = model.HostName;
            existObj[propertys["IPAddress"]].Value = model.IPAddress;
            existObj[propertys["SoftwareVersion"]].Value = model.SoftwareVersion;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.HostName;
        }

        /// <summary>
        /// Creates the enclosure collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>MPObject.</returns>
        private MPObject CreateEnclosureCollection(EnclosureCollection model)
        {
            var propertys = this.EnclosureCollectionClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.EnclosureCollectionClass);
            obj[propertys["ResourceName"]].Value = model.ResourceName;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = model.ResourceName;
            return obj;
        }

        /// <summary>
        /// Updates the enclosure collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existObj">The exist object.</param>
        private void UpdateEnclosureCollection(EnclosureCollection model, MonitoringObject existObj)
        {
            var propertys = this.EnclosureCollectionClass.PropertyCollection;
            existObj[propertys["ResourceName"]].Value = model.ResourceName;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.ResourceName;
        }

        /// <summary>
        /// Creates the server collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>MPObject.</returns>
        private MPObject CreateServerCollection(ServerCollection model)
        {
            var propertys = this.ServerCollectionClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.ServerCollectionClass);
            obj[propertys["ResourceName"]].Value = model.ResourceName;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = model.ResourceName;
            return obj;
        }

        /// <summary>
        /// Updates the server collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existObj">The exist object.</param>
        private void UpdateServerCollection(ServerCollection model, MonitoringObject existObj)
        {
            var propertys = this.ServerCollectionClass.PropertyCollection;
            existObj[propertys["ResourceName"]].Value = model.ResourceName;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.ResourceName;
        }

        /// <summary>
        /// Creates the event collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>MPObject.</returns>
        private MPObject CreateEventCollection(EventCollection model)
        {
            var propertys = this.EventCollectionClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.EventCollectionClass);
            obj[propertys["ResourceName"]].Value = model.ResourceName;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = model.ResourceName;
            return obj;
        }

        /// <summary>
        /// Updates the event collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existObj">The exist object.</param>
        private void UpdateEventCollection(EventCollection model, MonitoringObject existObj)
        {
            var propertys = this.EventCollectionClass.PropertyCollection;
            existObj[propertys["ResourceName"]].Value = model.ResourceName;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.ResourceName;
        }

        /// <summary>
        /// Creates the performance collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>MPObject.</returns>
        private MPObject CreatePerformanceCollection(PerformanceCollection model)
        {
            var propertys = this.PerformanceCollectionClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.PerformanceCollectionClass);
            obj[propertys["ResourceName"]].Value = model.ResourceName;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = model.ResourceName;
            return obj;
        }

        /// <summary>
        /// Updates the performance collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existObj">The exist object.</param>
        private void UpdatePerformanceCollection(PerformanceCollection model, MonitoringObject existObj)
        {
            var propertys = this.PerformanceCollectionClass.PropertyCollection;
            existObj[propertys["ResourceName"]].Value = model.ResourceName;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.ResourceName;
        }

        /// <summary>
        /// Creates the fusionDirector collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>MPObject.</returns>
        private MPObject CreateFusionDirectorCollection(FusionDirectorCollection model)
        {
            var propertys = this.FusionDirectorCollectionClass.PropertyCollection;
            var obj = new MPObject(MGroup.Instance, this.FusionDirectorCollectionClass);
            obj[propertys["ResourceName"]].Value = model.ResourceName;
            obj[propertys["Health"]].Value = model.Health.ToString();
            obj[this.DisplayNameField].Value = model.ResourceName;
            return obj;
        }

        /// <summary>
        /// Updates the fusionDirector collection.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="existObj">The exist object.</param>
        private void UpdateFusionDirectorCollection(FusionDirectorCollection model, MonitoringObject existObj)
        {
            var propertys = this.FusionDirectorCollectionClass.PropertyCollection;
            existObj[propertys["ResourceName"]].Value = model.ResourceName;
            existObj[propertys["Health"]].Value = model.Health.ToString();
            existObj[this.DisplayNameField].Value = model.ResourceName;
        }
        #endregion

        #region Removes

        /// <summary>
        /// Removes Appliance.
        /// </summary>
        public void RemoveAppliance()
        {
            try
            {
                HWLogger.Service.Info($"RemoveAppliance.");
                MGroup.Instance.CheckConnection();
                var devices = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(ApplianceClass, ObjectQueryOptions.Default).ToList();
                if (devices.Any())
                {
                    var discovery = new IncrementalDiscoveryData();
                    devices.ForEach(device => discovery.Remove(device));
                    discovery.Commit(this.MontioringConnector);
                }
            }
            catch (Exception ex)
            {
                HWLogger.Service.Error(ex, "RemoveAppliance");
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Inserts the event.
        /// </summary>
        /// <param name="mpClass">The mp class.</param>
        /// <param name="eventData">The event data.</param>
        public void InsertEvent(ApplianceEvent eventData)
        {
            var logger = HWLogger.Service;
            try
            {
                var alarmType = eventData.AlarmData.AlarmType.ToString();
                MGroup.Instance.CheckConnection();
                var logPre = $"ApplianceEvent：[alarmType:{alarmType}] [OptType={eventData.OptType}] [LevelId={eventData.LevelId}] ";
                var obj = GetAppliance();
                if (obj == null)
                {
                    logger.Warn($"{logPre} Can not find the ApplianceObject");
                    return;
                }
                var isReady = CheckAndWaitHealthStateReady(obj);
                if (!isReady)
                {
                    logger.Warn($"{logPre} The MonitoringObject state is uninitialized.Drop the event.");
                    return;
                }
                var alertHistory = this.GetUnclosedAlert();
                switch (eventData.OptType)
                {
                    case "1":
                        #region 告警
                        //如果不存在，则插入
                        var alertToUpdate = alertHistory.FirstOrDefault(x => x.CustomField1 == alarmType.ToString());
                        if (alertToUpdate == null || alertToUpdate.TimeAdded < MGroup.Instance.MpInstallTime)
                        {
                            obj.InsertCustomMonitoringEvent(eventData.ToCustomMonitoringEvent());
                            logger.Debug($"{logPre}Insert new Event.");
                        }
                        else
                        {
                            #region 存在则更新
                            if (alertToUpdate != null)
                            {
                                alertToUpdate.CustomField4 = eventData.AlarmData.PossibleCause;
                                alertToUpdate.CustomField5 = eventData.AlarmData.Additional;
                                alertToUpdate.Update(eventData.AlarmData.Additional);
                                logger.Debug($"{logPre}Update Event.");
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
                        if (eventData.LevelId == 1 || eventData.LevelId == 2)//清除告警
                        {
                            var alertToClose = alertHistory.FirstOrDefault(x => x.CustomField1 == alarmType);
                            if (alertToClose != null)
                            {
                                alertToClose.ResolutionState = this.CloseState.ResolutionState;
                                alertToClose.Update("Close by sdk.");
                                logger.Debug($"{logPre}Close Event.");
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
        /// Inserts the event.
        /// </summary>
        /// <param name="mpClass">The mp class.</param>
        /// <param name="eventData">The event data.</param>
        public void InsertEvent(ApplianceEvent eventData, string fusionDirectorIp)
        {
            var logger = HWLogger.Service;
            try
            {
                var alarmType = eventData.AlarmData.AlarmType.ToString();
                MGroup.Instance.CheckConnection();
                var logPre = $"ApplianceEvent：[alarmType:{alarmType}] [OptType={eventData.OptType}] [LevelId={eventData.LevelId}] ";
                var obj = GetAppliance();
                if (obj == null)
                {
                    logger.Warn($"{logPre} Can not find the ApplianceObject");
                    return;
                }
                var isReady = CheckAndWaitHealthStateReady(obj);
                if (!isReady)
                {
                    logger.Warn($"{logPre} The MonitoringObject state is uninitialized.Drop the event.");
                    return;
                }
                var alertHistory = this.GetUnclosedAlert();
                switch (eventData.OptType)
                {
                    case "1":
                        #region 告警
                        //如果不存在，则插入
                        var alertToUpdate = alertHistory.FirstOrDefault(x => x.CustomField1 == alarmType && x.CustomField4.Contains(fusionDirectorIp));
                        if (alertToUpdate == null || alertToUpdate.TimeAdded < MGroup.Instance.MpInstallTime)
                        {
                            obj.InsertCustomMonitoringEvent(eventData.ToCustomMonitoringEvent());
                            logger.Debug($"{logPre}Insert new Event.");
                        }
                        else
                        {
                            #region 存在则更新
                            if (alertToUpdate != null)
                            {
                                alertToUpdate.CustomField4 = eventData.AlarmData.PossibleCause;
                                alertToUpdate.CustomField5 = eventData.AlarmData.Additional;
                                alertToUpdate.Update(eventData.AlarmData.Additional);
                                logger.Debug($"{logPre}Update Event.");
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
                        if (eventData.LevelId == 1 || eventData.LevelId == 2)//清除告警
                        {
                            var alertToClose = alertHistory.FirstOrDefault(x => x.CustomField1 == alarmType && x.CustomField4.Contains(fusionDirectorIp));
                            if (alertToClose != null)
                            {
                                alertToClose.ResolutionState = this.CloseState.ResolutionState;
                                alertToClose.Update("Close by sdk.");
                                logger.Debug($"{logPre}Close Event.");
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
        /// <param name="obj">The object.</param>
        /// <returns>PartialMonitoringObject.</returns>
        public bool CheckAndWaitHealthStateReady(PartialMonitoringObject obj)
        {
            var logger = HWLogger.Service;
            if (obj.StateLastModified == null)
            {
                //如果对象添加超过5分钟，仍然没有健康状态，防止阻塞只查询一次
                if ((DateTime.Now - obj.LastModified).TotalMinutes > 5)
                {
                    obj = GetAppliance();
                    if (obj.HealthState != HealthState.Uninitialized)
                    {
                        logger.Debug($"Appliance first healthState is {obj.HealthState}.");
                        return true;
                    }
                    return false;
                }
                #region 新增对象
                logger.Debug($"New Object:Appliance");
                int i = 0;
                while (i < 48)
                {
                    i++;
                    // 重新查询obj状态
                    obj = GetAppliance();
                    if (obj.HealthState != HealthState.Uninitialized)
                    {
                        logger.Debug($"Appliance first healthState is {obj.HealthState}.");
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        return true;
                    }
                    logger.Debug($"wait Appliance first Initialized...");
                    Thread.Sleep(TimeSpan.FromSeconds(5));
                }
                return false;
                #endregion
            }
            return true;
        }

        /// <summary>
        /// Gets the history alarm datas.
        /// </summary>
        /// <returns>List&lt;AlarmData&gt;.</returns>
        public List<MonitoringAlert> GetUnclosedAlert()
        {
            //读取所有的未关闭的告警
            var unCloseEnclosureAlarm = MGroup.Instance.OperationalData.GetMonitoringAlerts(
                new MonitoringAlertCriteria($"ResolutionState = '0'"),
                this.ApplianceClass, TraversalDepth.OneLevel, null).ToList();
            return unCloseEnclosureAlarm;
        }
        #endregion

        private MonitoringObject GetAppliance()
        {
            var devices = MGroup.Instance.EntityObjects.GetObjectReader<MonitoringObject>(ApplianceClass, ObjectQueryOptions.Default).ToList();
            return devices.FirstOrDefault();
        }

    }
}
