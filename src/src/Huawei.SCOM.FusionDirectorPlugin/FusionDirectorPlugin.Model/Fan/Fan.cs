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
// <copyright file="Fan.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class Thermal.
    /// </summary>
    public class Fan
    {
        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.DeviceId}";

        /// <summary>
        /// ID-手动赋值
        /// </summary>
        /// <value>The physical UUID.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// 指定风扇传感器的ID。
        /// </summary>
        /// <value>The member identifier.</value>
        [JsonProperty("MemberId")]
        public string MemberId { get; set; }

        /// <summary>
        /// 指定风扇传感器的名称。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 指定风扇传感器的当前读数。
        /// </summary>
        /// <value>The reading.</value>
        [JsonProperty("Reading")]
        public double? Reading { get; set; }

        /// <summary>
        /// 风扇的转速单位，包括：RPM ；Percent 。
        /// </summary>
        /// <value>The reading units.</value>
        [JsonProperty("ReadingUnits")]
        public string ReadingUnits { get; set; }

        /// <summary>
        /// 指定风扇的部件号
        /// </summary>
        /// <value>The part number.</value>
        [JsonProperty("PartNumber")]
        public string PartNumber { get; set; }

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