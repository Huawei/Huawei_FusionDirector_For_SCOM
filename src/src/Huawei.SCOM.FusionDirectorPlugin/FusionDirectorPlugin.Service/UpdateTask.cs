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
// Assembly         : FusionDirectorPlugin.Service
// Author           : yayun
// Created          : 01-25-2019
//
// Last Modified By : yayun
// Last Modified On : 01-25-2019
// ***********************************************************************
// <copyright file="UpdateTask.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Service
{
    /// <summary>
    /// Class UpdateEnclosureTask.
    /// </summary>
    public class UpdateTask<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FusionDirectorPlugin.Service.UpdateTask" /> class.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <param name="sn">The sn.</param>
        public UpdateTask(string deviceId, int sn)
        {
            DeviceId = deviceId;
            Sn = sn;
            FirstRefreshTime = DateTime.Now.AddSeconds(10);
            LastRefreshTime = DateTime.Now.AddSeconds(60);
        }

        /// <summary>
        /// Dn
        /// </summary>
        /// <value>The dn.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// 告警Sn
        /// </summary>
        /// <value>The alarm sn.</value>
        public int Sn { get; set; }

        /// <summary>
        /// 首次刷新时间
        /// </summary>
        /// <value>The last refresh time.</value>
        public DateTime FirstRefreshTime { get; set; }

        /// <summary>
        /// 最后一次刷新时间
        /// </summary>
        /// <value>The last refresh time.</value>
        public DateTime LastRefreshTime { get; set; }

        /// <summary>
        /// Gets or sets the device first.
        /// </summary>
        /// <value>The device first.</value>
        public T DeviceFirst { get; set; }

        /// <summary>
        /// Checks the is change.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool CheckIsChange(T lastDevice)
        {
            return JsonConvert.SerializeObject(DeviceFirst) != JsonConvert.SerializeObject(lastDevice);
        }

    }
}