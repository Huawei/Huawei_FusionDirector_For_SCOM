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
// Author           : yayun
// Created          : 01-11-2019
//
// Last Modified By : yayun
// Last Modified On : 01-11-2019
// ***********************************************************************
// <copyright file="EventInfo.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 表明了一条告警或者事件的各种详细信息。
    /// </summary>

    public partial class EventInfo : EventSummary
    {
        /// <summary>
        /// 用于odata的资源标识符。
        /// </summary>
        /// <value>The odata context.</value>
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        /// <summary>
        /// 用于odata的资源唯一标识符。
        /// </summary>
        /// <value>The odata identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 资源类型。
        /// </summary>
        /// <value>The type of the odata.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        #region 详情中的字段

        /// <summary>
        /// 告警被清除的原因。
        /// </summary>
        /// <value>The clear case.</value>
        [JsonProperty("ClearCase")]
        public string ClearCase { get; set; }

        /// <summary>
        /// 告警被清除的操作用户。
        /// </summary>
        /// <value>The clear user.</value>
        [JsonProperty("ClearUser")]
        public string ClearUser { get; set; }

        /// <summary>
        /// 告警被确认的描述信息。
        /// </summary>
        /// <value>The confirm remarks.</value>
        [JsonProperty("ConfirmRemarks")]
        public string ConfirmRemarks { get; set; }

        /// <summary>
        /// 告警被确认的操作用户。
        /// </summary>
        /// <value>The confirm user.</value>
        [JsonProperty("ConfirmUser")]
        public string ConfirmUser { get; set; }

        /// <summary>
        /// 告警确认时间
        /// </summary>
        /// <value>The confirm time.</value>
        [JsonProperty("ConfirmTime")]
        public string ConfirmTime { get; set; }

        /// <summary>
        /// 告警发生的描述信息。
        /// </summary>
        /// <value>The description.</value>
        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// 告警发生的影响。
        /// </summary>
        /// <value>The effect.</value>
        [JsonProperty("Effect")]
        public string Effect { get; set; }

        /// <summary>
        /// 处理建议。
        /// </summary>
        /// <value>The handing suggesstion.</value>
        [JsonProperty("HandingSuggesstion")]
        public string HandingSuggesstion { get; set; }

        /// <summary>
        /// 告警发生可能原因。
        /// </summary>
        /// <value>The possible cause.</value>
        [JsonProperty("PossibleCause")]
        public string PossibleCause { get; set; }
        #endregion

    }
}