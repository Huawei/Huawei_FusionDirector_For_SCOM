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
// <copyright file="NetworkAdapter.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class NetworkAdapter.
    /// </summary>
    public class NetworkAdapter
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
        /// 物理网卡的ID。
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("Id")]
        public string UUID { get; set; }

        /// <summary>
        /// Node资源节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataType { get; set; }

        /// <summary>
        /// 物理网卡的设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 物理网卡的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 物理网卡所在单板位置。
        /// </summary>
        /// <value>The device locator.</value>
        [JsonProperty("DeviceLocator")]
        public string DeviceLocator { get; set; }

        /// <summary>
        /// 物理网卡的驱动名称。
        /// </summary>
        /// <value>The name of the driver.</value>
        [JsonProperty("DriverName")]
        public string DriverName { get; set; }

        /// <summary>
        /// 物理网卡的驱动版本。
        /// </summary>
        /// <value>The driver version.</value>
        [JsonProperty("DriverVersion")]
        public string DriverVersion { get; set; }

        /// <summary>
        /// 物理网卡的生产厂商。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 物理网卡的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// MZ卡的名称。
        /// </summary>
        /// <value>The name of the card.</value>
        [JsonProperty("CardName")]
        public string CardName { get; set; }

        /// <summary>
        /// MZ卡的制造厂商。
        /// </summary>
        /// <value>The card manufacturer.</value>
        [JsonProperty("CardManufacturer")]
        public string CardManufacturer { get; set; }

        /// <summary>
        /// MZ卡的型号。
        /// </summary>
        /// <value>The card model.</value>
        [JsonProperty("CardModel")]
        public string CardModel { get; set; }

        /// <summary>
        /// 物理网卡的位置。
        /// </summary>
        /// <value>The position.</value>
        [JsonProperty("Position")]
        public string Position { get; set; }

        /// <summary>
        /// 物理网卡的物理网口集合信息。
        /// </summary>
        /// <value>The network port.</value>
        [JsonProperty("NetworkPort")]
        public ODataId NetworkPort { get; set; }

        /// <summary>
        /// 接口未返回状态-暂默认OK
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; } = new Status()
        {
            State = "Enabled",
            Health = Health.OK
        };

        /// <summary>
        /// Node的使能状态。支持的状态包括：Enabled、Absent。
        /// </summary>
        /// <value>The health.</value>
        public string EnableState => Status.State;

        /// <summary>
        /// Gets the health.
        /// </summary>
        /// <value>The health.</value>
        public Health Health => Status.Health;
    }
}