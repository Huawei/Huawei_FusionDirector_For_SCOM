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
// <copyright file="Power.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class Power.
    /// </summary>
    public class Power
    {
        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.DeviceId}";

        /// <summary>
        /// DeviceId-手动赋值
        /// </summary>
        /// <value>The physical UUID.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// 指定电源模块的ID，为其在电源模块列表中的唯一标识。
        /// </summary>
        /// <value>The member identifier.</value>
        [JsonProperty("MemberId")]
        public string MemberId { get; set; }

        /// <summary>
        /// 指定电源模块的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 指定电源模块的供电类型。包括：Unknown ; AC ; DC ; ACorDC 。
        /// </summary>
        /// <value>The type of the power supply.</value>
        [JsonProperty("PowerSupplyType")]
        public string PowerSupplyType { get; set; }

        /// <summary>
        /// 指定电源模块的输出功率(额定功率)。
        /// </summary>
        /// <value>The power capacity watts.</value>
        [JsonProperty("PowerCapacityWatts")]
        public double? PowerCapacityWatts { get; set; }

        /// <summary>
        /// 指定电源模块的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 指定电源模块的固件版本号。
        /// </summary>
        /// <value>The firmware version.</value>
        [JsonProperty("FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// 指定电源模块的序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 指定电源模块的部件号。
        /// </summary>
        /// <value>The part number.</value>
        [JsonProperty("PartNumber")]
        public string PartNumber { get; set; }

        /// <summary>
        /// 指定电源模块的生产商。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 自定义信息。
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public object Oem { get; set; }

        /// <summary>
        /// 指定电源模块的状态。
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