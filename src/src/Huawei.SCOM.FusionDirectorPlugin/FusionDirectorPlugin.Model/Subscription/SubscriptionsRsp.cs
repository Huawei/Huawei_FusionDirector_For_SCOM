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
// <copyright file="SubscriptionsRsp.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 获取事件订阅的集合信息。
    /// </summary>

    public partial class SubscriptionsRsp
    {
        /// <summary>
        /// 用于odata的资源标识符。
        /// </summary>
        /// <value>The odata context.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// 用于odata的资源唯一标识符。
        /// </summary>
        /// <value>The odata identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 资源类型。
        /// </summary>
        /// <value>The type of the odata.</value>
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 系统中该资源的总数
        /// </summary>
        /// <value>The membersodata count.</value>
        [JsonProperty("Members@odata.count")]
        public double MembersOdataCount { get; set; }

        /// <summary>
        /// 满足查询条件的资源集合
        /// </summary>
        /// <value>The members.</value>
        [JsonProperty("Members")]
        public List<SubscriptionSummary> Members { get; set; } = new List<SubscriptionSummary>();

    }
}