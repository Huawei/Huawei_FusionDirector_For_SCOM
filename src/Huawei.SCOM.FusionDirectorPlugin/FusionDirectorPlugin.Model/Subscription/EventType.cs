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
// Author           : yayun
// Created          : 01-11-2019
//
// Last Modified By : yayun
// Last Modified On : 01-11-2019
// ***********************************************************************
// <copyright file="EventType.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Enum EventType
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// The status change
        /// </summary>
        StatusChange = 0,

        /// <summary>
        /// The resource updated
        /// </summary>
        ResourceUpdated = 1,

        /// <summary>
        /// The resource added
        /// </summary>
        ResourceAdded = 2,

        /// <summary>
        /// The resource removed
        /// </summary>
        ResourceRemoved = 3,

        /// <summary>
        /// The alert
        /// </summary>
        Alert = 4,

    }

}
