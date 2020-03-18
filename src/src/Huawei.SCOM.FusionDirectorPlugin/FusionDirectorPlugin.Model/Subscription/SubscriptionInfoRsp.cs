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
// <copyright file="SubscriptionInfoRsp.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class SubscriptionInfoRsp.
    /// </summary>
    public partial class SubscriptionInfo
    {
        /// <summary>
        /// 用于odata的资源标识符。
        /// </summary>
        /// <value>The odata context.</value>
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        /// <summary>
        /// 用于odata的资源唯一标识符。
        /// </summary>
        /// <value>The odata identifier.</value>
        [JsonProperty("@odata.id")]
        public string OdataId { get; set; }

        /// <summary>
        /// 资源类型。
        /// </summary>
        /// <value>The type of the odata.</value>
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        /// <summary>
        /// 资源Id。
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 资源名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 事件上报第三方的URI。
        /// </summary>
        /// <value>The destination.</value>
        [JsonProperty("Destination")]
        public string Destination { get; set; }

        /// <summary>
        /// 上报事件类型
        /// </summary>
        /// <value>The event types.</value>
        [JsonProperty("EventTypes")]
        public List<EventType> EventTypes { get; set; } = new List<EventType>();

        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>The context.</value>
        [JsonProperty("Context")]
        public string Context { get; set; }

        /// <summary>
        /// 上报事件协议
        /// </summary>
        /// <value>The protocol.</value>
        [JsonProperty("Protocol")]
        public string Protocol { get; set; }

        /// <summary>
        /// Gets or sets the oem.
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public Oem Oem { get; set; } = new Oem();
    }
}