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

namespace FusionDirectorPlugin.ViewLib
{
    /// <summary>
    /// EditFdDialog.xaml 的交互逻辑
    /// </summary>
    public partial class EditFdDialog : Window, INotifyPropertyChanged
    {
        private FdApplianceRepo FdApplianceRepo { get; set; }

        private bool _updateCredentialChecked = false;
        private FdAppliance _item = new FdAppliance();
        private Result _actionResult = Result.Done();

        public bool UpdateCredentialChecked
        {
            get
            {
                return this._updateCredentialChecked;
            }
            set
            {
                if (value != this._updateCredentialChecked)
                {
                    this._updateCredentialChecked = value;
                    this.OnPropertyChanged("UpdateCredentialChecked");
                }
            }
        }

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


        public EditFdDialog(FdApplianceRepo repo)
        {
            InitializeComponent();
            this.FdApplianceRepo = repo;
            this.DataContext = this;
            this.ShowInTaskbar = false;
        }

        public void SetItem(FdAppliance item)
        {
            this.Item = item;
            // using binding instead?
            txtHost.Text = item.HostIP;
            txtAlias.Text = item.AliasName;
            txtPort.Text = item.Port;
            txtAccount.Text = item.LoginAccount;
            txtEventAccount.Text = item.EventUserName;
            // txtPassword.Password = item.LoginPd;
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
                LoginAccount = txtAccount.Text,
                LoginPd = txtPassword.Password,
                EventUserName = txtEventAccount.Text,
                LastModifyTime = DateTime.Now,
                EventPd = txtEventPassword.Password
            };
            LogHelper.Info($"Update FusionDirector:{appliance.Summary()})");

            this.ActionResult = await FdApplianceRepo.Update(appliance, UpdateCredentialChecked);
            this.btnSave.IsEnabled = true;
            this.btnSave.Content = "Save";
            if (this.ActionResult.Success)
            {
                LogHelper.Info($"Update FusionDirector ({appliance.HostIP}) Success.");
                this.Close();
            }
            else
            {
                LogHelper.Error(this.ActionResult.Cause, $"Update FusionDirector ({appliance.HostIP}) Faild:" + this.ActionResult.Message);
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
                AliasName = txtAlias.Text,
                LoginAccount = txtAccount.Text,
                LoginPd = txtPassword.Password,
                EventUserName = txtEventAccount.Text,
                EventPd = txtEventPassword.Password,
            };

            this.ActionResult = await FdApplianceRepo.Test(appliance, true, UpdateCredentialChecked);
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

        void TestFdCallback(Result result)
        {
            this.ActionResult = result;
        }

        private void OnCloseBtnClicked(object sender, RoutedEventArgs e)
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
