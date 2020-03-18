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
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="ConfigHelper.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using FusionDirectorPlugin.PluginConfigs.Helpers;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ConfigHelper.
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// The env variable.
        /// </summary>
        private static string path = $"{Environment.GetEnvironmentVariable("FDSCOMPLUGIN")}/Configuration/PluginConfig.xml";

        /// <summary>
        /// Gets the plugin configuration.
        /// </summary>
        /// <returns>PluginConfig.</returns>
        /// <exception cref="Exception">
        /// can not find config file:
        /// or
        /// config is null
        /// </exception>
        public static PluginConfig GetPluginConfig()
        {
            if (!File.Exists(path))
            {
                throw new Exception("can not find config file:");
            }
            var config = XmlHelper.Load(typeof(PluginConfig), path) as PluginConfig;
            if (config == null)
            {
                throw new Exception("config is null");
            }
            return config;
        }

        /// <summary>
        /// Saves the plugin configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void SavePluginConfig(PluginConfig config)
        {
            XmlHelper.Save(config, path);
        }

        /// <summary>
        /// Saves the plugin configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="installPath">The install path.</param>
        public static void SavePluginConfig(PluginConfig config, string installPath)
        {
            XmlHelper.Save(config, installPath);
        }
    }
}
