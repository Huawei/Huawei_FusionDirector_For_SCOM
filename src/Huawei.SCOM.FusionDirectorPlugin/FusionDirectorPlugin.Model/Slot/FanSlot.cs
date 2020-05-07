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
// <copyright file="FanSlot.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 风扇槽位信息。
    /// </summary>
    public partial class FanSlot
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
        /// PCB版本号。
        /// </summary>
        /// <value>The PCB version.</value>
        [JsonProperty("PcbVersion")]
        public string PcbVersion { get; set; }

        /// <summary>
        /// 软件版本号。
        /// </summary>
        /// <value>The software version.</value>
        [JsonProperty("SoftwareVersion")]
        public string SoftwareVersion { get; set; }

        //public string Name => $"FanSlot{Index + 1}";

        /// <summary>
        /// 名称
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]
        public Health Health { get; set; } = Health.Warning;
    }
}