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
// Created          : 05-07-2019
//
// Last Modified By : mike
// Last Modified On : 05-07-2019
// ***********************************************************************
// <copyright file="FdAppliance.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;

namespace FusionDirectorPlugin.ViewLib.Model
{
    /// <summary>
    /// Class FdAppliance.
    /// </summary>
    public class FdAppliance
    {
        /// <summary>
        /// The entity class name
        /// </summary>
        public const string EntityClassName = "FusionDirector.Entity";

        /// <summary>
        /// Initializes a new instance of the <see cref="FdAppliance"/> class.
        /// </summary>
        public FdAppliance()
        {
            this.HostIP = string.Empty;
            this.AliasName = string.Empty;
            this.LoginAccount = string.Empty;
            this.LoginPd = string.Empty;
            this.Port = string.Empty;
            this.EventUserName = string.Empty;
            this.EventPd = string.Empty;
            this.SubscribeId = string.Empty;
            this.SubscribeStatus = string.Empty;
            this.LatestSubscribeInfo = string.Empty;
            this.LastModifyTime = DateTime.Now;
            this.CreateTime = DateTime.Now;
            this.DirectorVersion = string.Empty;
            this.LatestStatus = string.Empty;
            this.LatestConnectInfo = string.Empty;
            this.UniqueId = string.Empty;
        }

        /// <summary>
        /// Gets or sets the host ip.
        /// </summary>
        /// <value>The host ip.</value>
        public string HostIP { get; set; }

        /// <summary>
        /// Gets or sets the name of the alias.
        /// </summary>
        /// <value>The name of the alias.</value>
        public string AliasName { get; set; }

        /// <summary>
        /// Gets or sets the login account.
        /// </summary>
        /// <value>The login account.</value>
        public string LoginAccount { get; set; }

        /// <summary>
        /// Gets or sets the login password.
        /// </summary>
        /// <value>The login password.</value>
        public string LoginPd { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        /// <value>The port.</value>
        public string Port { get; set; }

        /// <summary>
        /// 事件服务用户名
        /// </summary>
        /// <value>The cert path.</value>
        public string EventUserName { get; set; }

        /// <summary>
        /// 事件服务密码
        /// </summary>
        /// <value>The cert path.</value>
        public string EventPd { get; set; }

        /// <summary>
        /// 订阅ID
        /// </summary>
        /// <value>The cert path.</value>
        public string SubscribeId { get; set; }

        /// <summary>
        /// 订阅状态
        /// </summary>
        /// <value>The latest status.</value>
        public string SubscribeStatus { get; set; }

        /// <summary>
        /// 上次订阅返回信息
        /// </summary>
        /// <value>The latest connect information.</value>
        public string LatestSubscribeInfo { get; set; }

        /// <summary>
        /// Gets or sets the last modify time.
        /// </summary>
        /// <value>The last modify time.</value>
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>The create time.</value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Gets or sets the latest connect information.
        /// </summary>
        /// <value>The latest connect information.</value>
        public string EventAuth => "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(this.EventUserName + ":" + this.EventPd));

        #region MyRegion
        /// <summary>
        /// Gets or sets the director version.
        /// </summary>
        /// <value>The director version.</value>
        public string DirectorVersion { get; set; }

        /// <summary>
        /// Gets or sets the latest status.
        /// </summary>
        /// <value>The latest status.</value>
        public string LatestStatus { get; set; }

        /// <summary>
        /// Gets or sets the latest connect information.
        /// </summary>
        /// <value>The latest connect information.</value>
        public string LatestConnectInfo { get; set; }

        /// <summary>
        /// Gets or sets the unique id of fd.
        /// </summary>
        /// <value>The the unique id of fd.</value>
        public string UniqueId { get; set; }

        #endregion

        public string Summary()
        {
            return $"HostIP={this.HostIP},AliasName={this.AliasName},Port={this.Port},LoginAccount={this.LoginAccount},EventUserName={this.EventUserName}";
        }
    }
}
