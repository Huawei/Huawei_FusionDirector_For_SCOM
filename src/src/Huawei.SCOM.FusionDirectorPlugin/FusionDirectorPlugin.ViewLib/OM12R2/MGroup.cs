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
// Assembly         : FusionDirectorPlugin.ViewLib
// Author           : mike
// Created          : 05-09-2019
//
// Last Modified By : mike
// Last Modified On : 05-09-2019
// ***********************************************************************
// <copyright file="MGroup.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FusionDirectorPlugin.ViewLib.Utils;
using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.Monitoring;
using System.Security;

namespace FusionDirectorPlugin.ViewLib.OM12R2
{
    /// <summary>
    /// The m group.
    /// </summary>
    /// <seealso cref="Microsoft.EnterpriseManagement.ManagementGroup" />
    public class MGroup : ManagementGroup
    {
        /// <summary>
        /// The this.
        /// </summary>
        private static MGroup instance;

        public readonly Guid FdEntityConnectorGuid = new Guid("{21e62afc-aaf9-417c-8125-c31273270969}");

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroup" /> class.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        public MGroup(string serverName)
            : base(serverName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroup" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public MGroup(ManagementGroupConnectionSettings settings)
            : base(settings)
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static MGroup Instance
        {
            get
            {
                if (instance == null)
                {
#if DEBUG
                    var settings = new ManagementGroupConnectionSettings("192.168" + ".8.12")
                    {
                        UserName = "scom",
                        Domain = "MOSAI",//"MOSAI",
                        Password = ConvertToSecureString("Mosai520"),//Mosai@520
                    };
                    instance = new MGroup(settings);
#else

                    instance = new MGroup("localhost");
#endif
                }

                return instance;
            }
        }

        /// <summary>
        /// The get connector.
        /// </summary>
        /// <param name="connectorGuid">The connector guid.</param>
        /// <returns>The <see cref="MonitoringConnector" />.</returns>
        public MonitoringConnector GetConnector(Guid connectorGuid)
        {
            var cfMgmt = this.GetConnectorFramework();
            try
            {
                var montioringConnector = cfMgmt.GetConnector(connectorGuid);
                return montioringConnector;
            }
            catch (ObjectNotFoundException)
            {
                throw new ApplicationException("GetConnector Faild:FdEntityConnector");
            }
        }

        /// <summary>
        /// The get connector framework.
        /// </summary>
        /// <returns>The <see cref="IConnectorFrameworkManagement" />.</returns>
        public IConnectorFrameworkManagement GetConnectorFramework()
        {
            var icfm = this.ConnectorFramework;
            return icfm;
        }

        /// <summary>
        /// The get management pack class.
        /// </summary>
        /// <param name="className">The class name.</param>
        /// <returns>The <see cref="ManagementPackClass" />.</returns>
        /// <exception cref="ApplicationException">Failed to find management pack class</exception>
        public ManagementPackClass GetManagementPackClass(string className)
        {
            IList<ManagementPackClass> mpClasses;

            mpClasses = this.EntityTypes.GetClasses(new ManagementPackClassCriteria("Name='" + className + "'"));

            if (mpClasses.Count == 0)
            {
                throw new ApplicationException("Failed to find management pack class " + className);
            }
            return mpClasses[0];
        }

        /// <summary>
        /// Exsitses the specified mp class.
        /// </summary>
        /// <param name="mpClass">The mp class.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Exsits(ManagementPackClass mpClass, string criteria)
        {
            var c = new EnterpriseManagementObjectCriteria(criteria, mpClass);
            var reader = this.EntityObjects.GetObjectReader<PartialMonitoringObject>(c, ObjectQueryOptions.Default);
            return reader.Any();
        }

        /// <summary>
        /// Alls the specified entity class name.
        /// </summary>
        /// <param name="entityClass">The entity class.</param>
        /// <returns>IObjectReader&lt;EnterpriseManagementObject&gt;.</returns>
        public IObjectReader<EnterpriseManagementObject> All(ManagementPackClass entityClass)
        {
            IObjectReader<EnterpriseManagementObject> items = this.EntityObjects
                .GetObjectReader<EnterpriseManagementObject>(entityClass, ObjectQueryOptions.Default);
            return items;
        }

        /// <summary>
        /// Queries the specified entity class name.
        /// </summary>
        /// <param name="entityClass">The entity class.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IObjectReader&lt;EnterpriseManagementObject&gt;.</returns>
        public IObjectReader<EnterpriseManagementObject> Query(ManagementPackClass entityClass, string criteria)
        {
            EnterpriseManagementObjectCriteria c = new EnterpriseManagementObjectCriteria(criteria, entityClass);
            IObjectReader<EnterpriseManagementObject> items = this.EntityObjects
                .GetObjectReader<EnterpriseManagementObject>(c, ObjectQueryOptions.Default);
            return items;
        }

#if DEBUG
        /// <summary>
        /// ConvertToSecureString
        /// </summary>
        /// <param name="pd">pd</param>
        /// <returns>SecureString</returns>
        private static SecureString ConvertToSecureString(string pd)
        {
            if (pd == null)
            {
                throw new ArgumentNullException("pd");
            }

            var securePd = new SecureString();
            foreach (char c in pd)
            {
                securePd.AppendChar(c);
            }
            securePd.MakeReadOnly();
            return securePd;
        }
#endif
    }
}