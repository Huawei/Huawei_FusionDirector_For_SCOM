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
// <copyright file="SlotState.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using FusionDirectorPlugin.Model.EnumSe;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 槽位的状态。
    /// Enabled 槽位上有设备。
    /// Absent 槽位上没设备。
    /// StandbySpare 设备处于备用状态。
    /// </summary>
   [JsonConverter(typeof(SlotStateEnumConverter))]
    public enum SlotState
    {
        /// <summary>
        /// The enabled
        /// </summary>
        Enabled = 0,

        /// <summary>
        /// The absent
        /// </summary>
        Absent = 1,

        /// <summary>
        /// The standby spare
        /// </summary>
        StandbySpare = 2,
    }
}