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
// Created          : 05-07-2019
//
// Last Modified By : mike
// Last Modified On : 05-07-2019
// ***********************************************************************
// <copyright file="FdApplianceRepo.cs" company="mike">
//     Copyright ©  2019
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using FusionDirectorPlugin.ViewLib.Client;
using FusionDirectorPlugin.ViewLib.Model;
using FusionDirectorPlugin.ViewLib.OM12R2;
using FusionDirectorPlugin.ViewLib.Utils;
using Microsoft.EnterpriseManagement.Common;
using Result = FusionDirectorPlugin.ViewLib.Model.Result;
using System.Threading.Tasks;
using System.Windows.Threading;
// ReSharper disable StyleCop.SA1600

namespace FusionDirectorPlugin.ViewLib.Repo
{

    public class FdApplianceRepo : INotifyPropertyChanged
    {


        #region Public Members

        public List<FdAppliance> AllItems { get; set; } = new List<FdAppliance>();

        public ObservableCollection<FdAppliance> FilteredItems { get; set; } = new ObservableCollection<FdAppliance>();

        #endregion //Public Members

        #region Load Appliance List

        public async Task<Result<List<FdAppliance>>> LoadAll()
        {
            try
            {
                var getListResult = await FdApplianceConnector.Instance.All();
                if (!getListResult.Success)
                {
                    LogHelper.Error(getListResult.Cause, getListResult.Message);
                    return Result<List<FdAppliance>>.Failed(getListResult.Code, getListResult.Message, getListResult.Cause);
                }
                this.AllItems = getListResult.Data.Select(x => GetModelFromMpObject(x)).ToList();

                OnFilteredItemsChanged(this.AllItems);

                foreach (var appliance in this.AllItems)
                {
                    if (PingFd(appliance.HostIP))
                    {
                        appliance.LatestConnectInfo = "success";
                        appliance.LatestStatus = Constants.FdConnectionStatus.ONLINE;
                    }
                    else
                    {
                        appliance.DirectorVersion = string.Empty;
                        appliance.LatestConnectInfo = "Can not connect the remote server.";
                        appliance.LatestStatus = Constants.FdConnectionStatus.FAILED;
                        LogHelper.Info("Can not connect the remote server.", $"PingFd Error:");
                    }
                    try
                    {
                        using (var client = new FdClient(appliance))
                        {
                            var result = await client.GetApplianceVersion();
                            appliance.DirectorVersion = result.CurrentVersion;
                        }
                    }
                    catch (Exception e)
                    {
                        appliance.DirectorVersion = string.Empty;
                        LogHelper.Error(e, $"GetApplianceVersion Error:");
                    }
                }

                OnFilteredItemsChanged(this.AllItems);
                return Result<List<FdAppliance>>.Done(this.AllItems);

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "LoadAll");
                return Result<List<FdAppliance>>.Failed("LoadAll", ex);
            }
        }

