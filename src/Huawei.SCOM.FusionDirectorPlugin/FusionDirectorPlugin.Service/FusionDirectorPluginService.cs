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
// Assembly         : FusionDirectorPlugin.Service
// Author           : yayun
// Created          : 01-04-2019
//
// Last Modified By : yayun
// Last Modified On : 02-21-2019
// ***********************************************************************
// <copyright file="FusionDirectorPluginService.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using FusionDirectorPlugin.Core;
using FusionDirectorPlugin.Dal;
using FusionDirectorPlugin.Dal.Helpers;
using FusionDirectorPlugin.Dal.Model;
using FusionDirectorPlugin.LogUtil;
using Timer = System.Timers.Timer;
using System.Threading.Tasks;
using CommonUtil;
using FusionDirectorPlugin.Core.Models;
using FusionDirectorPlugin.Model;
using Newtonsoft.Json;
using FusionDirectorPlugin.Model.Event;
using System.Net.Security;
using Microsoft.EnterpriseManagement.Monitoring;
using static Huawei.SCOM.ESightPlugin.Const.Constants.ESightEventeLogSource;

namespace FusionDirectorPlugin.Service
{
    /// <summary>
    /// The e sight plugin service.
    /// </summary>
    /// <seealso cref="System.ServiceProcess.ServiceBase" />
    public partial class FusionDirectorPluginService : ServiceBase
    {
        /// <summary>
        /// The polling timer
        /// </summary>
        private Timer pollingTimer;

        private Timer checkApplianceTimer;

        private Timer checkFdChangesTimer;

        /// <summary>
        /// The lock refresh PWDS
        /// </summary>
        private object lockRefreshPwds = new object();

        private PluginConfig pluginConfig;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FusionDirectorPluginService" /> class.
        /// </summary>
        public FusionDirectorPluginService()
        {
            this.InitializeComponent();
            SetCertificateValidation();
            pluginConfig = ConfigHelper.GetPluginConfig();
            this.SyncInstances = new List<FusionDirectorSyncInstance>();
            this.CanHandlePowerEvent = true;
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new NullToEmptyStringResolver()
            };
        }
        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the synchronize instances.
        /// </summary>
        /// <value>The synchronize instances.</value>
        public List<FusionDirectorSyncInstance> SyncInstances { get; set; }

        /// <summary>
        /// The env variable.
        /// </summary>
        /// <value>The env variable.</value>
        public string EnvVariable => "FDSCOMPLUGIN";

        /// <summary>
        /// Gets or sets the iis process.
        /// </summary>
        /// <value>The IIS process.</value>
        public Process IISProcess { get; set; }

        /// <summary>
        /// The install path.
        /// </summary>
        /// <value>The install path.</value>
        public string InstallPath => Environment.GetEnvironmentVariable(this.EnvVariable);

        /// <summary>
        /// 接收web转发的订阅消息
        /// </summary>
        /// <value>The TCP server taxk.</value>
        public Task TcpServerTask { get; private set; }
        #endregion

        #region Public Methods

        /// <summary>
        /// The debug.
        /// </summary>
        public void Debug()
        {
            this.OnStart(new[] { string.Empty });
            //this.CreateTcpServer();
            //this.CheckAndUpgradeKey();
        }

        /// <summary>
        /// Tests the update task.
        /// </summary>
        /// <param name="data">The data.</param>
        public void TestUpdateTask(AlarmData data)
        {
            var fdList = FusionDirectorDal.Instance.GetList();
            if (fdList != null)
            {
                this.OnLog($"fdList Count :{fdList.Count}");
                foreach (var x in fdList)
                {
                    var instance = this.FindInstance(x);
                    instance.DealNewAlarmAsync(data);
                }
            }
        }

        #endregion

