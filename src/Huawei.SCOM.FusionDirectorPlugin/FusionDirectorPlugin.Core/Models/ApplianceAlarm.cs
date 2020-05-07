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
// Created          : 02-22-2019
//
// Last Modified By : yayun
// Last Modified On : 02-22-2019
// ***********************************************************************
// <copyright file="ApplianceAlarm.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using CommonUtil;
using System;

namespace FusionDirectorPlugin.Core.Models
{
    /// <summary>
    /// Class ApplianceAlarm.
    /// </summary>
    public class ApplianceAlarm : ICloneable
    {
        public ApplianceAlarm()
        {
          
            this.Sn = SystemUtil.GenRadomInt();
            this.Severity = "2";
            this.Additional = "Additional";
            this.OccurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Gets or sets the sn.
        /// </summary>
        /// <value>The sn.</value>
        public int Sn { get; set; }

        /// <summary>
        /// 1-告警 2-恢复 3-更新
        /// </summary>
        /// <value>The type of the opt.</value>
        public string OptType { get; set; }

        /// <summary>
        /// 0-information, 1-warning, 2-error
        /// </summary>
        /// <value>The severity.</value>
        public string Severity { get; set; }

        /// <summary>
        /// Gets or sets the name of the alarm.
        /// </summary>
        /// <value>The name of the alarm.</value>
        public string AlarmName { get; set; }

        /// <summary>
        /// Gets or sets the alarm identifier.
        /// </summary>
        /// <value>The alarm identifier.</value>
        public EnumAlarmType AlarmType { get; set; }

        /// <summary>
        /// Gets or sets the occur time.
        /// </summary>
        /// <value>The occur time.</value>
        public string OccurTime { get; set; }

        /// <summary>
        /// Gets or sets the possible cause.
        /// </summary>
        /// <value>The possible cause.</value>
        public string PossibleCause { get; set; }

        /// <summary>
        /// Gets or sets the suggstion.
        /// </summary>
        /// <value>The suggstion.</value>
        public string Suggstion { get; set; }

        /// <summary>
        /// Gets or sets the additional.
        /// </summary>
        /// <value>The additional.</value>
        public string Additional { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
