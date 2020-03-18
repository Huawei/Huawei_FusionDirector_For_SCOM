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
// Assembly         : FusionDirectorPlugin.Core
// Author           : yayun
// Created          : 02-25-2019
//
// Last Modified By : yayun
// Last Modified On : 02-25-2019
// ***********************************************************************
// <copyright file="EnumAlarmType.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FusionDirectorPlugin.Core.Models
{
    public enum EnumAlarmType
    {
        /// <summary>
        /// The web server unavailable
        /// </summary>
        WebServerUnavailable = 1,

        /// <summary>
        /// The enclosure mp missing
        /// </summary>
        EnclosureMpMissing = 2,

        /// <summary>
        /// The server mp missing
        /// </summary>
        ServerMpMissing = 3,

        /// <summary>
        /// fd connect error
        /// </summary>
        FdConnectError = 4,
    }
}