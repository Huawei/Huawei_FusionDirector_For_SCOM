//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
// ***********************************************************************
// Assembly         : FusionDirectorPlugin.PackageHelper
// Author           : yayun
// Created          : 12-25-2018
//
// Last Modified By : yayun
// Last Modified On : 12-25-2018
// ***********************************************************************
// <copyright file="Program.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.EnterpriseManagement.Configuration.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using FusionDirectorPlugin.Core;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.Win32;
using FusionDirectorPlugin.Dal;
using FusionDirectorPlugin.Dal.Helpers;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.LogUtil;
using Microsoft.EnterpriseManagement.Packaging;
using FusionDirectorPlugin.Model;
using System.Security.Cryptography.X509Certificates;

namespace FusionDirectorPlugin.PackageHelper
{

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
#if DEBUG

        /// <summary>
        /// The run path.
        /// </summary>
        private static readonly string RunPath = @"E:\Projects\scom-plugin\SCOM\release\Configuration";
#else
        private static readonly string RunPath = AppDomain.CurrentDomain.BaseDirectory;
#endif
        /// <summary>
        /// The port
        /// </summary>
        private static int port;

        /// <summary>
        /// The port
        /// </summary>
        private static string ipAddress;

        private static string certPwd;

        private static string isEnableAlert;

        /// <summary>
        /// Gets or sets a value indicating whether is have exception.
        /// </summary>
        /// <value><c>true</c> if this instance is have exception; otherwise, <c>false</c>.</value>
        private static bool IsHaveException { get; set; }

        /// <summary>
        /// The service name.
        /// </summary>
        /// <value>The name of the service.</value>
        private static string PluginServiceName => "Huawei Fusion Director For SCOM plugin Service";

        /// <summary>
        /// Gets the framework install dir.
        /// </summary>
        /// <value>The framework install dir.</value>
        private static string FrameworkInstallDir => System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();

        /// <summary>
        /// Gets or sets the scom install path.
        /// </summary>
        /// <value>The scom install path.</value>
        private static string ScomInstallPath { get; set; }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">The args.</param>
        private static void Main(string[] args)
        {
            try
            {
                var tempFilePath = Path.Combine(FrameworkInstallDir, "Temporary ASP.NET Files");
                if (!Directory.Exists(tempFilePath))
                {
                    Directory.CreateDirectory(tempFilePath);
                }
                ScomInstallPath = ReadScomInstallPath();
                //CreateConfigLibraryMp();
#if DEBUG
                //ReadScomInstallPath();
                //ResetSubscribeStatus();
                // CreateConfigLibraryMp();
                // ModifyWebServerConfig();
                // Install();
                // SealMpb(Path.GetFullPath("..\\..\\..\\..\\..\\..\\..\\release\\MPFiles\\sealTemp"));
                // CreateMP(Path.GetFullPath("..\\..\\..\\..\\..\\..\\..\\release\\MPFiles\\Temp"), 44301);
                // UnInstall(); 
                Console.ReadLine();

#else
                if (args.Length == 0)
                {
                    return;
                }
                OnLog($"RunPath:{RunPath}");
                OnLog($"FrameworkInstallDir:{FrameworkInstallDir}");
                OnLog($"ScomInstallPath:{ScomInstallPath}");
                if (args[0] == "/u")
                {
                    if (!GetAttrValue(args, "keepIISExpress"))
                    {
                        UnInstallIISExpress();
                    }

                    UnInstall();
                    OnLog("PackageHelper work done.");
                }
                else if (args[0] == "/i")
                {
                    if (!int.TryParse(GetParamValue(args, "port"), out port))
                    {
                        throw new Exception("port is error");
                    }
                    if (port < 0 || port > 65536)
                    {
                        throw new Exception("port is error");
                    }
                    ipAddress = GetParamValue(args, "ip");
                    certPwd = GetParamValue(args, "certpwd");
                    OnLog($"ip:{ipAddress} port:{port} ");

                    isEnableAlert = GetParamValue(args, "isEnableAlert");
                    OnLog($"ip:{isEnableAlert} port:{port}");
                    var isPdRight = CertHelper.CheckPfxPwd(Path.GetFullPath($"{RunPath}\\..\\Certs\\scomforfd.pfx"), certPwd);
                    if (!isPdRight)
                    {
                        OnLog("The Certificate password is incorrect");
                        Environment.Exit(-2);
                    }
                    Install();
                    OnLog("PackageHelper work done.");
                }
                else if (args[0] == "/h")
                {
                    OnLog("/i install");
                    OnLog("/u uninstall");
                    Console.ReadLine();
                }
#endif
            }
            catch (Exception ex)
            {
                OnLog("Main Error", ex);
                Environment.Exit(-1);
            }
        }


