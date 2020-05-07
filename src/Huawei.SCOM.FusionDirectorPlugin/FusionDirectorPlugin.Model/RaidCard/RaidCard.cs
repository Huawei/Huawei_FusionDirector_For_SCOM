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
// <copyright file="ServerRaidCardInfo.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerRaidCardInfo.
    /// </summary>
    public class RaidCard
    {
        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.DeviceID}";

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// Node资源模型的OData描述信息。
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// 指定Node资源节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 指定Node资源类型。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// 指定Raid卡的ID。
        /// </summary>
        /// <value>The identifier.</value>
        //[JsonProperty("Id")]
        //public string Id { get; set; }

        /// <summary>
        /// 指定Raid卡的设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 指定Raid卡的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 存储控制器的信息。
        /// </summary>
        /// <value>The storage controllers.</value>
        [JsonProperty("StorageControllers")]
        public List<StorageController> StorageControllers { get; set; }

        /// <summary>
        /// 控制器的数量。
        /// </summary>
        /// <value>The storage controllersodatacount.</value>
        [JsonProperty("StorageControllers@odata.count")]
        public double? StorageControllersCount { get; set; }

        /// <summary>
        /// 逻辑盘集合资源的链接。
        /// </summary>
        /// <value>The volume.</value>
        [JsonProperty("Volume")]
        public ODataId Volume { get; set; }

        /// <summary>
        /// Node资源上PCIe信息。
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public JObject Oem { get; set; }

       /// <summary>
        /// Gets the health.
        /// </summary>
        /// <value>The health.</value>
        public Health Health
        {
            get
            {
                if (this.StorageControllers.Any(x => x.Health == Health.Critical))
                {
                    return Health.Critical;
                }
                if (this.StorageControllers.Any(x => x.Health == Health.Warning))
                {
                    return Health.Warning;
                }
                return Health.OK;
            }
        }
    }
}