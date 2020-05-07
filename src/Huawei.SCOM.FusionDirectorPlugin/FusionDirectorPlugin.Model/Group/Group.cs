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
// <copyright file="Group.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class Group.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// NodeGroups资源指定节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// NodeGroups资源的分组描述信息。
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// NodeGroups资源的分组使能类型，标志是否可修改和删除。
        /// </summary>
        /// <value><c>null</c> if [enabled] contains no value, <c>true</c> if [enabled]; otherwise, <c>false</c>.</value>
        [JsonProperty("Enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// NodeGroups资源的分组资源编号。
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("ID")]
        public double? ID { get; set; }

        /// <summary>
        /// NodeGroups资源的分组名。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// NodeGroups资源的分组类型。
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("Type")]
        public string Type { get; set; }
    }
}