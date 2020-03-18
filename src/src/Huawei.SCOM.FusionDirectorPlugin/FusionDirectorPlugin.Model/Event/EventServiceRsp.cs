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
// <copyright file="EventServiceRsp.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 查询服务信息。
    /// </summary>

    public partial class EventServiceRsp
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
        /// 微服务ID。
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 微服务名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 微服务状态。
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; } = new Status();

        /// <summary>
        /// 重试发送次数。
        /// </summary>
        /// <value>The delivery retry attempts.</value>
        [JsonProperty("DeliveryRetryAttempts")]
        public double DeliveryRetryAttempts { get; set; }

        /// <summary>
        /// 重试发送时间间隔。
        /// </summary>
        /// <value>The delivery retry interval seconds.</value>
        [JsonProperty("DeliveryRetryIntervalSeconds")]
        public double DeliveryRetryIntervalSeconds { get; set; }

        /// <summary>
        /// 订阅事件类型。
        /// </summary>
        /// <value>The event types for subscription.</value>
        [JsonProperty("EventTypesForSubscription")]
        public List<EventType> EventTypesForSubscription { get; set; } = new List<EventType>();

        /// <summary>
        /// 事件订阅。
        /// </summary>
        /// <value>The subscriptions.</value>
        [JsonProperty("Subscriptions")]
        public ODataId Subscriptions { get; set; } = new ODataId();

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>The actions.</value>
        [JsonProperty("Actions")]
        public object Actions { get; set; }

    }
}