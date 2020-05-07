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
// Assembly         : FusionDirectorPlugin.Model
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="FusionDirector.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Dal.Model
{
    /// <summary>
    /// Class FusionDirector.
    /// </summary>
    [Serializable]
    public class FusionDirector 
    {
        /// <summary>
        /// Gets or sets the host ip.
        /// </summary>
        /// <value>The host ip.</value>
        [JsonProperty(PropertyName = "hostIp")]
        public string HostIP { get; set; }

        /// <summary>
        /// Gets or sets the name of the alias.
        /// </summary>
        /// <value>The name of the alias.</value>
        [JsonProperty(PropertyName = "aliasName")]
        public string AliasName { get; set; }

        /// <summary>
        /// Gets or sets the login account.
        /// </summary>
        /// <value>The login account.</value>
        [JsonProperty(PropertyName = "loginAccount")]
        public string LoginAccount { get; set; }

        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        [JsonProperty(PropertyName = "loginPd")]
        public string LoginPd { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        /// <value>The port.</value>
        [JsonProperty(PropertyName = "port")]
        public string Port { get; set; }

        /// <summary>
        /// 事件服务用户名
        /// </summary>
        /// <value>The cert path.</value>
        [JsonProperty(PropertyName = "eventUserName")]
        public string EventUserName { get; set; }

        /// <summary>
        /// 事件服务密码
        /// </summary>
        /// <value>The cert path.</value>
        [JsonProperty(PropertyName = "eventPd")]
        public string EventPd { get; set; }

        /// <summary>
        /// 订阅ID
        /// </summary>
        /// <value>The cert path.</value>
        [JsonProperty(PropertyName = "subscribeId")]
        public string SubscribeId { get; set; }

        /// <summary>
        /// 订阅状态
        /// </summary>
        /// <value>The latest status.</value>
        [JsonProperty(PropertyName = "subscribeStatus")]
        public string SubscribeStatus { get; set; }

        /// <summary>
        /// 上次订阅返回信息
        /// </summary>
        /// <value>The latest connect information.</value>
        [JsonProperty(PropertyName = "latestSubscribeInfo")]
        public string LatestSubscribeInfo { get; set; }
        
        /// <summary>
        /// Gets or sets the last modify time.
        /// </summary>
        /// <value>The last modify time.</value>
        [JsonProperty(PropertyName = "lastModify")]
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        [JsonProperty(PropertyName = "createTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the latest connect information.
        /// </summary>
        /// <value>The latest connect information.</value>
        [JsonIgnore]
        public string EventAuth=> "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.EventUserName + ":" + EventPd));

        #region MyRegion
        /// <summary>
        /// Gets or sets the director version.
        /// </summary>
        /// <value>The director version.</value>
        [JsonProperty(PropertyName = "directorVersion")]
        public string DirectorVersion { get; set; }

        /// <summary>
        /// Gets or sets the latest status.
        /// </summary>
        /// <value>The latest status.</value>
        [JsonProperty(PropertyName = "latestStatus")]
        public string LatestStatus { get; set; }

        /// <summary>
        /// Gets or sets the latest connect information.
        /// </summary>
        /// <value>The latest connect information.</value>
        [JsonProperty(PropertyName = "latestConnectInfo")]
        public string LatestConnectInfo { get; set; }

        /// <summary>
        /// Gets or sets the unique id of fd.
        /// </summary>
        /// <value>The the unique id of fd.</value>
        [JsonProperty(PropertyName = "uniqueId")]
        public string UniqueId { get; set; }

        #endregion

        public string Summary()
        {
            return $"HostIP={this.HostIP},AliasName={this.AliasName},Port={this.Port},LoginAccount={this.LoginAccount},EventUserName={this.EventUserName}";
        }
    }
}
