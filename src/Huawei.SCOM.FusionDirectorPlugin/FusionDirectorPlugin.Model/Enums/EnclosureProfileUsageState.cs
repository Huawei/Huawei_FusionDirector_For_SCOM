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
// <copyright file="EnclosureProfileUsageState.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 机框配置文件的使用状态。
    /// Empty 没有关联机框配置文件。
    /// Consistent 机框上的实际配置与机框配置文件一致。
    /// Inconsistent 机框上的实际配置与机框配置文件不一致。
    /// </summary>
    public enum EnclosureProfileUsageState
    {
        /// <summary>
        /// The inconsistent
        /// </summary>
        Unsupported = -1,

        /// <summary>
        /// The empty
        /// </summary>
        Empty = 0,

        /// <summary>
        /// The consistent
        /// </summary>
        Consistent = 1,

        /// <summary>
        /// The inconsistent
        /// </summary>
        Inconsistent = 2,

    }
}