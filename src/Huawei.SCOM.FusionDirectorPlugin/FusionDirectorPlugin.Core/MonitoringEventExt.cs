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
// Created          : 01-22-2019
//
// Last Modified By : yayun
// Last Modified On : 01-21-2019
// ***********************************************************************
// <copyright file="MonitoringEventExt.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.IO;
using System.Xml.Serialization;
using FusionDirectorPlugin.Core.Models;
using FusionDirectorPlugin.Model.Event;
using Microsoft.EnterpriseManagement.Monitoring;

namespace FusionDirectorPlugin.Core
{
    /// <summary>
    /// Class MonitoringEventExt.
    /// </summary>
    public static class MonitoringEventExt
    {
        /// <summary>
        /// Gets the alarm data.
        /// </summary>
        /// <param name="monitoringEvent">The monitoring event.</param>
        /// <returns>AlarmData.</returns>
        public static AlarmData GetAlarmData(this MonitoringEvent monitoringEvent)
        {
            using (StringReader sr = new StringReader(monitoringEvent.EventData))
            {
                XmlSerializer xz = new XmlSerializer(typeof(AlarmData));
                return xz.Deserialize(sr) as AlarmData;
            }
        }
        public static ApplianceAlarm GetApplianceAlarm(this MonitoringEvent monitoringEvent)
        {
            using (StringReader sr = new StringReader(monitoringEvent.EventData))
            {
                XmlSerializer xz = new XmlSerializer(typeof(ApplianceAlarm));
                return xz.Deserialize(sr) as ApplianceAlarm;
            }
        }
    }
}
