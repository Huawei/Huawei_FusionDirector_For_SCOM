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
// Assembly         : FusionDirectorPlugin.ViewLib
// Author           : mike
// Created          : 05-09-2019
//
// Last Modified By : mike
// Last Modified On : 05-09-2019
// ***********************************************************************
// <copyright file="FdApplianceConnector.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FusionDirectorPlugin.ViewLib.Client;
using FusionDirectorPlugin.ViewLib.Model;
using FusionDirectorPlugin.ViewLib.Utils;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Result = FusionDirectorPlugin.ViewLib.Model.Result;

namespace FusionDirectorPlugin.ViewLib.OM12R2
{
    /// <summary>
    /// Class FdApplianceDal.
    /// </summary>
    public class FdApplianceConnector
    {

        private const string FusionDirectUniqueIdPrefix = "FusionDirector::";

        #region Fields
        /// <summary>
        /// Gets the instance.
        /// </summary>
        private static FdApplianceConnector instance;
        #endregion

        #region Properties
        /// <summary>
        /// The base entity class.
        /// </summary>
        private ManagementPackClass baseEntityClass;

        /// <summary>
        /// Initializes a new instance of the <see cref="FdApplianceConnector" /> class.
        /// </summary>
        public FdApplianceConnector()
        {
            this.FdApplianceClass = MGroup.Instance.GetManagementPackClass(FdAppliance.EntityClassName);

            this.FdApplianceKey = this.FdApplianceClass.PropertyCollection["HostIP"];

            this.MontioringConnector = MGroup.Instance.GetConnector(MGroup.Instance.FdEntityConnectorGuid);
            if (!this.MontioringConnector.Initialized)
            {
                this.MontioringConnector.Initialize();
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static FdApplianceConnector Instance => instance ?? (instance = new FdApplianceConnector());

        /// <summary>
        /// Gets the base entity class.
        /// </summary>
        /// <value>The base entity class.</value>
        public ManagementPackClass BaseEntityClass => this.baseEntityClass ?? (this.baseEntityClass = MGroup.Instance.GetManagementPackClass("System.Entity"));

        /// <summary>
        /// Gets the display name field.
        /// </summary>
        /// <value>The display name field.</value>
        public ManagementPackProperty DisplayNameField => this.BaseEntityClass.PropertyCollection["DisplayName"];

        /// <summary>
        /// Gets or sets the enclosure class.
        /// </summary>
        /// <value>The enclosure class.</value>
        public ManagementPackClass FdApplianceClass { get; set; }

        /// <summary>
        /// Gets or sets the enclosure key.
        /// </summary>
        /// <value>The enclosure key.</value>
        public ManagementPackProperty FdApplianceKey { get; set; }

        /// <summary>
        /// Gets or sets the montioring connector.
        /// </summary>
        /// <value>The montioring connector.</value>
        public MonitoringConnector MontioringConnector { get; set; }
        #endregion

        /// <summary>
        /// Alls this instance.
        /// </summary>
        /// <returns>Task&lt;Result&lt;List&lt;EnterpriseManagementObject&gt;&gt;&gt;.</returns>
        public async Task<Result<List<EnterpriseManagementObject>>> All()
        {
            return await Task.Run(() =>
            {
                try
                {
                    var objects = MGroup.Instance.All(this.FdApplianceClass);
                    return Result<List<EnterpriseManagementObject>>.Done(objects.ToList());
                }
                catch (Exception e)
                {
                    return Result<List<EnterpriseManagementObject>>.Failed("Get List Error", e);
                }
            });
        }

        /// <summary>
        /// Finds the by host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <returns>Task&lt;Result&lt;EnterpriseManagementObject&gt;&gt;.</returns>
        public async Task<Result<EnterpriseManagementObject>> FindByHost(string host)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var objects = MGroup.Instance.Query(this.FdApplianceClass, $"HostIP='{host}'");
                    return Result<EnterpriseManagementObject>.Done(objects.FirstOrDefault());
                }
                catch (Exception e)
                {
                    return Result<EnterpriseManagementObject>.Failed("All Error", e);
                }
            });
        }

        /// <summary>
        /// Adds the specified appliance.
        /// </summary>
        /// <param name="appliance">The appliance.</param>
        /// <returns>Result.</returns>
        public async Task<Result> Add(FdAppliance appliance)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (MGroup.Instance.Exsits(this.FdApplianceClass, $"HostIP='{appliance.HostIP}'"))
                    {
                        return Result.Failed(101, $"FusionDirector {appliance.HostIP} already exsits.");
                    }

                    var incrementalDiscoveryData = new IncrementalDiscoveryData();
                    // add appliance record

                    var emoAppliance = new CreatableEnterpriseManagementObject(MGroup.Instance, this.FdApplianceClass);
                    var props = this.FdApplianceClass.PropertyCollection;
                    emoAppliance[props["UniqueId"]].Value = FusionDirectUniqueIdPrefix + Guid.NewGuid().ToString("D");
                    emoAppliance[props["HostIP"]].Value = appliance.HostIP;
                    emoAppliance[props["AliasName"]].Value = appliance.AliasName;
                    emoAppliance[props["LoginAccount"]].Value = appliance.LoginAccount;
                    emoAppliance[props["LoginPd"]].Value = RijndaelManagedCrypto.Instance.EncryptForCs(appliance.LoginPd);
                    emoAppliance[props["Port"]].Value = appliance.Port;
                    emoAppliance[props["EventUserName"]].Value = appliance.EventUserName;
                    emoAppliance[props["EventPd"]].Value = RijndaelManagedCrypto.Instance.EncryptForCs(appliance.EventPd);
                    emoAppliance[props["SubscribeId"]].Value = appliance.SubscribeId;
                    emoAppliance[props["SubscribeStatus"]].Value = appliance.SubscribeStatus;
                    emoAppliance[props["LatestSubscribeInfo"]].Value = appliance.LatestSubscribeInfo;
                    emoAppliance[props["LastModifyTime"]].Value = appliance.LastModifyTime;
                    emoAppliance[props["CreateTime"]].Value = appliance.CreateTime;

                    emoAppliance[this.DisplayNameField].Value = appliance.HostIP;
                    incrementalDiscoveryData.Add(emoAppliance);

                    incrementalDiscoveryData.Commit(MGroup.Instance);
                    return Result.Done();
                }
                catch (Exception e)
                {
                    return Result.Failed(100, $"Internal error caused by {e.Message}", e);
                }
            });
        }

        /// <summary>
        /// Updates the specified appliance.
        /// </summary>
        /// <param name="appliance">The appliance.</param>
        /// <param name="isUpdateCredential">是否修改了密码</param>
        /// <returns>Result.</returns>
        public async Task<Result> Update(FdAppliance appliance, bool isUpdateCredential)
        {
            return await Task.Run(async () =>
             {
                 try
                 {
                     var obj = await this.FindByHost(appliance.HostIP);
                     var exsitObj = obj.Data;
                     if (exsitObj == null)
                     {
                         return Result.Failed(104, $"Fd {appliance.HostIP} can not find.");
                     }
                     var incrementalDiscoveryData = new IncrementalDiscoveryData();

                     var props = this.FdApplianceClass.PropertyCollection;
                     exsitObj[props["AliasName"]].Value = appliance.AliasName;
                     exsitObj[props["Port"]].Value = appliance.Port;
                     exsitObj[props["LastModifyTime"]].Value = appliance.LastModifyTime;

                     if (isUpdateCredential)
                     {
                         try
                         {
                             var eventAccount = exsitObj[props["EventUserName"]].Value as string;
                             var eventPd = exsitObj[props["EventPd"]].Value as string;
                             if (eventAccount != appliance.EventUserName || eventPd != appliance.EventPd)
                             {
                                 using (var client = new FdClient(appliance))
                                 {
                                     var res = await client.DeleteGivenSubscriptions(appliance.SubscribeId);
                                     LogHelper.Info($"Update Fd:DeleteGivenSubscriptions:{res.Code} {res.Message}");
                                     // 取消订阅后重置订阅状态
                                     exsitObj[props["SubscribeId"]].Value = string.Empty;
                                     exsitObj[props["SubscribeStatus"]].Value = string.Empty;
                                     exsitObj[props["LatestSubscribeInfo"]].Value = string.Empty;
                                 }
                             }
                         }
                         catch (Exception ex)
                         {
                             LogHelper.Error("DeleteSubscriptions Faild", ex);
                         }
                         exsitObj[props["LoginAccount"]].Value = appliance.LoginAccount;
                         exsitObj[props["LoginPd"]].Value = RijndaelManagedCrypto.Instance.EncryptForCs(appliance.LoginPd);
                         exsitObj[props["EventUserName"]].Value = appliance.EventUserName;
                         exsitObj[props["EventPd"]].Value = RijndaelManagedCrypto.Instance.EncryptForCs(appliance.EventPd);
                     }

                     incrementalDiscoveryData.Add(exsitObj);
                     incrementalDiscoveryData.Overwrite(MGroup.Instance);

                     return Result.Done();
                 }
                 catch (Exception e)
                 {
                     return Result.Failed(100, $"Internal error caused by {e.Message}", e);
                 }
             });
        }

        /// <summary>
        /// Deletes the specified appliance.
        /// </summary>
        /// <param name="appliance">The appliance.</param>
        /// <returns>Task&lt;Result&gt;.</returns>
        public async Task<Result> Delete(FdAppliance appliance)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var obj = await this.FindByHost(appliance.HostIP);
                    var exsitObj = obj.Data;
                    if (exsitObj == null)
                    {
                        return Result.Failed(104, $"{appliance.HostIP} does not exists, delete failed.");
                    }
                    using (var client = new FdClient(appliance))
                    {
                        var res = await client.DeleteGivenSubscriptions(appliance.SubscribeId);
                        LogHelper.Info($"Delete Fd:DeleteGivenSubscriptions:{res.Code} {res.Message}");

                        var incrementalDiscoveryData = new IncrementalDiscoveryData();
                        incrementalDiscoveryData.Remove(exsitObj);
                        incrementalDiscoveryData.Commit(MGroup.Instance);
                        return Result.Done();
                    }
                }
                catch (Exception e)
                {
                    return Result.Failed(100, $"Internal error caused by {e.Message}", e);
                }
            });
        }
    }
}
