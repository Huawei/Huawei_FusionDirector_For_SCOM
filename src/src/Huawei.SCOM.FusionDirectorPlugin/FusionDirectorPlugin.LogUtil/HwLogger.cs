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
// Created          : 12-25-2018
//
// Last Modified By : mike
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="HwLogger.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using NLog;

namespace FusionDirectorPlugin.LogUtil
{
    /// <summary>
    /// Class HWLogger.
    /// </summary>
    public class HWLogger
    {
        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>The default.</value>
        public static Logger Default => LogManager.GetLogger($"Default");

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>The service.</value>
        public static Logger Service => LogManager.GetLogger($"Service");

        /// <summary>
        /// Gets the install.
        /// </summary>
        /// <value>The install.</value>
        public static Logger Install => LogManager.GetLogger($"Install");

        /// <summary>
        /// Gets the UI.
        /// </summary>
        /// <value>The UI.</value>
        public static Logger UI => LogManager.GetLogger($"UI");

        /// <summary>
        /// Gets the notify recv.
        /// </summary>
        /// <value>The notify recv.</value>
        public static Logger NotifyRecv => LogManager.GetLogger($"NotifyRecv");

        /// <summary>
        /// Gets the e sight SDK logger.
        /// </summary>
        /// <param name="fdip">The e sight ip.</param>
        /// <returns>Logger.</returns>
        public static Logger GetFdSdkLogger(string fdip)
        {
            return LogManager.GetLogger($"{fdip}.Sdk");
        }

        /// <summary>
        /// Gets the e sight notify logger.
        /// </summary>
        /// <param name="fdip">The e sight ip.</param>
        /// <returns>Logger.</returns>
        public static Logger GetFdNotifyLogger(string fdip)
        {
            return LogManager.GetLogger($"{fdip}.NotifyProcess");
        }
    }
}
