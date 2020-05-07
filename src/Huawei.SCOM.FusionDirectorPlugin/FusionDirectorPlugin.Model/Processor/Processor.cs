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
// <copyright file="Processor.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class Processor.
    /// </summary>
    public class Processor
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
        ///// 单个CPU的ID。
        ///// </summary>
        ///// <value>The identifier.</value>
        //[JsonProperty("Id")]
        //public string Id { get; set; }

        /// <summary>
        /// 单个处理器的设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 单个CPU的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 单个CPU的架构。
        /// </summary>
        /// <value>The processor architecture.</value>
        [JsonProperty("ProcessorArchitecture")]
        public string ProcessorArchitecture { get; set; }

        /// <summary>
        /// 单个CPU的指令集。
        /// </summary>
        /// <value>The instruction set.</value>
        [JsonProperty("InstructionSet")]
        public string InstructionSet { get; set; }

        /// <summary>
        /// 单个CPU的生产厂商。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 单个CPU的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 单个CPU的运行速度，单位MHz。
        /// </summary>
        /// <value>The maximum speed m hz.</value>
        [JsonProperty("MaxSpeedMHz")]
        public double? MaxSpeedMHz { get; set; }

        /// <summary>
        /// 单个CPU的槽位号。
        /// </summary>
        /// <value>The socket.</value>
        [JsonProperty("Socket")]
        public double? Socket { get; set; }

        /// <summary>
        /// 单个CPU的总核数。
        /// </summary>
        /// <value>The total cores.</value>
        [JsonProperty("TotalCores")]
        public double? TotalCores { get; set; }

        /// <summary>
        /// 单个CPU的总线程数。
        /// </summary>
        /// <value>The total threads.</value>
        [JsonProperty("TotalThreads")]
        public double? TotalThreads { get; set; }

        /// <summary>
        /// 单个CPU上的自定义信息。
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public JObject Oem { get; set; }

        #region Oem

        /// <summary>
        /// Gets the temperature.
        /// </summary>
        /// <value>The temperature.</value>
        public string Temperature => Oem.GetString("Huawei.Temperature");

        /// <summary>
        /// Gets the frequency m hz.
        /// </summary>
        /// <value>The frequency m hz.</value>
        public string FrequencyMHz => Oem.GetString("Huawei.FrequencyMHz");

        /// <summary>
        /// Gets the part number.
        /// </summary>
        /// <value>The part number.</value>
        public string PartNumber => Oem.GetString("Huawei.PartNumber");
        #endregion

        /// <summary>
        /// Gets or sets the status.
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
        /// 健康状态
        /// </summary>
        /// <value>The health.</value>
        public Health Health => Status.Health;
    }
}