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
// <copyright file="SwitchInfo.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class SwitchInfo.
    /// </summary>
    public class SwitchInfo
    {
        /// <summary>
        /// Switch资源模型的OData描述信息。
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// Switch详细信息访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 指定Switch资源类型。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// Switch上的设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// Switch的UUID。
        /// </summary>
        /// <value>The UUID.</value>
        [JsonProperty("UUID")]
        public string UUID { get; set; }

        /// <summary>
        /// 指定Switch的ID。
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("Id")]
        public string Id { get; set; }

        /// <summary>
        /// 指定Switch的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 指定Switch的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 指定Switch的制造商。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 指定Switch的序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 指定Switch的主机名。
        /// </summary>
        /// <value>The name of the host.</value>
        [JsonProperty("HostName")]
        public string HostName { get; set; }

        /// <summary>
        /// 指定Switch的部件号。
        /// </summary>
        /// <value>The part number.</value>
        [JsonProperty("PartNumber")]
        public string PartNumber { get; set; }

        /// <summary>
        /// 指定Switch的Bios版本。
        /// </summary>
        /// <value>The bios version.</value>
        [JsonProperty("BiosVersion")]
        public string BiosVersion { get; set; }

        /// <summary>
        /// 指定Switch的IPv4信息。
        /// </summary>
        /// <value>The i PV4 address.</value>
        [JsonProperty("IPv4Address")]
        public IPv4Address IPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; } = new Status();

        /// <summary>
        /// 交换板状态。
        /// </summary>
        /// <value>The state of the switch.</value>
        [JsonProperty("SwitchState")]
        public string SwitchState { get; set; }

        /// <summary>
        /// 指定Switch的Board信息。
        /// </summary>
        /// <value>The board.</value>
        [JsonProperty("Board")]
        public ODataId Board { get; set; }

        /// <summary>
        /// 自定义信息。
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public object Oem { get; set; }

        /// <summary>
        /// 健康状态-需手动再次查询
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]
        public Health Health => Status.Health;

        /// <summary>
        /// Gets the state of the enabled.
        /// </summary>
        /// <value>The state of the enabled.</value>
        public string EnabledState => Status.State;
    }
}