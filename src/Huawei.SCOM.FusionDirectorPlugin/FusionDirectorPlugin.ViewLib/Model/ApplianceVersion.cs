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
// Assembly         : FusionDirectorPlugin.ViewLib
// Author           : mike
// Created          : 05-05-2019
//
// Last Modified By : mike
// Last Modified On : 05-05-2019
// ***********************************************************************
// <copyright file="ApplianceVersion.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FusionDirectorPlugin.ViewLib
{
    /// <summary>
    /// Class ApplianceVersion.
    /// </summary>
    public class ApplianceVersion
    {
        /// <summary>
        /// CurrentVersion
        /// </summary>
        /// <value>The current version.</value>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// InActiveVersion
        /// </summary>
        /// <value>The in active version.</value>
        public string InActiveVersion { get; set; }

        /// <summary>
        /// MinimunVersion
        /// </summary>
        /// <value>The minimun version.</value>
        public string MinimunVersion { get; set; }

        /// <summary>
        /// UpgradeTime
        /// </summary>
        /// <value>The upgrade time.</value>
        public string UpgradeTime { get; set; }

        /// <summary>
        /// ActivatedTime
        /// </summary>
        /// <value>The activated time.</value>
        public string ActivatedTime { get; set; }
    }
}