        #region Task
        /// <summary>
        /// The on start.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="Exception">ex</exception>
        protected override void OnStart(string[] args)
        {
            try
            {
                var maxTryTimes = 0;
                while (true)
                {
                    maxTryTimes++;
                    if (this.CheckScomConnection())
                    {
                        this.Start();
                        break;
                    }
                    else
                    {
                        if (maxTryTimes >= 200)
                        {
                            this.OnError($"Stop retry: maxTryTimes:{maxTryTimes}");
                            break;
                        }
                        this.OnError("The Data Access service is either not running or not yet initialized,try again 3 seconds later.");
                        Thread.Sleep(3000);
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnLog("Start Service Error：" + ex);
                this.Stop();
            }
        }

        /// <summary>
        /// The on stop.
        /// </summary>
        /// <exception cref="Exception">ex</exception>
        protected override void OnStop()
        {
            try
            {
                if (this.IISProcess != null)
                {
                    if (!this.IISProcess.HasExited)
                    {
                        this.OnLog("Kill IISProcess");
                        this.IISProcess.Kill();
                    }
                    else
                    {
                        this.OnLog("IISProcess Has Exited");
                    }
                }

                // we should stop all sync instance before stop
                StopAllFusionDirectorSyncInstance();

                this.OnLog("Stop Service Success");
            }
            catch (Exception ex)
            {
                this.OnLog("Stop Service Error：" + ex);
            }
        }

        protected void StopAllFusionDirectorSyncInstance()
        {
            this.SyncInstances.ForEach((instance) =>
            {
                instance.Close();
            });
        }

        /// <summary>
        /// Called when [power event].
        /// </summary>
        /// <param name="powerStatus">The power status.</param>
        /// <returns>System.Boolean.</returns>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            string message = $" powerStatus:{powerStatus} tcpServerTaskStatus:{this.TcpServerTask.Status} this.pollingTimer{this.pollingTimer.Enabled} ";
            this.OnLog(message);
            return true;
        }

        /// <summary>
        /// Called when [continue].
        /// </summary>
        protected override void OnContinue()
        {
            string message = $"OnContinue: tcpServerTaskStatus:{this.TcpServerTask.Status} this.pollingTimer{this.pollingTimer.Enabled} ";
            this.OnLog(message);
        }

        /// <summary>
        /// Called when [pause].
        /// </summary>
        protected override void OnPause()
        {
            string message = $"OnPause: tcpServerTaskStatus:{this.TcpServerTask.Status} this.pollingTimer{this.pollingTimer.Enabled} ";
            this.OnLog(message);
        }

        /// <summary>
        /// Checks the connection.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        private bool CheckScomConnection()
        {
            try
            {
                MGroup.Instance.Init();
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString() != "Microsoft.EnterpriseManagement.Common.ServiceNotRunningException")
                {
                    this.OnError("CheckScomConnection", ex);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// The start.
        /// </summary>
        /// <exception cref="System.Exception">Can not find the environment variable \"FDSCOMPLUGIN\"</exception>
        /// <exception cref="Exception">ex</exception>
        private void Start()
        {
            this.OnLog("env:FDSCOMPLUGIN：" + this.InstallPath);
            if (string.IsNullOrEmpty(this.InstallPath))
            {
                throw new Exception("Can not find the environment variable \"FDSCOMPLUGIN\"");
            }

            //this.CheckAndInstallMp();
            this.RunCheckApplianceTask();


            this.RunWebServer();

            this.TcpServerTask = Task.Run(() => this.CreateTcpServer());

            this.InitialWindowEventLog();
#if !DEBUG
            this.RunPolling();
#endif
            this.RunSync(); // 先执行一次 

            this.RunCheckFdChangesTask();

            this.ImportToRoot();

            this.OnLog("Service Start Success");
        }

        /// <summary>
        /// Checks the and install mp.
        /// </summary>
        private void CheckAndInstallMp()
        {
#if DEBUG

#else
            if (!MGroup.Instance.CheckIsInstallMp("Huawei.FusionDirector.View.Library"))
            {
                string path = $"{this.InstallPath}\\MPFiles\\Huawei.FusionDirector.View.Library.mpb";
                MGroup.Instance.InstallMpb(path);
            }
            if (!MGroup.Instance.CheckIsInstallMp("Huawei.FusionDirector.Server.Library"))
            {
                string path = $"{this.InstallPath}\\MPFiles\\Huawei.FusionDirector.Server.Library.mpb";
                MGroup.Instance.InstallMpb(path);
            }
            if (!MGroup.Instance.CheckIsInstallMp("Huawei.FusionDirector.Enclosure.Library"))
            {
                string path = $"{this.InstallPath}\\MPFiles\\Huawei.FusionDirector.Enclosure.Library.mpb";
                MGroup.Instance.InstallMpb(path);
            }
#endif
        }

        /// <summary>
        /// 开始轮询
        /// </summary>
        private void RunPolling()
        {
#if DEBUG
            this.pollingTimer = new Timer(5 * 60 * 1000) { Enabled = true, AutoReset = true };
            this.pollingTimer.Elapsed += (s, e) =>
            {
                this.RunSync();
            };
            this.pollingTimer.Start();
#else
            var config = ConfigHelper.GetPluginConfig();
            this.pollingTimer = new Timer(config.PollingInterval)
            {
                Enabled = true,
                AutoReset = true,
            };
            this.pollingTimer.Elapsed += (s, e) =>
                {
                    this.RunCheckApplianceTask();
                    this.RunSync();
                };
            this.pollingTimer.Start();
#endif
            this.checkApplianceTimer = new Timer(15 * 60 * 1000)
            {
                Enabled = true,
                AutoReset = true,
            };
            this.checkApplianceTimer.Elapsed += (s, e) =>
            {
                this.RunCheckApplianceTask();
            };
            this.checkApplianceTimer.Start();
        }

        /// <summary>
        /// 执行轮询任务
        /// </summary>
        private void RunSync()
        {
            try
            {
                this.OnLog("Run Sync Task");
                var fdList = FusionDirectorDal.Instance.GetList();
                if (fdList != null)
                {
                    this.OnLog($"fdList Count :{fdList.Count}");
                    foreach (var x in fdList)
                    {
                        var instance = this.FindInstance(x);
                        instance.Sync();
                    }
                }
                else
                {
                    this.OnLog("fdList is null");
                }
            }
            catch (Exception ex)
            {
                this.OnError("RunSync Error: ", ex);
            }
        }

        private void InitialWindowEventLog()
{
            bool success = WindowEventLogHelper.CreateEventSourceIfNotExists(EVENT_SOURCE, EVENT_LOG_NAME);
            if (!success)
{
                HWLogger.Service.Error($"Could not create Window EventLog Source {EVENT_SOURCE} with {EVENT_LOG_NAME}");
                this.OnError($"Failed to create window EventLog {EVENT_LOG_NAME} with Source {EVENT_SOURCE}");
            }
            else
{
                HWLogger.Service.Info($"Create Window EventLog Source {EVENT_SOURCE} with {EVENT_LOG_NAME} successfully.");
            }
        }

        /// <summary>
        /// 启动IIS Express
        /// </summary>
        private void RunWebServer()
        {
            try
            {
                // var cmd = $"cd %{EnvVariable}%\\IISExpress&&iisexpress /config:\"%{EnvVariable}%\\Configuration\\applicationhost.config\" /site:SCOMPluginWebServer /systray:true ";
                this.OnLog("Start IISExpress");
                var iisPath = Path.GetFullPath(
                    $"{Environment.GetEnvironmentVariable("ProgramFiles")}\\IIS Express\\iisexpress.exe");
                var configFilePath = Path.GetFullPath($"{this.InstallPath}\\Configuration\\applicationhost.config");
                this.IISProcess = new Process();
                var startInfo = new ProcessStartInfo
                {
                    FileName = iisPath,
                    Arguments = $" /config:\"{configFilePath}\" /systray:false ",
                    UseShellExecute = false,// 是否使用操作系统shell启动
                    RedirectStandardInput = true, // 接受来自调用程序的输入信息
                    RedirectStandardOutput = true,// 由调用程序获取输出信息
                    RedirectStandardError = true// 重定向标准错误输出
                };

                this.IISProcess.StartInfo = startInfo;
                this.IISProcess.OutputDataReceived += (s, e) => { this.OnIISLog(e.Data); };
                this.IISProcess.ErrorDataReceived += (s, e) => { this.OnIISLog(e.Data); };
                this.IISProcess.Start();
                this.IISProcess.BeginErrorReadLine();
                this.IISProcess.BeginOutputReadLine();
                //RunCmd("netsh http delete sslcert ipport=0.0.0.0:" + this.pluginConfig.InternetPort + "");
                //RunCmd("netsh http add sslcert ipport=0.0.0.0:" + this.pluginConfig.InternetPort + " certhash=23d05922ea28365820120ed7a2f98b34dcd09222 appid={214124cd-d05b-4309-9af9-9caa44b2b74a}");
            }
            catch (Exception ex)
            {
                this.OnError("RunWebServer Error: ", ex);
            }
        }

        /// <summary>
        /// Finds the instance.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        /// <returns>FusionDirectorSyncInstance.</returns>
        private FusionDirectorSyncInstance FindInstance(FusionDirector fusionDirector)
        {
            var syncInstance = this.SyncInstances.FirstOrDefault(y => y.FusionDirectorIp == fusionDirector.HostIP);
            if (syncInstance == null)
            {
                syncInstance = new FusionDirectorSyncInstance(fusionDirector);
                this.SyncInstances.Add(syncInstance);
            }
            else
            {
                syncInstance.UpdateFusionDirector(fusionDirector);
            }
            return syncInstance;
        }

        #endregion

        #region MyRegion

        /// <summary>
        /// Runs the check fd changes task.
        /// </summary>
        public void RunCheckFdChangesTask()
        {
            this.checkFdChangesTimer = new Timer(60 * 1000) { Enabled = true, AutoReset = true };
            this.checkFdChangesTimer.Elapsed += (s, e) => { this.RunCheckFdChanges(); };
            this.checkFdChangesTimer.Start();
        }

        /// <summary>
        /// Runs the check fd changes.
        /// </summary>
        private void RunCheckFdChanges()
        {
            var fdList = FusionDirectorDal.Instance.GetList();
            HWLogger.Service.Debug($"RunCheckFdChanges:{string.Join("|", fdList.Select(x => x.Summary()))}");
            foreach (var fd in fdList)
            {
                var existFusionDirector = this.SyncInstances.FirstOrDefault(y => y.FusionDirectorIp == fd.HostIP)?.FusionDirector;
                //不存在则新增
                if (existFusionDirector == null)
                {
                    OnLog($"check: new fusion director was added.{fd.HostIP}");
                    this.RunNewFusionDirector(fd);
                }
                else
                {
                    //账号密码有变化则立即触发轮询
                    if (fd.LoginAccount != existFusionDirector.LoginAccount || fd.LoginPd != existFusionDirector.LoginPd || fd.EventUserName != existFusionDirector.EventUserName || fd.EventPd != existFusionDirector.EventPd)
                    {
                        OnLog($"check: fusion director was changed.{fd.HostIP}");
                        this.RunUpdateFusionDirector(fd);
                    }
                }
            }

            foreach (var existFusionDirector in this.SyncInstances.Select(x => x.FusionDirector))
            {
                if (fdList.All(x => x.HostIP != existFusionDirector.HostIP))
                {
                    OnLog($"check: fusion director was deleted.{existFusionDirector.HostIP}");
                    this.RunDeleteFusionDirector(existFusionDirector.HostIP);
                }
            }
        }
        #endregion

        #region Notify

        /// <summary>
        /// Creates the TCP server.
        /// </summary>
        private void CreateTcpServer()
        {
            try
            {
                int localTcpPort = this.GetPort();
                IPAddress localAddr = IPAddress.Parse("127.0" + ".0.1");
                TcpListener tcpListener = new TcpListener(localAddr, localTcpPort);
                tcpListener.Start();

                pluginConfig.TempTcpPort = localTcpPort;
                ConfigHelper.SavePluginConfig(pluginConfig);

                var bytes = new byte[256 * 256 * 16];
                while (true)
                {
                    try
                    {
                        TcpClient tcp = tcpListener.AcceptTcpClient();
                        NetworkStream stream = tcp.GetStream();
                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var json = Encoding.UTF8.GetString(bytes, 0, i);
                            Task.Run(() => { AnalysisTcpMsg(json); });
                            // Send back a response.
                            byte[] responseMsg = Encoding.UTF8.GetBytes("Received");
                            stream.Write(responseMsg, 0, responseMsg.Length);
                        }
                        stream.Close();
                        tcp.Close();
                    }
                    catch (Exception ex)
                    {
                        this.OnError(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnError(ex.ToString());
            }
        }

        /// <summary>
        /// Analysises the TCP MSG.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <exception cref="System.Exception">can not find fusionDirector:" + tcpMessage.fdIp</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public void AnalysisTcpMsg(string json)
        {
            HWLogger.NotifyRecv.Info($"Receive new TCP message: {json}");
            var tcpMessage = JsonConvert.DeserializeObject<TcpMessage<object>>(json);
            try
            {
                var list = FusionDirectorDal.Instance.GetList();
                var fusionDirector = list.FirstOrDefault(x => x.EventAuth == tcpMessage.Auth);
                if (fusionDirector == null)
                {
                    throw new Exception($"Can not find the fusion director.");
                }
                switch (tcpMessage.MsgType)
                {

                    case TcpMessageType.Alarm:
                        this.RunReciveNewAlarm(fusionDirector, json);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                HWLogger.Service.Error(e);
            }
        }


        /// <summary>
        /// Runs the recive new alarm.
        /// </summary>
        /// <param name="json">The json.</param>
        private void RunReciveNewAlarm(FusionDirector fusionDirector, string json)
        {
            var tcpMessage = JsonConvert.DeserializeObject<TcpMessage<AlarmData>>(json);
            HWLogger.GetFdNotifyLogger(fusionDirector.HostIP).Info($"Recieve new alarm: {JsonConvert.SerializeObject(tcpMessage)}");
            var syncInstance = this.SyncInstances.FirstOrDefault(y => y.FusionDirectorIp == fusionDirector.HostIP);
            if (syncInstance != null)
            {
                var data = tcpMessage.Data;
                if (data != null)
                {
                    syncInstance.DealNewAlarmAsync(data);
                }
                else
                {
                    HWLogger.GetFdNotifyLogger(fusionDirector.HostIP).Error($"Recieve new alarm：message analysis faild");
                }
            }
            else
            {
                HWLogger.GetFdNotifyLogger(fusionDirector.HostIP).Info($"Recieve new alarm：can not find the syncInstance");
            }
        }

        /// <summary>
        /// Runs to delete FusionDirector.
        /// </summary>
        /// <param name="fdIp">The e sight ip.</param>
        private void RunDeleteFusionDirector(string fdIp)
        {
            HWLogger.GetFdNotifyLogger(fdIp).Info($"Recieve: delete this FusionDirector");
            HWLogger.Service.Info($"Recieve: delete this FusionDirector {fdIp}");
            var syncInstance = this.SyncInstances.FirstOrDefault(y => y.FusionDirectorIp == fdIp);
            if (syncInstance != null)
            {
                var isFinish = false;
                int i = 0;
                while (!isFinish)
                {
                    i++;
                    if (syncInstance.IsComplete)
                    {
                        try
                        {
                            syncInstance.Close();
                            this.SyncInstances.Remove(syncInstance);
                            if (i > 1)//如果首次检查正在同步，则睡眠一分钟后删除，防止UI报错
                            {
                                Thread.Sleep(TimeSpan.FromSeconds(60));
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(2));
                            EnclosureConnector.Instance.RemoveEnclosureByFd(fdIp);
                            ServerConnector.Instance.RemoveServerByFd(fdIp);
                        }
                        catch (Exception e)
                        {
                            HWLogger.GetFdNotifyLogger(fdIp).Error(e, $"delete this FusionDirector：");
                        }
                        isFinish = true;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }
            else
            {
                HWLogger.GetFdNotifyLogger(fdIp).Info($"delete this FusionDirector：can not find the syncInstance");
            }
        }

        /// <summary>
        /// Runs when add fusion director
        /// </summary>
        /// <param name="fusionDirector">The e sight.</param>
        private void RunNewFusionDirector(FusionDirector fusionDirector)
        {
            HWLogger.GetFdNotifyLogger(fusionDirector.HostIP).Info($"Recieve: add FusionDirector");
            var instance = this.FindInstance(fusionDirector);
            instance.Sync();
        }

        /// <summary>
        /// Runs when update fusion director.
        /// </summary>
        /// <param name="fusionDirector">The fusion director.</param>
        private void RunUpdateFusionDirector(FusionDirector fusionDirector)
        {
            HWLogger.GetFdNotifyLogger(fusionDirector.HostIP).Info($"Recieve: update FusionDirector");
            var instance = this.FindInstance(fusionDirector);
            instance.Sync();
        }

        // <summary>
        /// <summary>
        /// Gets the port.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetPort()
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();
            var udpPorts = properties.GetActiveUdpListeners().Select(x => x.Port).ToList();
            var tcpPorts = properties.GetActiveTcpListeners().Select(x => x.Port).ToList();
            for (int i = 40001; i < 40500; i++)
            {
                if (!udpPorts.Contains(i) && !tcpPorts.Contains(i))
                {
                    return i;
                }
            }
            return 0;
        }

        #endregion

        #region 应用程序监控
        private Appliance LastCheckApplianceResult { get; set; }

        /// <summary>
        /// Runs the check appliance task.
        /// </summary>
        private void RunCheckApplianceTask()
        {
            Task.Run(async () =>
            {
                try
                {
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    OnLog("Start RunCheckApplianceTask");
                    var appliance = new Appliance
                    {
                        HostName = Environment.MachineName,
                        IPAddress = pluginConfig.InternetIp,
                        SoftwareVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                        EnclosureCollection = GetEnclosureCollectionHealth(),
                        ServerCollection = GetServerCollectionHealth(),
                        EventCollection = await GetEventCollectionHealthAsync(),
                        FusionDirectorCollection = GetFusionDirectorCollectionHealth(),
                    };
                    #region PerformanceCollection
                    if (appliance.EnclosureCollection.Health == Health.OK && appliance.ServerCollection.Health == Health.OK)
                    {
                        appliance.PerformanceCollection = GetPerformanceCollectionHealth();
                    }
                    else
                    {
                        appliance.PerformanceCollection = new PerformanceCollection
                        {
                            Health = Health.Critical,
                            ResourceName = "PerformanceCollection"
                        };
                    }
                    #endregion
                    //判断两次结果是否一致
                    var isSame = LastCheckApplianceResult != null && JsonConvert.SerializeObject(appliance) == JsonConvert.SerializeObject(LastCheckApplianceResult);//判断两次结果是否一致
                    if (!isSame)
                    {
                        OnLog($"Check Appliance Result:{JsonConvert.SerializeObject(appliance)}");
                        LastCheckApplianceResult = appliance;

                        ApplianceConnector.Instance.Sync(appliance, true);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        var existAlarmData = ApplianceConnector.Instance.GetUnclosedAlert();
                        CreateOrCloseEnclosureMpAlarm(existAlarmData.Any(x => x.CustomField1 == EnumAlarmType.EnclosureMpMissing.ToString()), appliance.EnclosureCollection);
                        CreateOrCloseServerMpAlarm(existAlarmData.Any(x => x.CustomField1 == EnumAlarmType.ServerMpMissing.ToString()), appliance.ServerCollection);
                        CreateOrCloseEventServiceAlarm(existAlarmData.Any(x => x.CustomField1 == EnumAlarmType.WebServerUnavailable.ToString()), appliance.EventCollection);
                        CreateOrCloseFusionDirectorAlarm(existAlarmData, appliance.FusionDirectorCollection);
                    }
                }
                catch (Exception e)
                {
                    OnError("RunCheckApplianceTask Error", e);
                }
            });
        }

        private void CreateOrCloseFusionDirectorAlarm(List<MonitoringAlert> existAlarmData, FusionDirectorCollection currentState)
        {
            var fdStatuses = JsonConvert.DeserializeObject<Dictionary<string, string>>(currentState.ErrorMsg);
            foreach (var fdStatus in fdStatuses)
            {
                var fdIp = fdStatus.Key;
                var error = fdStatus.Value;
                bool isHaveOldAlarm = existAlarmData.Any(x => x.CustomField1 == EnumAlarmType.FdConnectError.ToString() && x.CustomField4.Contains(fdIp));
                if (fdStatus.Value != "OK")//本次有告警，则插入或更新告警
                {
                    OnLog($"[{fdIp}] Insert Or Update Event:FusionDirector connect Error");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "1",
                        AlarmName = "FusionDirector Connect Error",
                        AlarmType = EnumAlarmType.FdConnectError,
                        PossibleCause = $"{error}",
                        Suggstion = "check whether the FusionDirector server is shut down or whether the network is abnormal.",
                        Additional = error
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent, fdIp);
                }
                else
                {
                    //本次没有告警 且有旧的告警，则关闭
                    if (isHaveOldAlarm)
                    {
                        OnLog($"[{fdIp}] Close Event:FusionDirector Connect Error");
                        var alarm = new ApplianceAlarm
                        {
                            OptType = "2",
                            AlarmType = EnumAlarmType.FdConnectError,
                        };
                        var applianceEvent = new ApplianceEvent(alarm);
                        ApplianceConnector.Instance.InsertEvent(applianceEvent, fdIp);
                    }
                }
            }

        }

        private void CreateOrCloseEnclosureMpAlarm(bool isHaveOldAlarm, EnclosureCollection currentState)
        {
            if (currentState.Health != Health.OK)
            {
                if (!isHaveOldAlarm)
                {
                    OnLog("Insert Event:Enclosure ManagementPack Missing");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "1",
                        AlarmName = "Enclosure ManagementPack Missing",
                        AlarmType = EnumAlarmType.EnclosureMpMissing,
                        PossibleCause = "The Management Pack of the plug-in is uninstalled.",
                        Suggstion = "import the Huawei.FusionDirector.Enclosure.Library.mpb",
                        Additional = currentState.ErrorMsg
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent);
                }
            }
            else
            {
                if (isHaveOldAlarm)
                {
                    OnLog("Close Event:Enclosure ManagementPack Missing");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "2",
                        AlarmType = EnumAlarmType.EnclosureMpMissing,
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent);
                }
            }
        }

        private void CreateOrCloseServerMpAlarm(bool isHaveOldAlarm, ServerCollection currentState)
        {
            if (currentState.Health != Health.OK)
            {
                if (!isHaveOldAlarm)
                {
                    OnLog("Insert Event:Server ManagementPack Missing");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "1",
                        AlarmName = "Server ManagementPack Missing",
                        AlarmType = EnumAlarmType.ServerMpMissing,
                        PossibleCause = "The Management Pack of the plug-in is uninstalled.",
                        Suggstion = "import the Huawei.FusionDirector.Server.Library.mpb",
                        Additional = currentState.ErrorMsg
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent);
                }
            }
            else
            {
                if (isHaveOldAlarm)
                {
                    OnLog("Close Event:Server ManagementPack Missing");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "2",
                        AlarmType = EnumAlarmType.ServerMpMissing,
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent);
                }
            }
        }

        private void CreateOrCloseEventServiceAlarm(bool isHaveOldAlarm, EventCollection currentState)
        {
            if (currentState.Health != Health.OK)
            {
                if (!isHaveOldAlarm)
                {
                    OnLog("Insert Event:Webserver unavailable");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "1",
                        AlarmName = "Webserver unavailable ",
                        AlarmType = EnumAlarmType.WebServerUnavailable,
                        PossibleCause = "IIS express service is stopped or uninstalled.",
                        Suggstion = "Check the website",
                        Additional = currentState.ErrorMsg
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent);
                }
            }
            else
            {
                if (isHaveOldAlarm)
                {
                    OnLog("Close Event:Webserver unavailable");
                    var alarm = new ApplianceAlarm
                    {
                        OptType = "2",
                        AlarmType = EnumAlarmType.WebServerUnavailable,
                    };
                    var applianceEvent = new ApplianceEvent(alarm);
                    ApplianceConnector.Instance.InsertEvent(applianceEvent);
                }
            }
        }

        /// <summary>
        /// Gets the enclosure collection.
        /// </summary>
        /// <returns>EnclosureCollection.</returns>
        private EnclosureCollection GetEnclosureCollectionHealth()
        {
            var isInstall = MGroup.Instance.CheckIsInstallMp("Huawei.FusionDirector.Enclosure.Library");
            return new EnclosureCollection
            {
                Health = isInstall ? Health.OK : Health.Critical,
                ResourceName = "EnclosureCollection",
                ErrorMsg = isInstall ? "OK" : "Can not find the Huawei.FusionDirector.Enclosure.Library management pack.",
            };
        }

        /// <summary>
        /// Gets the server collection.
        /// </summary>
        /// <returns>ServerCollection.</returns>
        private ServerCollection GetServerCollectionHealth()
        {
            var isInstall = MGroup.Instance.CheckIsInstallMp("Huawei.FusionDirector.Server.Library");
            return new ServerCollection
            {
                Health = isInstall ? Health.OK : Health.Critical,
                ResourceName = "ServerCollection",
                ErrorMsg = isInstall ? "OK" : "Can not find the Huawei.FusionDirector.Server.Library management pack.",
            };
        }

        /// <summary>
        /// Gets the event collection.
        /// </summary>
        /// <returns>EventCollection.</returns>
        private async Task<EventCollection> GetEventCollectionHealthAsync()
        {
            var isInstall = CheckIISIsInstall();
            if (!isInstall)
            {
                return new EventCollection
                {
                    Health = Health.Critical,
                    ResourceName = "EventCollection",
                    ErrorMsg = "IIS Express is not installed."
                };
            }
            var webServerAvailable = await CheckWebserverAvailableAsync();
            if (!webServerAvailable)
            {
                return new EventCollection
                {
                    Health = Health.Critical,
                    ResourceName = "EventCollection",
                    ErrorMsg = "WebServer is unavailable."
                };
            }
            return new EventCollection
            {
                Health = Health.OK,
                ResourceName = "EventCollection",
                ErrorMsg = "OK"
            };
        }

        /// <summary>
        /// Gets the performance collection.
        /// </summary>
        /// <returns>PerformanceCollection.</returns>
        private PerformanceCollection GetPerformanceCollectionHealth()
        {
            return new PerformanceCollection
            {
                Health = Health.OK,
                ResourceName = "PerformanceCollection"
            };
        }

        /// <summary>
        /// Gets the FusionDirector collection Health.
        /// </summary>
        /// <returns></returns>
        private FusionDirectorCollection GetFusionDirectorCollectionHealth()
        {
            var fdList = FusionDirectorDal.Instance.GetList();
            var dic = new Dictionary<string, string>();
            var errorMsgs = new List<string>();
            var isHasError = false;
            foreach (var x in fdList)
            {
                var instance = this.FindInstance(x);
                var result = instance.CheckFd();
                if (!result.Success)
                {
                    dic.Add(x.HostIP, $"FusionDirector Server({x.HostIP}) connect faild ({result.Msg}).");
                    isHasError = true;
                }
                else
                {
                    dic.Add(x.HostIP, "OK");
                }
            }
            return new FusionDirectorCollection
            {
                Health = isHasError ? Health.Critical : Health.OK,
                ResourceName = "FusionDirectorCollection",
                ErrorMsg = JsonConvert.SerializeObject(dic)
            };
        }

        /// <summary>
        /// Checks the IIS is install.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CheckIISIsInstall()
        {
            var path = Path.GetFullPath(
                $"{Environment.GetEnvironmentVariable("ProgramFiles")}\\IIS Express\\iisexpress.exe");
            return File.Exists(path);
        }

        /// <summary>
        /// Checks the webserver availble.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private async Task<bool> CheckWebserverAvailableAsync()
        {
            try
            {
                var url = $"https://{pluginConfig.InternetIp}:{pluginConfig.InternetPort}/AlarmReciver.ashx";
                var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
                var res = httpClient.GetAsync(url).Result;
                var success = res.StatusCode == HttpStatusCode.OK;
                if (!success) {
                    var content = await res.Content.ReadAsStringAsync();
                    this.OnError($"IIS express index page is not correct responding, status code: {res.StatusCode}, response content: {content}");
                    res.EnsureSuccessStatusCode();
                }
                return res.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                this.OnError("IIS express index page is not correct responding", e);
                return false;
            }
        }

        #endregion

        #region Import Self Cert
        /// <summary>
        /// Gets the self cert.
        /// </summary>
        /// <returns>System.Security.Cryptography.X509Certificates.X509Certificate2.</returns>
        private X509Certificate2 GetSelfCert()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            foreach (var cert in store.Certificates)
            {
                if (cert.Subject == "CN=localhost")
                {
                    return cert;
                }
            }
            store.Close();
            return null;
        }

