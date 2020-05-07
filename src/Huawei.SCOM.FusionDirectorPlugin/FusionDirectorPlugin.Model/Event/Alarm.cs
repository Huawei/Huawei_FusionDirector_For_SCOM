//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model.Event
{
    public class AlarmData : ICloneable
    {
        public AlarmData()
        {
        }

        public AlarmData(EventInfo eventInfo)
        {
            this.AlarmId = eventInfo.EventID;
            this.AlarmName = eventInfo.EventName;
            this.ResourceId = eventInfo.EventSource + "_" + eventInfo.EventSubject;
            this.ResourceIdName = eventInfo.EventSource + "_" + eventInfo.EventSubject;
            this.Moc = string.Empty;
            this.Sn = eventInfo.SerialNumber;
            this.Category = GetCategory(eventInfo.Severity);
            this.Severity = GetSeverity(eventInfo.Severity);
            this.OccurTime = eventInfo.FirstOccurTime;
            this.ClearTime = eventInfo.ClearTime;
            this.ClearType = eventInfo.ClearType;
            this.IsClear = string.Empty;
            this.Status = eventInfo.Status;
            this.Additional = eventInfo.Description;
            this.DeviceId = eventInfo.DeviceID;
            this.EventCategory = eventInfo.EventCategory;
            this.EventSubject = eventInfo.EventSubject;
            this.EventDescriptionArgs = eventInfo.EventDescriptionArgs;
            this.PossibleCause = eventInfo.PossibleCause;
            this.Suggestion = eventInfo.HandingSuggesstion;
            this.Effect = eventInfo.Effect;

        }

        public AlarmData(EventSummary eventInfo)
        {
            this.AlarmId = eventInfo.EventID;
            this.AlarmName = eventInfo.EventName;
            this.ResourceId = eventInfo.EventSource + "_" + eventInfo.EventSubject;
            this.ResourceIdName = eventInfo.EventSource + "_" + eventInfo.EventSubject;
            this.Moc = string.Empty;
            this.Sn = eventInfo.SerialNumber;
            this.Category = GetCategory(eventInfo.Severity);
            this.Severity = GetSeverity(eventInfo.Severity);
            this.OccurTime = eventInfo.FirstOccurTime;
            this.ClearTime = eventInfo.ClearTime;
            this.ClearType = eventInfo.ClearType;
            this.IsClear = string.Empty;
            this.Status = eventInfo.Status;
            this.Additional = eventInfo.EventDescription;
            this.DeviceId = eventInfo.DeviceID;
            this.EventCategory = eventInfo.EventCategory;
            this.EventSubject = eventInfo.EventSubject;
            this.EventDescriptionArgs = eventInfo.EventDescriptionArgs;
            this.PossibleCause = string.Empty;
            this.Suggestion = string.Empty;
            this.Effect = string.Empty;
        }

        /// <summary>
        /// 获取告警级别 1：Critical 2：Major 3：Warning 4：OK
        /// </summary>
        /// <param name="eventInfoSeverity">The event information severity.</param>
        /// <returns>System.String.</returns>
        private string GetSeverity(string eventInfoSeverity)
        {
            switch (eventInfoSeverity.ToUpper())
            {
                case "1":
                case "CRITICAL":
                    return "1";
                case "2":
                case "MAJOR":
                    return "2";
                case "3":
                case "WARNING":
                    return "3";
                case "4":
                case "OK":
                    return "4";
                default:
                    return "4";
            }
        }

        /// <summary>
        /// 获取告警类型 1：告警 2：恢复 3：事件
        /// </summary>
        /// <param name="eventInfoSeverity">The event information severity.</param>
        /// <returns>System.String.</returns>
        private string GetCategory(string eventInfoSeverity)
        {
            switch (eventInfoSeverity.ToUpper())
            {
                case "1":
                case "CRITICAL":
                case "2":
                case "MAJOR":
                case "3":
                case "WARNING":
                    return "1";
                case "4":
                case "OK":
                    return "3";
                default:
                    return "3";
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        /// <summary>
        /// 告警号
        /// </summary>
        [JsonProperty("alarmid")]
        public string AlarmId { get; set; }

        /// <summary>
        /// 告警名称
        /// </summary>
        [JsonProperty("alarmname")]
        public string AlarmName { get; set; }

        /// <summary>
        /// 设备IP_事件主体
        /// </summary>
        [JsonProperty("resourceid")]
        public string ResourceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [JsonProperty("resourceidname")]
        public string ResourceIdName { get; set; }

        /// <summary>
        /// 告警MOC名称
        /// </summary>
        [JsonProperty("moc")]
        public string Moc { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        [JsonProperty("sn")]
        public int Sn { get; set; }

        /// <summary>
        /// 告警类型 1：告警 2：恢复 3：事件
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// 级别 1：Critical 2：Major 3：Warning 4:OK
        /// </summary>
        [JsonProperty("severity")]
        public string Severity { get; set; }

        /// <summary>
        /// 告警发生时间
        /// </summary>
        [JsonProperty("occurtime")]
        public string OccurTime { get; set; }

        /// <summary>
        /// 告警恢复时间
        /// </summary>
        [JsonProperty("cleartime")]
        public string ClearTime { get; set; }

        /// <summary>
        /// 告警恢复类型0：正常清除  2：手动清除
        /// </summary>
        [JsonProperty("cleartype")]
        public string ClearType { get; set; }

        /// <summary>
        /// 是否可以自动清除0：是 1：否
        /// </summary>
        [JsonProperty("isclear")]
        public string IsClear { get; set; }

        /// <summary>
        /// 附加信息
        /// </summary>
        [JsonProperty("additional")]
        public string Additional { get; set; }

        /// <summary>
        /// 告警描述
        /// </summary>
        //[JsonProperty("cause")]
        //public string Cause { get; set; }

        /// <summary>
        /// 设备deviceid
        /// </summary>
        [JsonProperty("deviceid")]
        public string DeviceId { get; set; }

        /// <summary>
        /// 事件类型 BMC Enclosure FusionDirector Switch
        /// </summary>
        [JsonProperty("eventcategory")]
        public string EventCategory { get; set; }

        /// <summary>
        /// 事件主体
        /// </summary>
        [JsonProperty("eventsubject")]
        public string EventSubject { get; set; }

        /// <summary>
        /// 告警描述参数列表
        /// </summary>
        [JsonProperty("eventdescriptionargs")]
        [XmlIgnore]
        public object EventDescriptionArgs { get; set; }

        /// <summary>
        /// 可能原因
        /// </summary>
        [JsonProperty("cause")]
        public string PossibleCause { get; set; }

        /// <summary>
        /// 处理建议
        /// </summary>
        [JsonProperty("suggestion")]
        public string Suggestion { get; set; }

        /// <summary>
        /// 影响
        /// </summary>
        [JsonProperty("effect")]
        public string Effect { get; set; }

        /// <summary>
        /// 告警状态。Uncleared表示未清除，Cleared代表已清除
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }
    }
}