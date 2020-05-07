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
// <copyright file="ServerCatalogue.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerCatalogue.
    /// </summary>
    public class ServerCatalogue
    {
        /// <summary>
        /// 指定Node上Catalogue资源模型的OData描述信息。
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// 指定Node上Catalogue资源的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Node上资源类型
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// 指定Node上内存。
        /// </summary>
        /// <value>The memory.</value>
        [JsonProperty("Memory")]
        public CatalogueItem Memory { get; set; }

        /// <summary>
        /// 指定Node上电源。
        /// </summary>
        /// <value>The power.</value>
        [JsonProperty("Power")]
        public CatalogueItem Power { get; set; }

        /// <summary>
        /// 指定Node上处理器。
        /// </summary>
        /// <value>The processor.</value>
        [JsonProperty("Processor")]
        public CatalogueItem Processor { get; set; }

        /// <summary>
        /// 指定Node上硬盘存储。
        /// </summary>
        /// <value>The storage.</value>
        [JsonProperty("Storage")]
        public CatalogueItem Storage { get; set; }

        /// <summary>
        /// 指定Node上风扇
        /// </summary>
        /// <value>The thermal.</value>
        [JsonProperty("Thermal")]
        public CatalogueItem Thermal { get; set; }
    }
}