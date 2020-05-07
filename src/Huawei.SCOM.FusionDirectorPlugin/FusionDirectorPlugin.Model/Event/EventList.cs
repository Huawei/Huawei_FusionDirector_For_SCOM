//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>获取告警列表的集合信息。</summary>

    public partial class EventList
    {
        /// <summary>用于odata的资源标识符。</summary>
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        /// <summary>用于odata的资源唯一标识符。</summary>
        [JsonProperty("@odata.id")]
        public string OdataId { get; set; }

        /// <summary>资源类型。</summary>
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        /// <summary>满足查询条件的告警列表集合</summary>
        [JsonProperty("Members")]
        public List<EventSummary> Members { get; set; } = new List<EventSummary>();

        /// <summary>系统中该资源的总数</summary>
        [JsonProperty("Members@odata.count")]
        public int MembersCount { get; set; }

    }
}