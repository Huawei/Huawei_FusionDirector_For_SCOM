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
// Assembly         : FusionDirectorPlugin.Core
// Author           : yayun
// Created          : 02-25-2019
//
// Last Modified By : yayun
// Last Modified On : 02-26-2019
// ***********************************************************************
// <copyright file="ApplianceEvent.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using CommonUtil;
using Microsoft.EnterpriseManagement.Monitoring;

namespace FusionDirectorPlugin.Core.Models
{
    public class ApplianceEvent
    {
        public ApplianceEvent(ApplianceAlarm data)
        {
            this.AlarmData = data;
        }

        /// <summary>
        ///  0-information, 1-warning, 2-error
        /// </summary>
        /// <value>The severity.</value>
        public string Severity => AlarmData.Severity;

        /// <summary>
        /// 1-Error, 2-Warning, 4-Information, 8-Success Audit, 16-Failure Audit.
        /// </summary>
        /// <value>The level identifier.</value>
        public int LevelId
        {
            get
            {
                switch (this.Severity)
                {
                    case "0":
                        return 4;
                    case "1":
                        return 2;
                    case "2":
                        return 1;
                    default:
                        return 4;
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
                    case 1:
                        return 2;
                    case 2:
                        return 1;
                }
                return 0;
            }
        }

        public ApplianceAlarm AlarmData { get; set; }

        /// <summary>
        /// 1-告警 2-恢复 3-更新
        /// </summary>
        public string OptType => AlarmData.OptType;

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

        public CustomMonitoringEvent ToCustomMonitoringEvent()
        {
            var additional = string.IsNullOrWhiteSpace(this.AlarmData.Additional) ? "Additional" : this.AlarmData.Additional;
            var customEvent = new CustomMonitoringEvent(this.AlarmData.AlarmName, this.AlarmData.Sn)
            {
                LoggingComputer = Environment.MachineName,
                Channel = this.AlarmData.AlarmName,
                TimeGenerated = DateTime.Now,
                LevelId = this.LevelId,//对应LevelId
                EventData = this.GetAlarmDataXml(),
                Message = new CustomMonitoringEventMessage(additional)
            };
            customEvent.Parameters.Add(this.Priority.ToString());
            customEvent.Parameters.Add(this.Severity);
            customEvent.Parameters.Add(this.AlarmData.Sn.ToString());
            customEvent.Parameters.Add("ApplianceAlert");
            return customEvent;
        }

        private string GetAlarmDataXml()
        {
            var alarmData = (ApplianceAlarm)this.AlarmData.Clone();
            alarmData.AlarmName = alarmData.AlarmName;
            alarmData.Severity = GetSeverityTxt(alarmData.Severity);
            return XmlHelper.SerializeToXmlStr(alarmData, true);
        }
    }
}
