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
// Assembly         : CommonUtil
// Author           : panwei
// Created          : 12-25-2018
//
// Last Modified By : mike
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="SystemUtil.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Management;
using FusionDirectorPlugin.LogUtil;
using System.Security.Cryptography;

namespace CommonUtil
{
    /// <summary>
    /// 系统相关工具类
    /// 如： 系统环境，系统IP等。
    /// </summary>
    public class SystemUtil
    {
        /// <summary>
        /// 获得本地计算机IP
        /// </summary>
        /// <returns>本地计算机IP</returns>
        public static string GetLocalhostIP()
        {
            try
            {
                string stringIp = string.Empty;
                ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
                foreach (ManagementObject managementObject in managementObjectCollection)
                {
                    if ((bool)managementObject["IPEnabled"])
                    {
                        string[] ipAddresses = managementObject["IPAddress"] as string[];
                        if (ipAddresses != null && ipAddresses.Length > 0)
                        {
                            stringIp = ipAddresses[0];
                        }
                    }
                }
                return stringIp;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static int GenRadomInt()
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[sizeof(int)];
                crypto.GetBytes(randomBytes);
                return Math.Abs(BitConverter.ToInt16(randomBytes, 0)) % 65536;
            }
        }
    }
}
