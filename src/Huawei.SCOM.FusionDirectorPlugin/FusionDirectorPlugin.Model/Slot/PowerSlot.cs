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
// <copyright file="PowerSlot.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 电源槽位信息。
    /// </summary>
    public partial class PowerSlot
    {
        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.UUID}";

        /// <summary>
        /// ID-手动赋值
        /// </summary>
        /// <value>The physical UUID.</value>
        public string UUID { get; set; }

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// 槽位号。
        /// </summary>
        /// <value>The index.</value>
        [JsonProperty("Index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty("State")]
        public SlotState SlotState { get; set; }

        /// <summary>
        /// 电源类型。
        /// </summary>
        /// <value>The type of the power supply.</value>
        [JsonProperty("PowerSupplyType")]
        public string PowerSupplyType { get; set; }

        /// <summary>
        /// 指定电源模块的输出功率(额定功率)。
        /// </summary>
        /// <value>The power capacity watts.</value>
        [JsonProperty("PowerCapacityWatts")]
        public string PowerCapacityWatts { get; set; }

        /// <summary>
        /// 序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 固件版本。
        /// </summary>
        /// <value>The firmware version.</value>
        [JsonProperty("FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// 硬件版本。
        /// </summary>
        /// <value>The hardware version.</value>
        [JsonProperty("HardwareVersion")]
        public string HardwareVersion { get; set; }

        /// <summary>
        /// 休眠状态。
        /// </summary>
        /// <value>The sleep status.</value>
        [JsonProperty("SleepStatus")]
        public string SleepStatus { get; set; }
 
        //public string Name => $"PowerSlot{Index + 1}";

        /// <summary>
        /// 名称
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 健康状态
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]
        public Health Health { get; set; } = Health.Warning;

    }
}