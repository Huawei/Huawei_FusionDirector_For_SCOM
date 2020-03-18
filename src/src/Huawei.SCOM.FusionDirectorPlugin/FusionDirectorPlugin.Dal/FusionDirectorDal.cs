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
// Assembly         : FusionDirectorPlugin.Dal
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="FusinoDirectorDal.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using CommonUtil;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.ViewLib.Model;
using FusionDirectorPlugin.ViewLib.OM12R2;
using Microsoft.EnterpriseManagement.Common;

namespace FusionDirectorPlugin.Dal
{
    /// <summary>
    /// Class FusinoDirectorDal.
    /// </summary>
    public class FusionDirectorDal
    {

        /// <summary>
        /// 单例
        /// </summary>
        /// <value>The instance.</value>
        public static FusionDirectorDal Instance => SingletonProvider<FusionDirectorDal>.Instance;

        /// <summary>
        /// The get list.
        /// </summary>
        /// <returns>The <see><cref>IList</cref></see>
        /// .</returns>
        /// <exception cref="System.Exception">ex</exception>
        public IList<FusionDirector> GetList()
        {
            var monitoringObjects = FdApplianceConnector.Instance.All().Result.Data;
            return monitoringObjects.Select(GetModelFromMpObject).OrderByDescending(x => x.CreateTime).ToList();
        }

        /// <summary>
        /// 根据IP查找Fd实体。
        /// </summary>
        /// <param name="hostIp">The host ip.</param>
        /// <returns>The <see cref="FdAppliance" />.</returns>
        public FusionDirector GetEntity(string hostIP)
        {
            var managementObject = FdApplianceConnector.Instance.FindByHost(hostIP).Result.Data;
            if (managementObject == null)
            {
                throw new Exception($"Can not find the FusionDirector:{hostIP}");
            }
            return GetModelFromMpObject(managementObject);
        }

        /// <summary>
        /// Updates the subscribe status.
        /// </summary>
        /// <param name="hostIP">The host ip.</param>
        /// <param name="subscribeStatus">The subscribe status.</param>
        /// <param name="latestSubscribeInfo">The latest subscribe information.</param>
        /// <param name="subscribeId">The subscribe identifier.</param>
        public void UpdateSubscribeStatus(string hostIP, string subscribeStatus, string latestSubscribeInfo, string subscribeId)
        {
            var managementObject = FdApplianceConnector.Instance.FindByHost(hostIP).Result.Data;
            if (managementObject == null)
            {
                throw new Exception($"Can not find the FusionDirector:{hostIP}");
            }

            var props = FdApplianceConnector.Instance.FdApplianceClass.PropertyCollection;
            managementObject[props["SubscribeId"]].Value = DateTime.UtcNow;
            managementObject[props["SubscribeStatus"]].Value = subscribeStatus;
            managementObject[props["LatestSubscribeInfo"]].Value = latestSubscribeInfo;
            managementObject[props["LastModifyTime"]].Value = DateTime.Now;

            managementObject.Overwrite();
        }

        /// <summary>
        /// Gets the model from mp object.
        /// </summary>
        /// <param name="managementObject">The management object.</param>
        /// <returns>FdAppliance.</returns>
        private FusionDirector GetModelFromMpObject(EnterpriseManagementObject managementObject)
        {
            var props = FdApplianceConnector.Instance.FdApplianceClass.PropertyCollection;
            var model = new FusionDirector();
            model.HostIP = managementObject[props["HostIP"]].Value.ToString();
            model.AliasName = managementObject[props["AliasName"]].Value.ToString();
            model.LoginAccount = managementObject[props["LoginAccount"]].Value.ToString();
            model.LoginPd = RijndaelManagedCrypto.Instance.DecryptFromCs(managementObject[props["LoginPd"]].Value.ToString());
            model.Port = managementObject[props["Port"]].Value.ToString();
            model.EventUserName = managementObject[props["EventUserName"]].Value.ToString();
            model.EventPd = RijndaelManagedCrypto.Instance.DecryptFromCs(managementObject[props["EventPd"]].Value.ToString());
            model.SubscribeId = managementObject[props["SubscribeId"]].Value.ToString();
            model.SubscribeStatus = managementObject[props["SubscribeStatus"]].Value.ToString();
            model.LatestSubscribeInfo = managementObject[props["LatestSubscribeInfo"]].Value.ToString();
            model.LastModifyTime = Convert.ToDateTime(managementObject[props["LastModifyTime"]].Value.ToString());
            model.CreateTime = Convert.ToDateTime(managementObject[props["CreateTime"]].Value.ToString());

            return model;
        }
    }
}
