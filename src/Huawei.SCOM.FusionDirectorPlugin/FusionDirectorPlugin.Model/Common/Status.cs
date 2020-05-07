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
// <copyright file="Status.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Node的状态信息。
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Node的使能状态。支持的状态包括：Enabled、Absent。
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty("State")]
        public string State { get; set; }

        /// <summary>
        /// Node的健康级别。支持的健康级别包括：OK、Warning、Critical。
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]
        public Health Health { get; set; }= Health.Warning;
    }
}