        /// <summary>
        /// The install.
        /// </summary>
        private static void Install()
        {
            SaveConfig();
            CopySdkFiles();
            ModifyWebServerConfig();
            // 服务已安装 则跳过安装
            if (ServiceController.GetServices().All(s => s.ServiceName == PluginServiceName))
            {
                StopService();
                UnInstallService();
            }

            InstallService();
            if (!IsIISExpressInstalled())
            {
                InstallIISExpress();
            }
            ImportCerts();
            InstallMpPlugin();
            InstallConnector();
            // StartService();
        }

        /// <summary>
        /// The un install.
        /// </summary>
        private static void UnInstall()
        {
            // 删除服务
            if (ServiceController.GetServices().Any(s => s.ServiceName == PluginServiceName))
            {
                StopService();
                UnInstallService();
            }
            else
            {
                OnLog("Huawei SCOM Plugin For fusionDirector.Service is not installed");
            }

            // 卸载前先取消订阅
            UnSubscribeAll();

            RemoveFusionDirectorObjects();
            UnInstallConnector();
            UnInstallMpPlugin();
            UnInstallCerts();
        }

        #region Install

        /// <summary>
        /// Copies the SDK files.
        /// </summary>
        /// <exception cref="System.Exception">
        /// can not find sdk dll " + packagingFile
        /// or
        /// can not find sdk dll " + operationsManagerFile
        /// or
        /// can not find sdk dll " + coreFile
        /// or
        /// can not find sdk dll " + runtimeFile
        /// </exception>
        private static void CopySdkFiles()
        {
            OnLog("CopySdkFiles Start.");
            var packagingFile = "Microsoft.EnterpriseManagement.Packaging.dll";
            var packagingFilePath = Path.Combine(ScomInstallPath, "Setup", packagingFile);

            var operationsManagerFile = "Microsoft.EnterpriseManagement.OperationsManager.dll";
            var operationsManagerFilePath = Path.Combine(ScomInstallPath, "Server\\SDK Binaries", operationsManagerFile);
            var coreFile = "Microsoft.EnterpriseManagement.Core.dll";
            var coreFilePath = Path.Combine(ScomInstallPath, "Server\\SDK Binaries", coreFile);
            var runtimeFile = "Microsoft.EnterpriseManagement.Runtime.dll";
            var runtimeFilePath = Path.Combine(ScomInstallPath, "Server\\SDK Binaries", runtimeFile);
            if (!File.Exists(packagingFilePath))
            {
                throw new Exception("can not find sdk dll " + packagingFile);
            }
            if (!File.Exists(operationsManagerFilePath))
            {
                throw new Exception("can not find sdk dll " + operationsManagerFile);
            }
            if (!File.Exists(coreFilePath))
            {
                throw new Exception("can not find sdk dll " + coreFile);
            }
            if (!File.Exists(runtimeFilePath))
            {
                throw new Exception("can not find sdk dll " + runtimeFile);
            }

            File.Copy(packagingFilePath, Path.Combine(RunPath, packagingFile), true);
            File.Copy(operationsManagerFilePath, Path.Combine(RunPath, operationsManagerFile), true);
            File.Copy(coreFilePath, Path.Combine(RunPath, coreFile), true);
            File.Copy(runtimeFilePath, Path.Combine(RunPath, runtimeFile), true);
            OnLog("CopySdkFiles Finish.");
        }

