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
// <copyright file="Slot.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class Slot.
    /// </summary>
    public partial class Slot
    {
        /// <summary>
        /// Gets or sets the server slot.
        /// </summary>
        /// <value>The server slot.</value>
        [JsonProperty("ServerSlot")]
        public List<ServerSlot> ServerSlot { get; set; } = new List<ServerSlot>();

        /// <summary>
        /// Gets or sets the switch slot.
        /// </summary>
        /// <value>The switch slot.</value>
        [JsonProperty("SwitchSlot")]
        public List<SwitchSlot> SwitchSlot { get; set; } = new List<SwitchSlot>();

        /// <summary>
        /// Gets or sets the manager slot.
        /// </summary>
        /// <value>The manager slot.</value>
        [JsonProperty("ManagerSlot")]
        public List<ManagerSlot> ManagerSlot { get; set; } = new List<ManagerSlot>();

        /// <summary>
        /// Gets or sets the fan slot.
        /// </summary>
        /// <value>The fan slot.</value>
        [JsonProperty("FanSlot")]
        public List<FanSlot> FanSlot { get; set; } = new List<FanSlot>();

        /// <summary>
        /// Gets or sets the power slot.
        /// </summary>
        /// <value>The power slot.</value>
        [JsonProperty("PowerSlot")]
        public List<PowerSlot> PowerSlot { get; set; } = new List<PowerSlot>();
    }
}