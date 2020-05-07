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
// Created          : 02-20-2019
//
// Last Modified By : yayun
// Last Modified On : 02-20-2019
// ***********************************************************************
// <copyright file="Appliance.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using FusionDirectorPlugin.Model;

namespace FusionDirectorPlugin.Core.Models
{
    /// <summary>
    /// Class Appliance.
    /// </summary>
    public class Appliance
    {

        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>The name of the host.</value>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the software version.
        /// </summary>
        /// <value>The software version.</value>
        public string SoftwareVersion { get; set; }

        public EnclosureCollection EnclosureCollection { get; set; }

        public ServerCollection ServerCollection { get; set; }

        public EventCollection EventCollection { get; set; }

        public PerformanceCollection PerformanceCollection { get; set; }

        public FusionDirectorCollection FusionDirectorCollection { get; set; }

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        public Health Health
        {
            get
            {
                var list = new List<Health>
                {
                    EnclosureCollection.Health,
                    ServerCollection.Health,
                    EventCollection.Health,
                    PerformanceCollection.Health,
                    FusionDirectorCollection.Health
                };
                if (list.Any(x => x == Health.Critical))
                {
                    return Health.Critical;
                }
                if (list.Any(x => x == Health.Warning))
                {
                    return Health.Warning;
                }
                return Health.OK;

            }
        }

    }
}
