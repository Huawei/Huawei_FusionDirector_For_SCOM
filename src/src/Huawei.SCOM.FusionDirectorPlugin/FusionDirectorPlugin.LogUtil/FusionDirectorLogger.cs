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
// Assembly         : FusionDirectorPlugin.LogUtil
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="FusionDirectorLogger.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using NLog;

namespace FusionDirectorPlugin.LogUtil
{
    /// <summary>
    /// Class FdLogger.
    /// </summary>
    public class FusionDirectorLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FusionDirectorLogger"/> class.
        /// </summary>
        /// <param name="fdip">The fd ip.</param>
        public FusionDirectorLogger(string fdip) 
        {
            this.FdIp = fdip;
        }

        /// <summary>
        /// Gets or sets the fd ip.
        /// </summary>
        /// <value>The fd ip.</value>
        public string FdIp { get; set; }

        /// <summary>
        /// Gets the polling.
        /// </summary>
        /// <value>The polling.</value>
        public Logger Polling => LogManager.GetLogger($"{this.FdIp}.Polling");

        /// <summary>
        /// Gets the subscribe.
        /// </summary>
        /// <value>The subscribe.</value>
        public Logger Subscribe => LogManager.GetLogger($"{this.FdIp}.Subscribe");

        /// <summary>
        /// Gets the notify process.
        /// </summary>
        /// <value>The notify process.</value>
        public Logger NotifyProcess => LogManager.GetLogger($"{this.FdIp}.NotifyProcess");

        /// <summary>
        /// Gets the API.
        /// </summary>
        /// <value>The API.</value>
        public Logger Api => LogManager.GetLogger($"{this.FdIp}.Api");

        /// <summary>
        /// Gets the SDK.
        /// </summary>
        /// <value>The SDK.</value>
        public Logger Sdk => LogManager.GetLogger($"{this.FdIp}.Sdk");
    }
}