        public void LoadAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                var list = this.AllItems.Where(x => x.HostIP.Contains(keyword) || x.AliasName.Contains(keyword)).ToList();
                OnFilteredItemsChanged(list);
            }
        }

        public void OnFilteredItemsChanged(List<FdAppliance> data)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() =>
            {
                FilteredItems.Clear();
                foreach (var item in data)
                {
                    FilteredItems.Add(item);
                }
                OnPropertyChanged("FilteredItems");
            }));

        }

        /// <summary>
        /// Gets the model from mp object.
        /// </summary>
        /// <param name="managementObject">The management object.</param>
        /// <returns>FdAppliance.</returns>
        private FdAppliance GetModelFromMpObject(EnterpriseManagementObject managementObject)
        {
            var props = FdApplianceConnector.Instance.FdApplianceClass.PropertyCollection;
            var model = new FdAppliance();
            model.HostIP = managementObject[props["HostIP"]].Value.ToString();
            model.AliasName = managementObject[props["AliasName"]].Value.ToString();
            model.LoginAccount = managementObject[props["LoginAccount"]].Value.ToString();
            model.LoginPd = RijndaelManagedCrypto.Instance.DecryptFromCs(managementObject[props["LoginPd"]].Value.ToString());
            model.Port = managementObject[props["Port"]].Value.ToString();
            model.EventUserName = managementObject[props["EventUserName"]].Value.ToString();
            model.EventPd = RijndaelManagedCrypto.Instance.DecryptFromCs(managementObject[props["EventPd"]].Value.ToString());
            model.SubscribeId = managementObject[props["SubscribeId"]].Value.ToString();
            model.SubscribeStatus = managementObject[props["SubscribeStatus"]].Value.ToString();
            model.LatestSubscribeInfo = managementObject[props["LatestSubscribeInfo"]].Value.ToString();
            model.LastModifyTime = Convert.ToDateTime(managementObject[props["LastModifyTime"]].Value.ToString());
            model.CreateTime = Convert.ToDateTime(managementObject[props["CreateTime"]].Value.ToString());

            return model;
        }

        #endregion 

        #region 获取版本号

        private bool PingFd(string nameOrAddress)
        {
            var pingable = false;
            Ping pinger = null;
            try
            {
                pinger = new Ping();
                var reply = pinger.Send(nameOrAddress, 1000);
                if (reply != null)
                {
                    pingable = reply.Status == IPStatus.Success;
                }
            }
            catch (Exception)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                pinger?.Dispose();
            }
            return pingable;
        }
        #endregion

        #region Test Appliance 
        internal async Task<Result> Test(FdAppliance appliance, bool isUpdate, bool isUpdateCredential)
        {
            try
            {
                var validateResult = Validate(appliance, isUpdate, isUpdateCredential);
                if (!validateResult.Success)
                {
                    return validateResult;
                }
                using (var client = new FdClient(appliance))
                {
                    return await client.TestCredential();   
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Test");
                return Result.Failed(100, "Test", ex);
            }
        }

        #endregion //Test Appliance 

        #region Add Appliance 

        internal async Task<Result> Add(FdAppliance appliance)
        {
            try
            {
                var validateResult = Validate(appliance, false, false);
                if (!validateResult.Success)
                {
                    return validateResult;
                }
                using (var client = new FdClient(appliance))
                {
                    var result = await client.TestCredential();
                    if (!result.Success)
                    {
                        return result;
                    }

                    var addResult = await FdApplianceConnector.Instance.Add(appliance);
                    if (addResult.Success)
                    {
                        await this.LoadAll();
                    }
                    return addResult;
                }

            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Add");
                return Result.Failed(100, "Add", ex);
            }

        }
        #endregion //Add Appliance 

        #region Update Appliance 

        public async Task<Result> Update(FdAppliance appliance, bool isUpdateCredential)
        {
            try
            {
                var validateResult = Validate(appliance, true, isUpdateCredential);
                if (!validateResult.Success)
                {
                    return validateResult;
                }

                if (isUpdateCredential)//修改了密码
                {
                    using (var client = new FdClient(appliance))
                    {
                        var result = await client.TestCredential();
                        if (!result.Success)
                        {
                            return result;
                        }
                    }
                }
                else
                {
                    var oldFdObj = await FdApplianceConnector.Instance.FindByHost(appliance.HostIP);
                    if (oldFdObj.Data == null)
                    {
                        return Result.Failed(104, $"FusionDirector {appliance.HostIP} can not find.");
                    }
                    var oldFd = GetModelFromMpObject(oldFdObj.Data);
                    if (oldFd.Port != appliance.Port) //修改了端口
                    {
                        appliance.LoginPd = oldFd.LoginPd;
                        using (var client = new FdClient(appliance))
                        {
                            var result = await client.TestCredential();
                            if (!result.Success)
                            {
                                return result;
                            }
                        }
                    }
                }

                var updateResult = await FdApplianceConnector.Instance.Update(appliance, isUpdateCredential);
                if (updateResult.Success)
                {
                    await this.LoadAll();
                }
                return updateResult;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Update");
                return Result.Failed(100, "Update", ex);
            }
        }

        #endregion //Update Appliance 

        #region Delete Appliance 
        public async Task<Result> Delete(FdAppliance appliance)
        {
            try
            {
                var deleteResult = await FdApplianceConnector.Instance.Delete(appliance);
                if (deleteResult.Success)
                {
                    await this.LoadAll();
                }
                return deleteResult;
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "Test");
                return Result.Failed(100, "Test", ex);
            }
        }
        #endregion //Delete Appliance 

        #region Validate Appliance 
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="appliance">appliance</param>
        /// <param name="isUpdate">是否是更新操作</param>
        /// <param name="isUpdateCredential">是否修改了密码</param>
        /// <returns></returns>
        public Result Validate(FdAppliance appliance, bool isUpdate, bool isUpdateCredential)
        {
            #region HostIP
            var hostIp = appliance.HostIP;
            if (string.IsNullOrEmpty(hostIp))
            {
                return Result.Failed(1000, "The FQDN or IP Address is required");
            }
            IPAddress Address = null;
            var isValidIPAddr = IPAddress.TryParse(hostIp, out Address);
            if (!isValidIPAddr)//如果不是IP
            {
                if (Regex.IsMatch(hostIp.Replace(".", ""), @"^[0-9]*$")) //如果是纯数字，说明不是域名
                {
                    return Result.Failed(1000, "Invalid FQDN or IP Address.");
                }
                else
                {
                    if (Uri.CheckHostName(hostIp) != UriHostNameType.Dns || !hostIp.Contains(".") || hostIp.EndsWith(".") || hostIp.StartsWith("."))
                    {
                        return Result.Failed(1000, "Invalid FQDN or IP Address.");
                    }
                }
            }
            #endregion

            #region Port
            if (string.IsNullOrEmpty(appliance.Port))
            {
                return Result.Failed(1001, "The port is required");
            }

            int PortAsInt;
            var isNumeric = int.TryParse(appliance.Port, out PortAsInt);
            if (isNumeric)
            {
                if (PortAsInt < 1 || PortAsInt > 65535)
                {
                    return Result.Failed(1001, "The port must between 0 and 65535");
                }

            }
            else
            {
                return Result.Failed(1001, "The port must between 0 and 65535");
            }
            #endregion

            if (isUpdateCredential || !isUpdate)
            {
                // Login Account
                if (string.IsNullOrEmpty(appliance.LoginAccount))
                {
                    return Result.Failed(1001, "The LoginAccount is required");
                }
                if (!Regex.IsMatch(appliance.LoginAccount, @"^[a-zA-Z0-9_\-\.]{1,100}$"))
                {
                    return Result.Failed(1001, "The loginAccount contains 1 to 100 characters, which can include letters, digits, hyphens (-), underscores (_), and periods (.).");
                }

                // Login Password
                if (string.IsNullOrEmpty(appliance.LoginPd))
                {
                    return Result.Failed(1001, "The password is required");
                }

                // Event Password
                var regexEventPwd = new Regex(@"
(?=.*[0-9])                              #必须包含数字
(?=.*[a-z])                              #必须包含小写字母
(?=.*[A-Z])                              #必须包含大写字母
(?=([\x21-\x7e]+)[^a-zA-Z0-9])           #必须包含特殊符号
.{8,32}                                  #至少8个字符，最多32个字符
", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

                if (string.IsNullOrEmpty(appliance.EventPd))
                {
                    return Result.Failed(1001, "The event password is required");
                }
                if (appliance.EventPd.Length > 32)
                {
                    return Result.Failed(1001, @"The event password must contain 8 to 32 characters.");
                }
                if (!regexEventPwd.IsMatch(appliance.EventPd) || appliance.EventPd.Contains("#"))
                {
                    return Result.Failed(1001, @"The event password must contain 8 to 32 characters, and include: uppercase letters, lowercase letters, digits, and special characters including `~!@$%\^&*()_+-={}[]|;:"";'<,>.?");
                }
            }

            return Result.Done();
        }
        #endregion //Validate Credential 

        #region NotifyPropertyChanged 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //NotifyPropertyChanged 
    }
}
