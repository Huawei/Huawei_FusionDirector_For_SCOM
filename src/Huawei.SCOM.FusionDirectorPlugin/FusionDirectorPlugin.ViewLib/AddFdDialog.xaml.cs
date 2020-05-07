//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using FusionDirectorPlugin.ViewLib.Model;
using FusionDirectorPlugin.ViewLib.Repo;
using FusionDirectorPlugin.ViewLib.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static FusionDirectorPlugin.ViewLib.Model.Constants;

namespace FusionDirectorPlugin.ViewLib
{
    /// <summary>
    /// AddFdDialog.xaml 的交互逻辑
    /// </summary>
    public partial class AddFdDialog : Window, INotifyPropertyChanged
    {
        private FdApplianceRepo FdApplianceRepo { get; }
        private FdAppliance _item = new FdAppliance();
        private Result _actionResult = Result.Done();

        public FdAppliance Item
        {
            get { return _item; }
            set
            {
                if (_item != value)
                {
                    _item = value;
                    this.OnPropertyChanged("ActionResult");
                }
            }
        }
        public Result ActionResult
        {
            get { return _actionResult; }

            set
            {

                if (_actionResult != value)
                {
                    _actionResult = value;
                    this.OnPropertyChanged("ActionResult");
                }
            }
        }

        public AddFdDialog(FdApplianceRepo repo)
        {
            InitializeComponent();
            this.FdApplianceRepo = repo;
            this.ShowInTaskbar = false;
            this.DataContext = this;
        }

        public void SetEventUserName(string name)
        {
            txtEventAccount.Text = name;
        }

        private async void OnSaveBtnClicked(object sender, RoutedEventArgs e)
        {
            this.btnSave.IsEnabled = false;
            this.btnSave.Content = "Saving";
            this.ActionResult = Result.Done();
            FdAppliance appliance = new FdAppliance
            {
                HostIP = txtHost.Text,
                Port = txtPort.Text,
                AliasName = txtAlias.Text,
                LoginAccount = txtLoginAccount.Text,
                LoginPd = txtPassword.Password,
                EventUserName = txtEventAccount.Text,//使用后台生成的eventUserName
                EventPd = txtEventPassword.Password,
                SubscribeStatus = FdSubscriptionStatus.NotSubscribed,
                LatestSubscribeInfo = String.Empty,
                CreateTime = DateTime.Now,
                LastModifyTime = DateTime.Now
            };
            LogHelper.Info($"Add FusionDirector:{appliance.Summary()})");

            this.ActionResult = await FdApplianceRepo.Add(appliance);
            this.btnSave.IsEnabled = true;
            this.btnSave.Content = "Save";
            if (this.ActionResult.Success)
            {
                LogHelper.Info($"Add FusionDirector ({appliance.HostIP}) Success.");
                this.Close();
            }
            else
            {
                LogHelper.Error(this.ActionResult.Cause, $"Add FusionDirector ({appliance.HostIP}) Faild:" + this.ActionResult.Message);
            }
        }


        private async void OnTestBtnClicked(object sender, RoutedEventArgs e)
        {
            this.btnTest.IsEnabled = false;
            this.btnTest.Content = "Testing";
            this.ActionResult = Result.Done();
            FdAppliance appliance = new FdAppliance
            {
                HostIP = txtHost.Text,
                Port = txtPort.Text,
                LoginAccount = txtLoginAccount.Text,
                LoginPd = txtPassword.Password,
                EventUserName = txtEventAccount.Text,
                EventPd = txtEventPassword.Password
            };

            this.ActionResult = await FdApplianceRepo.Test(appliance, false, false);
            this.btnTest.IsEnabled = true;
            this.btnTest.Content = "Test";
            if (this.ActionResult.Success)
            {
                LogHelper.Info($"Test FusionDirector ({appliance.HostIP}) Success.");
            }
            else
            {
                LogHelper.Error(this.ActionResult.Cause, $"Test FusionDirector ({appliance.HostIP}) Faild:" + this.ActionResult.Message);
            }
        }


        private void OnCloseBtnClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
        }

        private void OnCancelBtnClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #region NotifyPropertyChanged 
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion //NotifyPropertyChanged 
    }
}
