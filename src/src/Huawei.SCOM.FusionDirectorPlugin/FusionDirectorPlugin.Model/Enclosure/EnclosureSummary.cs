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
// Assembly         : WindowsFormsApp1
// Author           : mike
// Created          : 12-27-2018
//
// Last Modified By : mike
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="EnclosureSummary.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Linq;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 集合资源中的单个对象。
    /// </summary>
    public partial class EnclosureSummary
    {
        public EnclosureSummary()
        {
            this.ODataId = string.Empty;
            this.Name = string.Empty;
            this.State = EnclosureState.Unknown;
            this.EnclosureProfileURL = string.Empty;
            this.Type = EnclosureType.Unknown;
            this.StateReason = string.Empty;
            this.Health = Health.Warning;
        }

        /// <summary>
        /// 用于odata的资源唯一标识符。
        /// </summary>
        /// <value>The odata identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// Gets the UUID.
        /// </summary>
        /// <value>The UUID.</value>
        public string UUID => ODataId.Split('/').Last();

        /// <summary>
        /// 资源的名字。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// 机框的类型
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("Type")]
        public EnclosureType Type { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty("State")]

        public EnclosureState State { get; set; }

        /// <summary>
        /// Gets or sets the state reason.
        /// </summary>
        /// <value>The state reason.</value>
        [JsonProperty("StateReason")]
        public string StateReason { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]

        public Health Health { get; set; } = Health.Warning;

        /// <summary>
        /// Gets or sets the state of the enclosure profile usage.
        /// </summary>
        /// <value>The state of the enclosure profile usage.</value>
        [JsonProperty("EnclosureProfileUsageState")]

        public EnclosureProfileUsageState EnclosureProfileUsageState { get; set; }

        /// <summary>
        /// 机框配置文件的链接。
        /// </summary>
        /// <value>The enclosure profile URL.</value>
        [JsonProperty("EnclosureProfileURL")]
        public string EnclosureProfileURL { get; set; }
    }
}