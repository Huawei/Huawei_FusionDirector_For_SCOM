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
// Assembly         : FusionDirectorPlugin.Api
// Author           : panwei
// Created          : 01-02-2019
//
// Last Modified By : panwei
// Last Modified On : 01-02-2019
// ***********************************************************************
// <copyright file="MySecurityProtocolType.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace FusionDirectorPlugin.WebServer
{
    /// <summary>
    /// 定义需要使用的系统枚举。
    /// </summary>
    [Flags]
    public enum MySecurityProtocolType
    {
        /// <summary>
        /// The SSL3
        /// </summary>
        Ssl3 = 48,

        /// <summary>
        /// The TLS
        /// </summary>
        Tls = 192,

        /// <summary>
        /// The TLS11
        /// </summary>
        Tls11 = 768,

        /// <summary>
        /// The TLS12
        /// </summary>
        Tls12 = 3072
    }
}
