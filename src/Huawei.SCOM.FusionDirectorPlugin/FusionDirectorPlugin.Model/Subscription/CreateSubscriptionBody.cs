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
// Author           : yayun
// Created          : 01-11-2019
//
// Last Modified By : yayun
// Last Modified On : 01-11-2019
// ***********************************************************************
// <copyright file="CreateSubscriptionBody.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class CreateSubscriptionBody.
    /// </summary>
    public partial class CreateSubscriptionBody
    {
        /// <summary>
        /// 事件订阅接收地址，合法的https接收地址。
        /// </summary>
        /// <value>The destination.</value>
        [JsonProperty("Destination", Required = Required.Always)]
        public string Destination { get; set; }

        /// <summary>
        /// 事件订阅监听的事件类型，取值范围为StatusChange：资源状态改变事件，ResourceUpdated：资源更新事件，ResourceAdded：资源添加事件，ResourceRemoved：资源移除事件，Alert：告警事件。
        /// </summary>
        /// <value>The event types.</value>
        [JsonProperty("EventTypes", Required = Required.Always)]
        public List<string> EventTypes { get; set; }

        /// <summary>
        /// 事件订阅上下文信息，字符串。
        /// </summary>
        /// <value>The context.</value>
        [JsonProperty("Context")]
        public string Context { get; set; }

        /// <summary>
        /// 协议类型。
        /// </summary>
        /// <value>The protocol.</value>
        [JsonProperty("Protocol", Required = Required.Always)]
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the oem.
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem", Required = Required.Always)]
        public Oem Oem { get; set; } = new Oem();


    }
}