        /// <summary>
        /// Installs the connector.
        /// </summary>
        private static void InstallConnector()
        {
            try
            {
                OnLog("Install Connectors");
                OnLog("Start Install EnclosureConnector");
                MGroup.Instance.InstallEnclosureConnector();
                OnLog("Start Install ServerConnector");
                MGroup.Instance.InstallServerConnector();
                OnLog("Start Install ApplianceConnector");
                MGroup.Instance.InstallApplianceConnector();
                OnLog("Start Install FdEntityConnector");
                MGroup.Instance.InstallFdEntityConnector();
                OnLog("Install Connectors success");
            }
            catch (Exception ex)
            {
                OnLog("Install Connector faild", ex);
                IsHaveException = true;
                throw ex;
            }
        }

        /// <summary>
        /// The install iis express.
        /// </summary>
        private static void InstallIISExpress()
        {
            try
            {
                OnLog("Install IISExpress");
                var process = new Process
                {
                    StartInfo =
                     {
                         FileName = "msiexec",
                         Arguments =
                             $" /i \"{RunPath}\\iisexpress_amd64_en-US.msi\" /qn /l* iisexpress_amd64_en-US.install.log",
                         Verb = "runas",
                         WindowStyle = ProcessWindowStyle.Hidden,
                         UseShellExecute = false,
                         RedirectStandardInput = true,
                         RedirectStandardOutput = true,
                         RedirectStandardError = true
                     }
                };
                process.OutputDataReceived += (s, e) => { OnLog(e.Data); };
                process.ErrorDataReceived += (s, e) => { OnLog(e.Data); };
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit(60000);
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("Install IISExpress faild", ex);
                throw ex;
            }
        }

        /// <summary>
        /// The install mp plugin.
        /// </summary>
        private static void InstallMpPlugin()
        {
            try
            {
                OnLog("Start Install fusionDirector ManagementPacks");
#if DEBUG
                MGroup.Instance.InstallMpb(Path.GetFullPath("..\\..\\..\\..\\..\\..\\release\\MPFiles\\Huawei.FusionDirector.View.Library.mpb"));
                MGroup.Instance.InstallMpb(Path.GetFullPath("..\\..\\..\\..\\..\\..\\release\\MPFiles\\Huawei.FusionDirector.Enclosure.Library.mpb"));
                MGroup.Instance.InstallMpb(Path.GetFullPath("..\\..\\..\\..\\..\\..\\release\\MPFiles\\Huawei.FusionDirector.Server.Library.mpb"));
#else
                OnLog("Start Install Huawei.FusionDirector.View.Library");
                MGroup.Instance.InstallMpb(Path.GetFullPath($"{RunPath}\\..\\MPFiles\\Huawei.FusionDirector.View.Library.mpb"));
                OnLog("Start Install Huawei.FusionDirector.Enclosure.Library");
                MGroup.Instance.InstallMpb(Path.GetFullPath($"{RunPath}\\..\\MPFiles\\Huawei.FusionDirector.Enclosure.Library.mpb"));
                OnLog("Start Install Huawei.FusionDirector.Server.Library");
                MGroup.Instance.InstallMpb(Path.GetFullPath($"{RunPath}\\..\\MPFiles\\Huawei.FusionDirector.Server.Library.mpb"));
#endif
                OnLog("Install FusionDirector ManagementPacks Finish.");
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("Install ManagementPacks faild.", ex);
                throw ex;
            }
        }

        /// <summary>
        /// The install service.
        /// </summary>
        private static void InstallService()
        {
            try
            {
                OnLog($"Install {PluginServiceName} as Windows Service");
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = $"{FrameworkInstallDir}\\InstallUtil.exe",
                        Arguments =
                            $" \"{RunPath}\\FusionDirectorPlugin.Service.exe\"",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                process.OutputDataReceived += (s, e) => { OnLog(e.Data); };
                process.ErrorDataReceived += (s, e) => { OnLog(e.Data); };
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit(60000);
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("InstallService", ex);
                throw ex;
            }
        }

        /// <summary>
        /// The is iis express installed.
        /// </summary>
        /// <returns>The <see cref="bool" />.</returns>
        private static bool IsIISExpressInstalled()
        {
            var path = Path.GetFullPath(
                $"{Environment.GetEnvironmentVariable("ProgramFiles")}\\IIS Express\\iisexpress.exe");
            return File.Exists(path);
        }

