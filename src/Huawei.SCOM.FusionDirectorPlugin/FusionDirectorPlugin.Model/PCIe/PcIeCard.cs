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
// Last Modified On : 02-12-2019
// ***********************************************************************
// <copyright file="PcIeCard.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ************************************************************************
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class PCIeCard.
    /// </summary>
    public class PCIeCard
    {
        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.DeviceID}";

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        ///// <summary>
        ///// PCIe卡的ID号。
        ///// </summary>
        ///// <value>The identifier.</value>
        //[JsonProperty("Id")]
        //public string Id { get; set; }

        /// <summary>
        /// Node资源节点的Odate描述信息
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// Node资源列表节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Node资源列表类型。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// PCIe卡的设备ID。
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// PCIe卡的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// PCIe卡描述。
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// PCIe卡的厂商。
        /// </summary>
        /// <value>The manufacturer.</value>
        [JsonProperty("Manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// PCIe卡的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// PCIe卡的型号类别。
        /// </summary>
        /// <value>The type of the model.</value>
        [JsonProperty("ModelType")]
        public string ModelType { get; set; }

        /// <summary>
        /// PCIe卡序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 固件版本。
        /// </summary>
        /// <value>The firmware version.</value>
        [JsonProperty("FirmwareVersion")]
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Node资源上PCIe信息。
        /// </summary>
        /// <value>The oem.</value>
        [JsonProperty("Oem")]
        public JObject Oem { get; set; }
        #region Oem
        //DeviceLocator、Position、Power FunctionType

        /// <summary>
        /// Gets the device locator.
        /// </summary>
        /// <value>The device locator.</value>
        public string DeviceLocator => Oem.GetString("Huawei.DeviceLocator");

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>The position.</value>
        public string Position => Oem.GetString("Huawei.Position");

        /// <summary>
        /// Gets the type of the function.
        /// </summary>
        /// <value>The type of the function.</value>
        public string FunctionType => Oem.GetString("Huawei.FunctionType");

        #endregion
        /// <summary>
        /// PCIe卡的功能描述。
        /// </summary>
        /// <value>The pc ie function.</value>
        [JsonProperty("PCIeFunction")]
        public ODataId PCIeFunction { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; }

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