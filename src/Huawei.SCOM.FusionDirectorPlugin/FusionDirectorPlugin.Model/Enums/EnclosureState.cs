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
// <copyright file="EnclosureState.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using FusionDirectorPlugin.EnumConverter;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 机框的状态。
    /// Added 机框已经被添加，但还没准备就绪。
    /// Ready 机框已经准备就绪，可以进行操作。
    /// Locked机框因执行任务被锁定。
    /// Unmanaged 机框处于未被管理的状态。
    /// </summary>
   [JsonConverter(typeof(EnclosureStateEnumConverter))]
    public enum EnclosureState
    {
        /// <summary>
        /// The added
        /// </summary>
        Added = 0,

        /// <summary>
        /// The ready
        /// </summary>
        Ready = 1,

        /// <summary>
        /// The locked
        /// </summary>
        Locked = 2,

        /// <summary>
        /// The unmanaged
        /// </summary>
        Unmanaged = 3,

        /// <summary>
        /// The unknown
        /// </summary>
        Unknown = -1,
    }
}