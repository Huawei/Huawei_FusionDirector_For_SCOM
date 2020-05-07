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
// <copyright file="ServerSummary.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerSummary.
    /// </summary>
    public class ServerSummary
    {
        public ServerSummary()
        {
            this.ODataId = string.Empty;
            this.DeviceID = string.Empty;
            this.UUID = string.Empty;
            this.Name = string.Empty;
            this.Model = string.Empty;
            this.SerialNumber = string.Empty;
            this.Alias = string.Empty;
            this.PowerState = string.Empty;
            this.State = string.Empty;
            this.ProcessorSummary = new ProcessorSummary();
            this.MemorySummary = new MemorySummary();
            this.StorageSummary = new StorageSummary();
        }

        /// <summary>
        /// Node详细信息访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id => ODataId.Split('/').Last();

        /// <summary>
        /// Node上的设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// Node的UUID。
        /// </summary>
        /// <value>The UUID.</value>
        [JsonProperty("UUID")]
        public string UUID { get; set; }

        /// <summary>
        /// Node上部署的Profile名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Node的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// 指定Node资源所属机框的序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Node别名
        /// </summary>
        /// <value>The alias.</value>
        [JsonProperty("Alias")]
        public string Alias { get; set; }

        /// <summary>
        /// Node标签
        /// </summary>
        /// <value>The tag.</value>
        [JsonProperty("Tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Node所属用户组。
        /// string / List string 
        /// </summary>
        /// <value>The group.</value>
        [JsonProperty("Group")]
        public object Group { get; set; }

        /// <summary>
        /// Gets or sets the i PV4 address.
        /// </summary>
        /// <value>The i PV4 address.</value>
        [JsonProperty("IPv4Address")]
        public IPv4Address IPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; } = new Status();

        /// <summary>
        /// Node的CPU电源状态。
        /// </summary>
        /// <value>The state of the power.</value>
        [JsonProperty("PowerState")]
        public string PowerState { get; set; }

        /// <summary>
        /// 服务器管理状态。
        /// </summary>
        /// <value>The state of the server.</value>
        [JsonProperty("State")]

        public string State { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>The profile.</value>
        [JsonProperty("Profile")]
        public object Profile { get; set; }

        /// <summary>
        /// Gets or sets the processor summary.
        /// </summary>
        /// <value>The processor summary.</value>
        [JsonProperty("ProcessorSummary")]
        public ProcessorSummary ProcessorSummary { get; set; }

        /// <summary>
        /// Gets or sets the memory summary.
        /// </summary>
        /// <value>The memory summary.</value>
        [JsonProperty("MemorySummary")]
        public MemorySummary MemorySummary { get; set; }

        /// <summary>
        /// Gets or sets the storage summary.
        /// </summary>
        /// <value>The storage summary.</value>
        [JsonProperty("StorageSummary")]
        public StorageSummary StorageSummary { get; set; }
    }
}