        /// <summary>
        /// Determines whether [is exsit cert].
        /// </summary>
        /// <returns>System.Boolean.</returns>
        private bool IsNeedImport()
        {
            var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            foreach (var cert in store.Certificates)
            {
                if (cert.Subject == "CN=localhost")
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Imports to root.
        /// </summary>
        /// <exception cref="System.Exception">can not fin Self Signed Certificate</exception>
        private void ImportToRoot()
        {
            if (this.IsNeedImport())
            {
                this.OnLog("Start import the self signed certificate");
                var self = this.GetSelfCert();
                if (self == null)
                {
                    throw new Exception("can not fin Self Signed Certificate");
                }
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                store.Add(self);
                store.Close();
            }
            else
            {
                this.OnLog("Do not need import the self signed certificate");
            }
        }
        #endregion

        #region Utils

        /// <summary>
        /// Trusts the certificate.
        /// </summary>
        public void SetCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return sslPolicyErrors == SslPolicyErrors.None || sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch;
            };
            //兼容所有ssl协议
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(MySecurityProtocolType.Tls12 | MySecurityProtocolType.Tls11 | MySecurityProtocolType.Tls | MySecurityProtocolType.Ssl3);
            ServicePointManager.DefaultConnectionLimit = 1000;
        }

        /// <summary>
        /// The on error.
        /// </summary>
        /// <param name="msg">The msg.</param>
        /// <param name="ex">The ex.</param>
        private void OnError(string msg, Exception ex = null)
        {
            HWLogger.Service.Error(ex, msg);
        }

        /// <summary>
        /// The on log.
        /// </summary>
        /// <param name="msg">The msg.</param>
        private void OnLog(string msg)
        {
            HWLogger.Service.Info(msg);
        }

        /// <summary>
        /// Called when [IIS log].
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void OnIISLog(string msg)
        {
            // IIS请求的日志
            if (msg != null)
            {
                if (!msg.StartsWith("Request"))
                {
                    HWLogger.Service.Info("IIS-" + msg);
                }
            }
        }

        #endregion

        public void RunCmd(string cmd)
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