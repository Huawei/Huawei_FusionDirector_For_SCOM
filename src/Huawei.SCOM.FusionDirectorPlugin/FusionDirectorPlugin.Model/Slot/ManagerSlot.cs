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
// Created          : 12-27-2018
//
// Last Modified By : panwei
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="ManagerSlot.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 管理板槽位信息。
    /// </summary>
    public partial class ManagerSlot
    {

        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.UUID}";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string UUID => this.PhysicalUUID;

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// 物理UUID。
        /// </summary>
        /// <value>The physical UUID.</value>
        [JsonProperty("PhysicalUUID")]
        public string PhysicalUUID { get; set; }

        /// <summary>
        /// 槽位号。
        /// </summary>
        /// <value>The index.</value>
        [JsonProperty("Index")]
        public int Index { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty("State")]
        public SlotState SlotState { get; set; }

        /// <summary>
        /// 产品名。
        /// </summary>
        /// <value>The name of the product.</value>
        [JsonProperty("ProductName")]
        public string ProductName { get; set; }

        /// <summary>
        /// 序列号。
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
        /// CPLD版本。
        /// </summary>
        /// <value>The CPLD version.</value>
        [JsonProperty("CPLDVersion")]
        public string CPLDVersion { get; set; }

        /// <summary>
        /// Gets or sets the static i PV4 address.
        /// </summary>
        /// <value>The static i PV4 address.</value>
        [JsonProperty("StaticIPv4Address")]
        public IPv4Address StaticIPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the float i PV4 address.
        /// </summary>
        /// <value>The float i PV4 address.</value>
        [JsonProperty("FloatIPv4Address")]
        public IPv4Address FloatIPv4Address { get; set; }

        /// <summary>
        /// Gets or sets the appliance i PV4 address.
        /// </summary>
        /// <value>The appliance i PV4 address.</value>
        [JsonProperty("ApplianceIPv4Address")]
        public IPv4Address ApplianceIPv4Address { get; set; }

        /// <summary>
        /// Appliance的BMC固件版本。
        /// </summary>
        /// <value>The appliance firmware version.</value>
        [JsonProperty("ApplianceFirmwareVersion")]
        public string ApplianceFirmwareVersion { get; set; }

        /// <summary>
        /// Appliance的BIOS固件版本。
        /// </summary>
        /// <value>The appliance bios version.</value>
        [JsonProperty("ApplianceBIOSVersion")]
        public string ApplianceBIOSVersion { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]
        public Health Health { get; set; }= Health.Warning;
    }
}