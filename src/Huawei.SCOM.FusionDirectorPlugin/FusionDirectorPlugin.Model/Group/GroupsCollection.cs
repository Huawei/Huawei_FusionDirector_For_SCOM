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
// <copyright file="GroupsCollection.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class GroupsCollection.
    /// </summary>
    public class GroupsCollection
    {
        /// <summary>
        /// NodeGroups集合资源模型的OData描述信息。
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// NodeGroups集合资源节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// NodeGroups集合资源类型。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>The groups.</value>
        [JsonProperty("Groups")]
        public ObservableCollection<Group> Groups { get; set; }
    }
}