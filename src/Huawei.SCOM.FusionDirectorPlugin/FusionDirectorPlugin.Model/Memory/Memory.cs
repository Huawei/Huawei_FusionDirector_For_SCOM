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
// <copyright file="Memory.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 指定Node上内存资源列表。
    /// </summary>
    public class Memory
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

        ///// <summary>
        ///// 单个内存的ID。
        ///// </summary>
        ///// <value>The identifier.</value>
        //[JsonProperty("Id")]
        //public string Id { get; set; }

        /// <summary>
        /// 单个内存的设备ID
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 单个内存的名称
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 单个内存容量，单位GByte
        /// </summary>
        /// <value>The capacity gi b.</value>
        [JsonProperty("CapacityGiB")]
        public double? CapacityGiB { get; set; }

        /// <summary>
        /// 单个内存的生产厂商
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 单个内存的运行速度，单位MHz。
        /// </summary>
        /// <value>The operating speed MHZ.</value>
        [JsonProperty("OperatingSpeedMhz")]
        public double? OperatingSpeedMhz { get; set; }

        /// <summary>
        /// 单个内存的序列号
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 单个内存的设备类型
        /// </summary>
        /// <value>The type of the memory device.</value>
        [JsonProperty("MemoryDeviceType")]
        public string MemoryDeviceType { get; set; }

        /// <summary>
        /// 单个内存位宽，单位bit
        /// </summary>
        /// <value>The data width bits.</value>
        [JsonProperty("DataWidthBits")]
        public double? DataWidthBits { get; set; }

        ///// <summary>
        ///// 单个内存的槽位号
        ///// </summary>
        ///// <value>The slot.</value>
        //[JsonProperty("Slot")]
        //public double? Slot { get; set; }

        /// <summary>
        /// 单个内存上的自定义信息
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public JObject Oem { get; set; }

        #region Oem

        /// <summary>
        /// Gets the technology.
        /// </summary>
        /// <value>The technology.</value>
        public string Technology => Oem.GetString("Huawei.Technology");

        /// <summary>
        /// Gets the minimum voltage millivolt.
        /// </summary>
        /// <value>The minimum voltage millivolt.</value>
        public string MinVoltageMillivolt => Oem.GetString("Huawei.MinVoltageMillivolt");

        #endregion

        /// <summary>
        /// 单个内存的状态信息
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