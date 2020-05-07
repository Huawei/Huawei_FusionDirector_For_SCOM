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
// <copyright file="IPv4Address.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class IPv4Address.
    /// </summary>
    public class IPv4Address
    {
        /// <summary>
        /// Node上的BMC IPv4地址。
        /// </summary>
        /// <value>The address.</value>
        [JsonProperty("Address")]
        public string Address { get; set; }

        /// <summary>
        /// Node上的BMC IPv4地址获取方式。
        /// </summary>
        /// <value>The address origin.</value>
        [JsonProperty("AddressOrigin")]
        public string AddressOrigin { get; set; }

        /// <summary>
        /// Node上的BMC IPv4地址对应的网关。
        /// </summary>
        /// <value>The gate way.</value>
        [JsonProperty("GateWay")]
        public string GateWay { get; set; }

        /// <summary>
        /// Node上的BMC IPv4地址对应的子网掩码。
        /// </summary>
        /// <value>The subnet mask.</value>
        [JsonProperty("SubnetMask")]
        public string SubnetMask { get; set; }

        /// <summary>
        /// 指定iBMC网口的IPv6地址对应的前缀长度。
        /// </summary>
        /// <value>The length of the prefix.</value>
        [JsonProperty("PrefixLength")]
        public double? PrefixLength { get; set; }
    }
}