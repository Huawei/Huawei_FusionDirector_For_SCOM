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
// Created          : 12-26-2018
//
// Last Modified By : mike
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="ServerList.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerList.
    /// </summary>
    public class ServerList
    {
        /// <summary>
        /// Node资源列表模型的OData描述信息。
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// Node资源列表节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Node资源列表类型。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// Node资源列表中返回的Node数量。
        /// </summary>
        /// <value>The membersodatacount.</value>
        [JsonProperty("Members@odata.count")]
        public int? Membersodatacount { get; set; }

        /// <summary>
        /// NodeGroups集合资源列表。
        /// </summary>
        /// <value>The members.</value>
        [JsonProperty("Members")]
        public List<ServerSummary> Members { get; set; }
    }
}