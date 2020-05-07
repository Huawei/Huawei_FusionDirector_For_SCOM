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
// Created          : 12-27-2018
//
// Last Modified By : panwei
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="EnclosureList.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class EnclosureList.
    /// </summary>
    public partial class EnclosureList
    {
        /// <summary>
        /// 用于odata的资源唯一标识符。
        /// </summary>
        /// <value>The o data identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 系统中该资源的总数。
        /// </summary>
        /// <value>The membersodata count.</value>
        [JsonProperty("Members@odata.count")]
        public int MembersodataCount { get; set; }

        /// <summary>
        /// $top 参数的值。
        /// </summary>
        /// <value>The top.</value>
        [JsonProperty("Top")]
        public int Top { get; set; }

        /// <summary>
        /// $skip 参数的值。
        /// </summary>
        /// <value>The skip.</value>
        [JsonProperty("Skip")]
        public int Skip { get; set; }

        /// <summary>
        /// 满足查询条件的资源集合。
        /// </summary>
        /// <value>The members.</value>
        [JsonProperty("Members")]
        public List<EnclosureSummary> Members { get; set; }
    }
}