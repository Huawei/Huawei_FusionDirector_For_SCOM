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
// <copyright file="EventSummary.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 表明了一条告警或者事件的各种详细信息。列表返回
    /// </summary>

    public partial class EventSummary
    {
        /// <summary>
        /// 告警或者事件的屏蔽状态。
        /// </summary>
        /// <value><c>true</c> if [blocking status]; otherwise, <c>false</c>.</value>
        [JsonProperty("BlockingStatus")]
        public bool BlockingStatus { get; set; }

        /// <summary>
        /// 告警被清除的时间。
        /// </summary>
        /// <value>The clear time.</value>
        [JsonProperty("ClearTime")]
        public string ClearTime { get; set; }

        /// <summary>
        /// 告警被清除的类型，比如：手动清除、正常清除、或者设备删除被清除。
        /// </summary>
        /// <value>The type of the clear.</value>
        [JsonProperty("ClearType")]
        public string ClearType { get; set; }

        /// <summary>
        /// 告警确认状态。Unconfirmed标示未确认，Confirmed标识已确认
        /// </summary>
        /// <value>The confirm status.</value>
        [JsonProperty("ConfirmStatus")]
        public string ConfirmStatus { get; set; }

        /// <summary>
        /// 告警发生的设备Id
        /// </summary>
        /// <value>The device identifier.</value>
        [JsonProperty("DeviceID")]
        public string DeviceID { get; set; }

        /// <summary>
        /// 告警发生的设备序列号。
        /// </summary>
        /// <value>The device sn.</value>
        [JsonProperty("DeviceSn")]
        public string DeviceSn { get; set; }

        /// <summary>
        /// 告警发生的设备类型。
        /// </summary>
        /// <value>The type of the device.</value>
        [JsonProperty("DeviceType")]
        public string DeviceType { get; set; }

        /// <summary>
        /// 告警号类型。
        /// </summary>
        /// <value>The event category.</value>
        [JsonProperty("EventCategory")]
        public string EventCategory { get; set; }

        /// <summary>
        /// 告警产生描述。
        /// </summary>
        /// <value>The event description.</value>
        [JsonProperty("EventDescription")]
        public string EventDescription { get; set; }

        /// <summary>
        /// 告警描述参数列表。
        /// </summary>
        /// <value>The event description arguments.</value>
        [JsonProperty("EventDescriptionArgs")]
        public object EventDescriptionArgs { get; set; }

        /// <summary>
        /// 告警号ID。
        /// </summary>
        /// <value>The event identifier.</value>
        [JsonProperty("EventID")]
        public string EventID { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        [JsonProperty("EventName")]
        public string EventName { get; set; }

        /// <summary>
        /// 告警发生的设备源。
        /// </summary>
        /// <value>The event source.</value>
        [JsonProperty("EventSource")]
        public string EventSource { get; set; }

        /// <summary>
        /// 告警发生的事件主体。
        /// </summary>
        /// <value>The event subject.</value>
        [JsonProperty("EventSubject")]
        public string EventSubject { get; set; }

        /// <summary>
        /// 事件或者告警，alert代表告警，event代表事件。
        /// </summary>
        /// <value>The type of the event.</value>
        [JsonProperty("EventType")]
        public string EventType { get; set; }

        /// <summary>
        /// 告警首次发生时间。
        /// </summary>
        /// <value>The first occur time.</value>
        [JsonProperty("FirstOccurTime")]
        public string FirstOccurTime { get; set; }

        /// <summary>
        /// 告警最后一次发生时间。
        /// </summary>
        /// <value>The last occur time.</value>
        [JsonProperty("LastOccurTime")]
        public string LastOccurTime { get; set; }

        /// <summary>
        /// 告警发生次数。
        /// </summary>
        /// <value>The occur counts.</value>
        [JsonProperty("OccurCounts")]
        public string OccurCounts { get; set; }

        /// <summary>
        /// 告警发生的部件。
        /// </summary>
        /// <value>The parts.</value>
        [JsonProperty("Parts")]
        public string Parts { get; set; }

        /// <summary>
        /// 告警读标识，Read表示已读，Unread表示未读。
        /// </summary>
        /// <value>The read flag.</value>
        [JsonProperty("ReadFlag")]
        public string ReadFlag { get; set; }

        /// <summary>
        /// 告警流水号Id。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public int SerialNumber { get; set; }

        /// <summary>
        /// 告警级别。OK表示正常， Major、Warning表示警告，Critical代表紧急
        /// </summary>
        /// <value>The severity.</value>
        [JsonProperty("Severity")]
        public string Severity { get; set; }

        /// <summary>
        /// 告警状态。Uncleared表示未清除，Cleared代表已清除
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        [JsonProperty("Link")]
        public ODataId Link { get; set; } = new ODataId();

    }
}