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
// <copyright file="Drive.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerDriveInfo.
    /// </summary>
    public class Drive
    {
        ///// <summary>
        ///// 指定硬盘的Id。
        ///// </summary>
        ///// <value>The identifier.</value>
        //[JsonProperty("Id")]
        //public string Id { get; set; }

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
        /// 指定Node资源类型
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// 指定硬盘接口最大速率。
        /// </summary>
        /// <value>The capable speed GBS.</value>
        [JsonProperty("CapableSpeedGbs")]
        public double? CapableSpeedGbs { get; set; }

        /// <summary>
        /// 指定硬盘容量，单位为GB。
        /// </summary>
        /// <value>The capacity gi b.</value>
        [JsonProperty("CapacityGiB")]
        public double? CapacityGiB { get; set; }

        /// <summary>
        /// 指定硬盘的设备Id。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 指定驱动器的定位指示灯状态，包括：Off Blinking
        /// </summary>
        /// <value>The indicator led.</value>
        [JsonProperty("IndicatorLED")]
        public string IndicatorLED { get; set; }

        /// <summary>
        /// 指定硬盘的制造商信息。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 指定硬盘的介质类型。
        /// </summary>
        /// <value>The type of the media.</value>
        [JsonProperty("MediaType")]
        public string MediaType { get; set; }

        /// <summary>
        /// 指定硬盘的型号信息。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 指定硬盘的名称信息。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 自定义信息
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public object Oem { get; set; }

        /// <summary>
        /// 指定驱动器遵从的协议。
        /// </summary>
        /// <value>The protocol.</value>
        [JsonProperty("Protocol")]
        public string Protocol { get; set; }

        /// <summary>
        /// 指定驱动器的版本信息。
        /// </summary>
        /// <value>The revision.</value>
        [JsonProperty("Revision")]
        public string Revision { get; set; }

        /// <summary>
        /// 指定驱动器的序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 硬盘的状态信息。
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; }

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