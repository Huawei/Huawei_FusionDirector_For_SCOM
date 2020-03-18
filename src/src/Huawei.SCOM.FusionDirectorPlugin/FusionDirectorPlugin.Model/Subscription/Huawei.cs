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
// <copyright file="Huawei.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class Huawei.
    /// </summary>
    public partial class Huawei
    {
        /// <summary>
        /// 事件订阅请求者的用户名。
        /// </summary>
        /// <value>The name of the user.</value>
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        /// <summary>
        /// 事件订阅请求来源，字符串。
        /// </summary>
        /// <value>The resource.</value>
        [JsonProperty("Resource")]
        public string Resource { get; set; }

        /// <summary>
        /// 事件订阅请求者的密码。
        /// </summary>
        /// <value>The password.</value>
        [JsonProperty("Password")]
        public string Password { get; set; }

    }
}