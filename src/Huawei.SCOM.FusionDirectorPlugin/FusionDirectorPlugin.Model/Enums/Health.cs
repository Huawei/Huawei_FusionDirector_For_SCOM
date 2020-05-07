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
// <copyright file="Health.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using FusionDirectorPlugin.Model.EnumSe;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 健康状态。
    /// OK 健康。
    /// Warning 有轻微健康问题。
    /// Critical 有严重健康问题。
    /// </summary>
   [JsonConverter(typeof(HealthEnumConverter))]
    public enum Health
    {
        /// <summary>
        /// The ok
        /// </summary>
        OK = 0,

        /// <summary>
        /// The warning
        /// </summary>
        Warning = 1,

        /// <summary>
        /// The critical
        /// </summary>
        Critical = 2,
    }
}