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
// Assembly         : FusionDirectorPlugin.Dal
// Author           : yayun
// Created          : 01-18-2019
//
// Last Modified By : yayun
// Last Modified On : 01-18-2019
// ***********************************************************************
// <copyright file="SubscribeStatus.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace FusionDirectorPlugin.Dal.Model
{
    /// <summary>
    ///  SubscribeStatus
    /// </summary>
    public class SubscribeStatus
    {
        /// <summary>
        /// 未订阅
        /// </summary>
        public const string NotSubscribed = "NotSubscribed";

        /// <summary>
        /// 订阅失败
        /// </summary>
        public const string Error = "Error";

        /// <summary>
        /// 订阅成功
        /// </summary>
        public const string Success = "Success";
    }
}