        /// <summary>
        /// Import Certs.
        /// </summary>
        private static void ImportCerts()
        {
            try
            {
                OnLog($"ImportCerts Start");

                CertHelper.ImportCertByPath(Path.GetFullPath($"{RunPath}\\..\\Certs\\HuaweiEquipmentRootCA.crt"));
                CertHelper.ImportCertByPath(Path.GetFullPath($"{RunPath}\\..\\Certs\\HuaweiITProductCA.crt"));
                CertHelper.ImportPfxByPath(Path.GetFullPath($"{RunPath}\\..\\Certs\\scomforfd.pfx"), certPwd);
                OnLog($"ImportCerts Finish");
                DeleteCertBinding();
                AddCertBinding();
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("ImportCerts", ex);
                throw ex;
            }
        }

        private static void AddCertBinding()
        {
            try
            {
                OnLog($"start set web server ssl ");
                RunCmd("netsh http add sslcert ipport=0.0.0.0:" + port + " certhash=23d05922ea28365820120ed7a2f98b34dcd09222 appid={214124cd-d05b-4309-9af9-9caa44b2b74a}");
                OnLog($" set web server ssl end");
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("AddCertBinding", ex);
                throw ex;
            }
        }

        private static void DeleteCertBinding()
        {
            try
            {
                OnLog($"start delete web server ssl ");
                RunCmd("netsh http delete sslcert ipport=0.0.0.0:" + port + "");
                OnLog($" delete web server ssl end");
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("DeleteSSLCertBinding", ex);
                throw ex;
            }
        }

        #endregion

        #region UnInstall

        /// <summary>
        /// The kill.
        /// </summary>
        private static void KillService()
        {
            OnLog("Kill Service Process Start");
            try
            {
                var p = Process.GetProcessesByName("FusionDirectorPlugin.Service");
                if (p.Length > 0)
                {
                    OnLog("Kill Process : FusionDirectorPlugin.Service");
                    p.First().Kill();
                }

                p = Process.GetProcessesByName("iisexpress");
                if (p.Length > 0)
                {
                    OnLog("Kill Process : iisexpress");
                    p.First().Kill();
                }
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("Kill Process faild", ex);
            }
        }

        /// <summary>
        /// 删除所有管理的FusionDirector对象
        /// </summary>
        private static void RemoveFusionDirectorObjects()
        {
            try
            {
                OnLog($"Remove fusionDirector Servers From SCOM Start.");
                EnclosureConnector.Instance.RemoveAllEnclosure();
                ServerConnector.Instance.RemoveAllServer();
                ApplianceConnector.Instance.RemoveAppliance();
                OnLog($"Remove fusionDirector Servers From SCOM Finish.");
            }
            catch (Exception ex)
            {
                OnLog("Remove fusionDirector Servers From SCOM error", ex);
            }
        }

