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
// Author           : panwei
// Created          : 12-26-2018
//
// Last Modified By : mike
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="StorageController.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class StorageController.
    /// </summary>
    public class StorageController
    {

        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.DeviceId}";

        /// <summary>
        /// ID-手动指定
        /// </summary>
        /// <value>The identifier.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// 指定存储控制器的描述信息。
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 指定存储控制器的固件版本。
        /// </summary>
        /// <value>The firmware version.</value>
        [JsonProperty("FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// 指定存储控制器生产厂商。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// 指定控制器的ID。
        /// </summary>
        /// <value>The member identifier.</value>
        [JsonProperty("MemberId")]
        public string MemberId { get; set; }

        /// <summary>
        /// 指定存储控制器型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 指定存储控制器的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 自定义属性。
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public JObject Oem { get; set; }

        /// <summary>
        /// 指定存储控制器的接口速率。
        /// </summary>
        /// <value>The speed GBPS.</value>
        [JsonProperty("SpeedGbps")]
        public double? SpeedGbps { get; set; }

        /// <summary>
        /// 指定存储控制器支持的协议类型。
        /// </summary>
        /// <value>The supported device protocols.</value>
        [JsonProperty("SupportedDeviceProtocols")]
        public List<string> SupportedDeviceProtocols { get; set; }

        /// <summary>
        /// 指定存储控制器的状态。
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; }

        #region Oem下的华为字段

        /// <summary>
        /// Gets the configuration version.
        /// </summary>
        /// <value>The configuration version.</value>
        public string ConfigurationVersion => Oem.GetString("Huawei.ConfigurationVersion");

        /// <summary>
        /// Gets the memory size mi b.
        /// </summary>
        /// <value>The memory size mi b.</value>
        public string MemorySizeMiB => Oem.GetString("Huawei.MemorySizeMiB");

        /// <summary>
        /// Gets the sas address.
        /// </summary>
        /// <value>The sas address.</value>
        public string SASAddress => Oem.GetString("Huawei.SASAddress");

        /// <summary>
        /// Gets the supported raid levels.
        /// </summary>
        /// <value>The supported raid levels.</value>
        public string SupportedRAIDLevels => Oem.GetString("Huawei.SupportedRAIDLevels");
        #endregion

        /// <summary>
        /// Node的使能状态。支持的状态包括：Enabled、Absent。
        /// </summary>
        /// <value>The health.</value>
        public string EnableState => Status.State;

        /// <summary>
        /// Gets the health.
        /// </summary>
        /// <value>The health.</value>
        public Health Health => Status.Health;

        
    }
}