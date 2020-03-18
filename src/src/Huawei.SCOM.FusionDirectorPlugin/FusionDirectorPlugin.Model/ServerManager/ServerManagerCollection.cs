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
// Created          : 01-28-2019
//
// Last Modified By : yayun
// Last Modified On : 01-28-2019
// ***********************************************************************
// <copyright file="ServerManagerCollection.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using Newtonsoft.Json;
using System.Collections.Generic;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerManagerCollection.
    /// </summary>
    public partial class ServerManagerCollection
    {
        /// <summary>
        /// Node资源模型的OData描述信息。
        /// </summary>
        /// <value>The o data context.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// 指定Node资源节点的访问路径。
        /// </summary>
        /// <value>The o data identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 指定Node资源类型。
        /// </summary>
        /// <value>The type of the o data.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// 指定管理资源额设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        [JsonProperty("EthernetInterface")]
        public List<EthernetInterface> EthernetInterfaces { get; set; } = new List<EthernetInterface>();

        /// <summary>
        /// 当前管理资源的网口数量。
        /// </summary>
        /// <value>The ethernet interfacecount.</value>
        [JsonProperty("EthernetInterface@odata.count")]
        public double? EthernetInterfacecount { get; set; }

        /// <summary>
        /// 指定管理资源的FW版本号。
        /// </summary>
        /// <value>The firmware version.</value>
        [JsonProperty("FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// 指定管理资源的ID。
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 指定管理资源的详细类型。
        /// </summary>
        /// <value>The type of the manager.</value>
        [JsonProperty("ManagerType")]
        public string ManagerType { get; set; }

        /// <summary>
        /// 指定管理资源的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 指定管理资源的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 当前管理资源的网口数量。
        /// </summary>
        /// <value>The total count.</value>
        [JsonProperty("TotalCount")]
        public double? TotalCount { get; set; }
    }
}