        /// <summary>
        /// The start service.
        /// </summary>
        private static void StartService()
        {
            try
            {
                OnLog("Start Huawei SCOM Plugin For fusionDirector.Service");
                var sc = new ServiceController { ServiceName = PluginServiceName };
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    sc.Start();
                }
                OnLog("Start Service success");
            }
            catch (Exception ex)
            {
                OnLog("Start Service faild", ex);
                IsHaveException = true;
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <exception cref="System.Exception">Stop Service Error：" + ServiceName</exception>
        private static void StopService()
        {
            try
            {
                OnLog($"Stop {PluginServiceName}");
                var service = new ServiceController { ServiceName = PluginServiceName };
                if (service.Status == ServiceControllerStatus.Running || service.Status == ServiceControllerStatus.StartPending || service.Status == ServiceControllerStatus.ContinuePending)
                {
                    service.Stop();
                    for (int i = 0; i < 60; i++)
                    {
                        service.Refresh();
                        System.Threading.Thread.Sleep(1000);
                        if (service.Status == ServiceControllerStatus.Stopped)
                        {
                            break;
                        }
                        if (i == 59)
                        {
                            throw new Exception("Stop Service Error：" + PluginServiceName);
                        }
                    }
                }
                OnLog("Stop Service success");
            }
            catch (Exception ex)
            {
                OnLog("Stop Service faild", ex);
                KillService(); // 服务停止失败 杀死进程。
                IsHaveException = true;
            }
        }

        /// <summary>
        /// UnInstalls the connector.
        /// </summary>
        private static void UnInstallConnector()
        {
            try
            {
                OnLog("UnInstall Connectors");

                OnLog("Start UnInstall EnclosureConnector");
                MGroup.Instance.UnInstallEnclosureConnector();
                OnLog("Start UnInstall ServerConnector");
                MGroup.Instance.UnInstallServerConnector();
                OnLog("Start UnInstall ApplianceConnector");
                MGroup.Instance.UnInstallApplianceConnector();
                OnLog("Start UnInstall FdEntityConnector");
                MGroup.Instance.UnInstallFdEntityConnector();

                OnLog("UnInstall Connector success");
            }
            catch (Exception ex)
            {
                OnLog("UnInstall Connector faild", ex);
                IsHaveException = true;
            }
        }

        /// <summary>
        /// UnInstalls the IIS express.
        /// </summary>
        private static void UnInstallIISExpress()
        {
            try
            {
                OnLog("Start UnInstall IISExpress");
                DeleteCertBinding();
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = "msiexec",
                        Arguments =
                            $" /x \"{RunPath}\\iisexpress_amd64_en-US.msi\" /qn /l* iisexpress_amd64_en-US.uninstall.log",
                        Verb = "runas",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                process.OutputDataReceived += (s, e) => { OnLog(e.Data); };
                process.ErrorDataReceived += (s, e) => { OnLog(e.Data); };
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit(60000);

                OnLog("UnInstall IISExpress success");
            }
            catch (Exception ex)
            {
                OnLog("Stop Service faild", ex);
                IsHaveException = true;
            }
        }

        /// <summary>
        /// UnInstalls the mp.
        /// </summary>
        private static void UnInstallMpPlugin()
        {
            try
            {
                OnLog("Start UnInstall FusionDirector ManagementPacks");
                OnLog("Start UnInstall Huawei.FusionDirector.Server.Library");
                MGroup.Instance.UnInstallMp("Huawei.FusionDirector.Server.Library");
                OnLog("Start UnInstall FusionDirector.Enclosure.Library");
                MGroup.Instance.UnInstallMp("Huawei.FusionDirector.Enclosure.Library");
                OnLog("Start UnInstall Huawei.FusionDirector.View.Library");
                MGroup.Instance.UnInstallMp("Huawei.FusionDirector.View.Library");
                OnLog("UnInstall fusionDirector ManagementPacks Finish.");
            }
            catch (Exception ex)
            {
                OnLog("UnInstall FusionDirector ManagementPacks faild", ex);
                IsHaveException = true;
            }
        }

        /// <summary>
        /// Uns the install service.
        /// </summary>
        private static void UnInstallService()
        {
            try
            {
                OnLog($"Start UnInstall {PluginServiceName}");
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = $"{FrameworkInstallDir}\\InstallUtil.exe",
                        Arguments =
                            $"/u \"{RunPath}FusionDirectorPlugin.Service.exe\"",
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };
                process.OutputDataReceived += (s, e) => { OnLog(e.Data); };
                process.ErrorDataReceived += (s, e) => { OnLog(e.Data); };
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit(60000);
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("UnInstall Service faild", ex);
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        private static void UnSubscribeAll()
        {
            try
            {
                OnLog("Start cancel fusionDirector subcribe.");

                var list = FusionDirectorDal.Instance.GetList();
                foreach (var fusionDirector in list)
                {
                    if (!string.IsNullOrEmpty(fusionDirector.SubscribeId))
                    {
                        try
                        {
                            using (EventService eventService = new EventService(fusionDirector))
                            {
                                eventService.DeleteGivenSubscriptions(fusionDirector.SubscribeId);
                            }
                        }
                        catch (Exception ex)
                        {
                            OnLog($"UnSubscribe fusionDirector:{fusionDirector.HostIP}", ex);
                        }
                    }
                }
                OnLog("FusionDirector cancel subcribe finish.");
            }
            catch (Exception ex)
            {
                OnLog("FusionDirector cancel subcribe:", ex);
            }
        }

        private static void UnInstallCerts()
        {
            try
            {
                OnLog($"Start Remove Certs");
                CertHelper.RemoveCert("23d05922ea28365820120ed7a2f98b34dcd09222", StoreName.My);
            }
            catch (Exception ex)
            {
                IsHaveException = true;
                OnLog("UnInstall Service faild", ex);
            }
        }

        #endregion

        #region Utils
        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>System.Boolean.</returns>
        private static string GetParamValue(string[] args, string attrName)
        {
            foreach (var s in args)
            {
                if (s.Contains(attrName))
                {
                    return s.Split('=')[1].Trim();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets the attribute value.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="attrName">Name of the attribute.</param>
        /// <returns>System.Boolean.</returns>
        private static bool GetAttrValue(string[] args, string attrName)
        {
            foreach (var s in args)
            {
                if (s.Contains(attrName))
                {
                    return Convert.ToBoolean(s.Split('=')[1].Trim());
                }
            }
            return false;
        }

        /// <summary>
        /// The on log.
        /// </summary>
        /// <param name="data">The data.</param>
        private static void OnLog(string data)
        {
            HWLogger.Install.Info(data);
            Console.WriteLine(data);
        }

        /// <summary>
        /// The on log.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="ex">The ex.</param>
        private static void OnLog(string data, Exception ex)
        {
            HWLogger.Install.Error(ex, data);
            Console.WriteLine(data);
            Console.WriteLine(ex);
        }

        /// <summary>
        /// Reads the scom install path.
        /// </summary>
        /// <returns>System.String.</returns>
        /// <exception cref="System.Exception"></exception>
        private static string ReadScomInstallPath()
        {
            string softPath = @"SOFTWARE\Microsoft\System Center Operations Manager\12\Setup";
            RegistryKey localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
            RegistryKey installKey = localKey.OpenSubKey(softPath, false);
            if (installKey == null)
            {
                throw new Exception($"can not find the installKey: {softPath}");
            }
            var installPath = installKey.GetValue("InstallDirectory").ToString();
            return installPath;
        }
        #endregion

        #region Modify applicationhost.config

        /// <summary>
        /// Modifies the web server configuration.
        /// </summary>
        private static void ModifyWebServerConfig()
        {
            try
            {
                var filePath = Path.Combine(RunPath, "applicationhost.config");
                // filePath= Path.Combine("..\\..\\..\\..\\..\\..\\..\\release\\Configuration\\", "applicationhost.config");
                // 读取文本　  
                StreamReader sr = new StreamReader(filePath);
                string str = sr.ReadToEnd();
                sr.Close();
                // 替换文本  
                str = str.Replace("44301", port.ToString());
                // 更改保存文本  
                StreamWriter sw = new StreamWriter(filePath, false);
                sw.WriteLine(str);
                sw.Close();
            }
            catch (Exception ex)
            {
                OnLog("Modify applicationhost.config faild", ex);

                throw;
            }

        }
        #endregion

        /// <summary>
        /// 保存配置文件
        /// </summary>
        private static void SaveConfig()
        {
            var config = new PluginConfig
            {
                InternetIp = ipAddress,
                InternetPort = port,
                PollingInterval = 3600000 * 4,
                TempTcpPort = 40001,
                IsEnableAlert = true,
                ThreadCount = 2,
                RateLimitQueueSize = 60,
                RateLimitTimeSpan = 30
            };
            var path = $"{RunPath}\\PluginConfig.xml";
            ConfigHelper.SavePluginConfig(config, path);
        }

        public static void RunCmd(string cmd)
        {
            cmd = cmd.Trim().TrimEnd('&') + "&exit";//说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口写入命令
                p.StandardInput.WriteLine(cmd);
                p.StandardInput.AutoFlush = true;

                //获取cmd窗口的输出信息
                OnLog(p.StandardOutput.ReadToEnd());
                p.WaitForExit();//等待程序执行完退出进程
                p.Close();
            }
        }
    }
}
