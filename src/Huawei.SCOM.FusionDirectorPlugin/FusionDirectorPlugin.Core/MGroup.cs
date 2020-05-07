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
// Created          : 01-04-2019
//
// Last Modified By : yayun
// Last Modified On : 01-04-2019
// ***********************************************************************
// <copyright file="MGroup.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using FusionDirectorPlugin.LogUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Timers;

using Microsoft.EnterpriseManagement;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.Configuration.IO;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.Packaging;

namespace FusionDirectorPlugin.Core
{
    /// <summary>
    /// The m group.
    /// </summary>
    public class MGroup : ManagementGroup
    {
        /// <summary>
        /// The this.
        /// </summary>
        private static MGroup instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroup"/> class.
        /// </summary>
        /// <param name="serverName">
        /// The server name.
        /// </param>
        public MGroup(string serverName)
            : base(serverName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroup"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
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
                    instance.InstallEnclosureConnector();
                    instance.InstallServerConnector();
                    instance.InstallApplianceConnector();
                    instance.InstallFdEntityConnector();
#else

                    instance = new MGroup("localhost");
#endif
                }

                if (!instance.IsConnected)
                {
                    instance.Reconnect();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the enclosure connector unique identifier.
        /// </summary>
        /// <value>The enclosure connector unique identifier.</value>
        public Guid EnclosureConnectorGuid => new Guid("{628C8486-2E62-42FB-9AFB-96CB8C089864}");

        /// <summary>
        /// Gets the server connector unique identifier.
        /// </summary>
        /// <value>The server connector unique identifier.</value>
        public Guid ServerConnectorGuid => new Guid("{728C8486-2E62-42FB-9AFB-96CB8C089864}");

        public Guid ApplianceConnectorGuid => new Guid("{828C8486-2E62-42FB-9AFB-96CB8C089864}");

        public readonly Guid FdEntityConnectorGuid = new Guid("{21e62afc-aaf9-417c-8125-c31273270969}");

        public DateTime MpInstallTime
        {
            get
            {
                if (mpInstallTime == null)
                {
                    mpInstallTime = GetMpIntallTime();
                }
                return mpInstallTime.Value;
            }
            set { mpInstallTime = value; }
        }

        private DateTime? mpInstallTime;

        /// <summary>
        /// 检查MP文件是否已安装
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        public bool CheckIsInstallMp(string name)
        {
            var criteria = new ManagementPackCriteria($"Name = '{name}'");
            return this.ManagementPacks.GetManagementPacks(criteria).Count > 0;
        }

        /// <summary>
        /// The get connector.
        /// </summary>
        /// <param name="connectorGuid">
        /// The connector guid.
        /// </param>
        /// <returns>
        /// The <see cref="MonitoringConnector"/>.
        /// </returns>
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
                return null;
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Init()
        {
            Timer timer = new Timer(5 * 60 * 1000)
            {
                Enabled = true,
                AutoReset = true,
            };
            timer.Elapsed += (s, e) =>
                {
                    try
                    {
                        // 保持对scom 的连接
                        this.GetManagementPackClass("Microsoft.Windows.Computer");
                    }
                    catch (Exception ex)
                    {
                        HWLogger.Service.Error(ex, "keep Management Group Connection error");
                        this.Reconnect();
                    }
                };
            timer.Start();
        }

        /// <summary>
        /// The get connector framework.
        /// </summary>
        /// <returns>
        /// The <see cref="IConnectorFrameworkManagement"/>.
        /// </returns>
        public IConnectorFrameworkManagement GetConnectorFramework()
        {
            var icfm = this.ConnectorFramework;
            return icfm;
        }

        /// <summary>
        /// The get management pack.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="ManagementPack"/>.
        /// </returns>
        public ManagementPack GetManagementPack(string name)
        {
            var criteria = new ManagementPackCriteria($"Name = '{name}'");
            return this.ManagementPacks.GetManagementPacks(criteria).FirstOrDefault();
        }

        /// <summary>
        /// The get management pack class.
        /// </summary>
        /// <param name="className">
        /// The class name.
        /// </param>
        /// <returns>
        /// The <see cref="ManagementPackClass"/>.
        /// </returns>
        /// <exception cref="ApplicationException">Failed to find management pack class </exception>
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
        /// The get management pack relationship.
        /// </summary>
        /// <param name="relationshipName">
        /// The relationship name.
        /// </param>
        /// <returns>
        /// The <see cref="ManagementPackRelationship"/>.
        /// </returns>
        /// <exception cref="ApplicationException">Failed to find monitoring relationship  </exception>
        public ManagementPackRelationship GetManagementPackRelationship(string relationshipName)
        {
            IList<ManagementPackRelationship> relationshipClasses;

            relationshipClasses =
                this.EntityTypes.GetRelationshipClasses(
                    new ManagementPackRelationshipCriteria("Name='" + relationshipName + "'"));

            if (relationshipClasses.Count == 0)
            {
                throw new ApplicationException("Failed to find monitoring relationship " + relationshipName);
            }
            return relationshipClasses[0];
        }

        #region Mp包安装卸载
        /// <summary>
        /// 安装MP
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public void InstallMp(string path)
        {
            var newMp = new ManagementPack(path);
            var criteria = new ManagementPackCriteria($"Name = '{newMp.Name}'");
            var oldMp = this.ManagementPacks.GetManagementPacks(criteria).FirstOrDefault();
            if (oldMp != null)
            {
                // 已安装则跳过
                HWLogger.Install.Warn($"Skip install：{oldMp.Name}-{oldMp.Version} has Installed.");
                //// 已安装
                // if (oldMp.Version != newMp.Version)
                // {
                // // 版本不一致则先进行卸载
                // this.UnInstallMp(oldMp.Name);
                // this.ManagementPacks.ImportManagementPack(newMp);
                // Console.WriteLine($"Install {newMp.Name} Finish.");
                // }
                // else
                // {
                // HwLogger.Install.Warn($"Skip install：{newMp.Name}-{newMp.Version} has Installed.");
                // }
            }
            else
            {
                this.ManagementPacks.ImportManagementPack(newMp);
                HWLogger.Install.Warn($"Install {newMp.Name} Finish.");
            }
        }

        /// <summary>
        /// Installs the MPB.
        /// </summary>
        /// <param name="path">The path.</param>
        public void InstallMpb(string path)
        {
            var mpStore = new ManagementPackFileStore();
            mpStore.AddDirectory(Path.GetDirectoryName(path));

            var reader = ManagementPackBundleFactory.CreateBundleReader();
            var bundle = reader.Read(path, mpStore);

            var newMp = bundle.ManagementPacks.FirstOrDefault();
            if (newMp == null)
            {
                HWLogger.Install.Warn($"Install faild. can not find ManagementPack in the path :{path}");
                return;
            }

            var criteria = new ManagementPackCriteria($"Name = '{newMp.Name}'");
            var oldMp = this.ManagementPacks.GetManagementPacks(criteria).FirstOrDefault();
            if (oldMp != null)
            {
                HWLogger.Install.Warn($"Skip install：{oldMp.Name}-{oldMp.Version} has Installed.");
            }
            else
            {
                this.ManagementPacks.ImportBundle(bundle);
                HWLogger.Install.Warn($"Install {newMp.Name} Finish.");
            }
        }

        /// <summary>
        /// Uninstalls the mp.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public void UnInstallMp(string name)
        {
            var criteria = new ManagementPackCriteria($"Name = '{name}'");
            var mp = this.ManagementPacks.GetManagementPacks(criteria).FirstOrDefault();
            if (mp == null)
            {
                Console.WriteLine($"ManagementPack {name} is not installed.");
            }
            else
            {
                this.ManagementPacks.UninstallManagementPack(mp);
                Console.WriteLine($"Uninstall {name} Finish.");
            }
        }

        #endregion

        #region Connector 安装卸载

        /// <summary>
        /// Installs the enclosure connector.
        /// </summary>
        public void InstallEnclosureConnector()
        {
            var connectorInfo = new ConnectorInfo
            {
                Description = "FusionDirector Enclosure Connector Description",
                DisplayName = "FusionDirector Enclosure Connector",
                Name = "FusionDirector.Enclosure.Connector",
                DiscoveryDataIsManaged = true
            };
            this.InstallConnector(connectorInfo, EnclosureConnectorGuid);
        }

        /// <summary>
        /// Installs the server connector.
        /// </summary>
        public void InstallServerConnector()
        {
            var connectorInfo = new ConnectorInfo
            {
                Description = "FusionDirector Server Connector Description",
                DisplayName = "FusionDirector Server Connector",
                Name = "FusionDirector.Server.Connector",
                DiscoveryDataIsManaged = true
            };
            this.InstallConnector(connectorInfo, ServerConnectorGuid);
        }

        /// <summary>
        /// Installs the Appliance connector.
        /// </summary>
        public void InstallApplianceConnector()
        {
            var connectorInfo = new ConnectorInfo
            {
                Description = "FusionDirector Appliance Connector Description",
                DisplayName = "FusionDirector Appliance Connector",
                Name = "FusionDirector.Appliance.Connector",
                DiscoveryDataIsManaged = true
            };
            this.InstallConnector(connectorInfo, ApplianceConnectorGuid);
        }

        /// <summary>
        /// Installs the fd Entity connector.
        /// </summary>
        public void InstallFdEntityConnector()
        {
            var connectorInfo = new ConnectorInfo
            {
                Description = "FusionDirector Entity Connector Description",
                DisplayName = "FusionDirector Entity Connector",
                Name = "FusionDirector.Entity.Connector",
                DiscoveryDataIsManaged = true
            };
            this.InstallConnector(connectorInfo, this.FdEntityConnectorGuid);
        }

        /// <summary>
        /// The install.
        /// </summary>
        /// <exception cref="Exception">ex</exception>
        public void InstallConnector(ConnectorInfo connectorInfo, Guid connectorGuid)
        {
            try
            {
                var connector = this.GetConnector(connectorGuid);
                if (connector == null)
                {
                    HWLogger.Install.Info($"Start install {connectorInfo.Name}");
                    var cfMgmt = MGroup.Instance.GetConnectorFramework();
                    cfMgmt.Setup(connectorInfo, connectorGuid);
                    HWLogger.Install.Info($"{connectorInfo.Name} install finish.");
                }
            }
            catch (Exception ex)
            {
                HWLogger.Install.Error(ex, "Install connector error:");
                throw;
            }
        }

        public void UnInstallEnclosureConnector()
        {
            this.UnInstallConnector(EnclosureConnectorGuid);
        }

        public void UnInstallServerConnector()
        {
            this.UnInstallConnector(ServerConnectorGuid);
        }

        public void UnInstallApplianceConnector()
        {
            this.UnInstallConnector(ApplianceConnectorGuid);
        }
        public void UnInstallFdEntityConnector()
        {
            this.UnInstallConnector(FdEntityConnectorGuid);
        }

        /// <summary>
        /// The un install connector.
        /// </summary>
        /// <param name="connectorGuid">The connector guid.</param>
        /// <exception cref="Exception">Error</exception>
        private void UnInstallConnector(Guid connectorGuid)
        {
            var montioringConnector = this.GetConnector(connectorGuid);
            Console.WriteLine($"Start Uninstall connector: {connectorGuid}");
            HWLogger.Install.Info($"Start Uninstall connector {connectorGuid}");
            try
            {
                if (montioringConnector != null)
                {
                    var connectorName = montioringConnector.Name;
                    var icfm = this.GetConnectorFramework();
                    IList<MonitoringConnectorSubscription> subscriptions =
                        icfm.GetConnectorSubscriptions().Where(c => c.MonitoringConnectorId.Equals(connectorGuid))
                            .ToList();
                    foreach (var subscription in subscriptions)
                    {
                        icfm.DeleteConnectorSubscription(subscription);
                    }
                    try
                    {
                        montioringConnector.Uninitialize();
                    }
                    catch (Exception ex)
                    {
                        HWLogger.Install.Error(ex, $"Error on {connectorName} Uninitialize.");
                        Console.WriteLine($" Error on {connectorName} Uninitialize", ex);
                    }

                    icfm.Cleanup(montioringConnector);
                }
                else
                {
                    HWLogger.Install.Info($"Error uninstalling : Can not find connector: {connectorGuid}");
                    Console.WriteLine($"Error uninstalling :Can not find connector: {connectorGuid} ");
                }
            }
            catch (Exception ex)
            {
                HWLogger.Install.Error(ex, "Error uninstalling connector...");
                Console.WriteLine($" Error uninstalling {connectorGuid} ", ex);
                throw;
            }

            Console.WriteLine($"{connectorGuid} UnInstall finish");
        }

        #endregion

        #region Private Methods

        #endregion

        /// <summary>
        /// Checks the connection.
        /// </summary>
        public void CheckConnection()
        {
            if (!this.IsConnected)
            {
                HWLogger.Service.Info("Reconnect");
                this.Reconnect();
            }
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
        public DateTime GetMpIntallTime()
        {
            var mp = GetManagementPack("Huawei.FusionDirector.View.Library");
            return mp.TimeCreated;
        }
    }
}