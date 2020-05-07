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
// Assembly         : Huawei.SCOM.ESightPlugin.Core
// Author           : yayun
// Created          : 12-19-2017
//
// Last Modified By : yayun
// Last Modified On : 12-19-2017
// ***********************************************************************
// <copyright file="EventData.cs" company="广州摩赛网络技术有限公司">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using System.Linq;
using CommonUtil;
using FusionDirectorPlugin.Model.Event;
using Microsoft.EnterpriseManagement.Monitoring;

namespace FusionDirectorPlugin.Core.Model
{
    /// <summary>
    /// Class EventModel.
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventData" /> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fusionDirectorIp">The fusion director ip.</param>
        public EventData(AlarmData data, string fusionDirectorIp)
        {
            this.FusionDirectorIp = fusionDirectorIp;
            this.AlarmData = data;
            this.UnionId = $"{this.FusionDirectorIp}-{this.AlarmData.DeviceId}";
        }

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// Gets or sets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId { get; set; }

        /// <summary>
        /// 1-告警 2-恢复 3-事件
        /// </summary>
        /// <value>The type of the opt.</value>
        public string OptType => AlarmData.Category;

        /// <summary>
        /// AlarmData
        /// </summary>
        /// <value>The alarm data.</value>
        public AlarmData AlarmData { get; }

        /// <summary>
        /// Gets or sets the alarm sn.
        /// </summary>
        /// <value>The alarm sn.</value>
        public int AlarmSn => this.AlarmData.Sn;

        /// <summary>
        /// 0-information, 1-warning, 2-error
        /// </summary>
        /// <value>The severity.</value>
        public int Severity
        {
            get
            {
                switch (this.LevelId)
                {
                    case EventLogEntryType.Error:
                        return 2;
                    case EventLogEntryType.Warning:
                        return 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// 1-Error, 2-Warning, 4-Information, 8-Success Audit, 16-Failure Audit.
        /// </summary>
        /// <value>The level identifier.</value>
        public EventLogEntryType LevelId
        {
            get
            {
                switch (AlarmData.Severity)
                {
                    case "1":
                        return EventLogEntryType.Error;
                    case "2":
                    case "3":
                        return EventLogEntryType.Warning;
                    case "4":
                        return EventLogEntryType.Information;
                    default:
                        return EventLogEntryType.Information;
                }
            }
        }

        /// <summary>
        /// 0-low, 1-medium, 2-high
        /// </summary>
        /// <value>The priority.</value>
        public int Priority
        {
            get
            {
                switch (this.LevelId)
                {
                    case EventLogEntryType.Error:
                        return 2;
                    case EventLogEntryType.Warning:
                        return 1;
                }
                return 0;
            }
        }

        /// <summary>
        /// 事件编号的尾数
        /// </summary>
        /// <value>The mantissa number.</value>
        public string MantissaNumber
        {
            get
            {
                return this.AlarmSn.ToString().Substring(this.AlarmSn.ToString().Length - 1, 1);
            }
        }

        public string EventId
        {
            get { return $"{this.Severity}{this.MantissaNumber}"; }
        }

        /// <summary>
        /// To the custom monitoring event.
        /// </summary>
        /// <returns>CustomMonitoringEvent.</returns>
        public CustomMonitoringEvent ToCustomMonitoringEvent()
        {
            var customEvent = new CustomMonitoringEvent(this.AlarmData.ResourceId, this.AlarmSn)
            {
                LoggingComputer = this.FusionDirectorIp,//对应 Logging Computer
                Channel = this.AlarmData.AlarmName.Split('#').Last(),//对应Log Name
                TimeGenerated = DateTime.Now,
                LevelId = (int)this.LevelId,//对应LevelId
                EventData = this.GetAlarmDataXml(),
                Message = new CustomMonitoringEventMessage(this.AlarmData.Additional)
            };
            customEvent.Parameters.Add($"{this.Severity}{this.MantissaNumber}");
            customEvent.Parameters.Add(this.Priority.ToString());
            customEvent.Parameters.Add(this.UnionId);
            customEvent.Parameters.Add(this.AlarmData.Sn.ToString());
            //customEvent.Parameters.Add(this.AlarmData.AlarmId ?? string.Empty);
            //customEvent.Parameters.Add(this.AlarmData.AlarmName ?? string.Empty);
            //customEvent.Parameters.Add(this.AlarmData.ResourceId ?? string.Empty);
            //customEvent.Parameters.Add(this.AlarmData.Sn.ToString());
            //customEvent.Parameters.Add(this.AlarmData.Additional ?? string.Empty);
            //customEvent.Parameters.Add(this.AlarmData.OccurTime);
            //customEvent.Parameters.Add(this.AlarmData.PossibleCause);
            //customEvent.Parameters.Add(this.AlarmData.Effect);
            return customEvent;

        }

        /// <summary>
        /// Gets the Custom XML data.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetAlarmDataXml()
        {
            var alarmData = (AlarmData)this.AlarmData.Clone();
            alarmData.AlarmName = alarmData.AlarmName.Split('#').Last();
            alarmData.Category = GetCategoryTxt(alarmData.Category);
            alarmData.Severity = GetSeverityTxt(alarmData.Severity);
            alarmData.IsClear = GetIsClearTxt(alarmData.IsClear);
            alarmData.Suggestion = ":" + Environment.NewLine + alarmData.Suggestion;
            return XmlHelper.SerializeToXmlStr(alarmData, true);
        }

        private string GetCategoryTxt(string category)
        {
            switch (category)
            {
                case "1":
                    return "Alert";
                case "2":
                    return "Recovery";
                case "3":
                    return "Event";
                default:
                    return category;
            }
        }

        private string GetSeverityTxt(string severity)
        {
            switch (severity)
            {
                case "1":
                    return "Critical";
                case "2":
                case "3":
                    return "Warning";
                case "4":
                    return "OK";
                default:
                    return severity;
            }
        }

        private string GetIsClearTxt(string isClear)
        {
            switch (isClear)
            {
                case "0":
                    return "YES";
                case "1":
                    return "NO";
                default:
                    return isClear;
            }
        }

        public string Description
        {
            get
            {
            /*
                The alert (Large Fan Speed Difference Warning Alarm) generated at 2020-03-22T17:23:18+08:00 for Fan1--1.rear (112.93.129.54_Fan1--1.rear) is caused by [Mock]Fan 1 rear failure or incorrect fan model. To rectify the fault, do as follows:
            1.Replace the fan module.
            2.If the server has a fan backplane, replace it.
            **/
                return $@"Alert ""{AlarmData.AlarmName.Split('#').Last()}"" was reported for ""{AlarmData.EventSubject}"" at {AlarmData.OccurTime}.
The possible cause is: ""{AlarmData.PossibleCause ?? string.Empty}"". Suggested resolution is: {Environment.NewLine}{AlarmData.Suggestion ?? string.Empty}.";
            }
        }
    }
}
 