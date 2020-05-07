//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.ComponentModel.Composition;
using FusionDirectorPlugin.ViewLib.Repo;
using FusionDirectorPlugin.ViewLib.Model;
using FusionDirectorPlugin.ViewLib.Utils;
using System.Windows.Threading;

namespace FusionDirectorPlugin.ViewLib
{

    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]

    public partial class FdConfigDashboard : UserControl, INotifyPropertyChanged
    {
        private Result _actionResult = Result.Done();
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

        public FdApplianceRepo FdApplianceRepo { get; set; }


        public FdConfigDashboard()
        {
            InitializeComponent();
            add_btn.IsEnabled = false;
            this.FdApplianceRepo = this.Resources["FdApplianceRepo"] as FdApplianceRepo;
            this.Loaded += FdConfigDashboard_Loaded;
            this.dispatcherTimer_Tick(null, null);//首先触发一次
        }

        private void FdConfigDashboard_Loaded(object sender, RoutedEventArgs e)
        {
            // Start dispatcher timer
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();
        }

        //Refreshes grid data on timer tick
        protected void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var result = await this.FdApplianceRepo.LoadAll();
                if (result.Success)
                {
                    LogHelper.Info("Load FusionDirector list success!");
                }
                else
                {
                    LogHelper.Error("Load FusionDirector list failed! errorInfo:{0}", new Exception(result.Message));
                }
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() =>
                {
                    this.ActionResult = result;
                    add_btn.IsEnabled = true;
                }));
            });
        }

        public void OnSearchFd()
        {
            string keyword = txtSearchKeyword.Text;
            this.FdApplianceRepo.LoadAll(keyword);
        }


        private async void OnDeleteFd(object sender, RoutedEventArgs e)
        {
            int selectedIndex = Grid.SelectedIndex;
            if (Grid.SelectedIndex > -1 && selectedIndex < this.FdApplianceRepo.FilteredItems.Count)
            {
                FdAppliance appliance = this.FdApplianceRepo.FilteredItems[Grid.SelectedIndex];
                MessageBoxResult confirmResult = MessageBox.Show("Are you sure you want to delete the FusionDirector?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirmResult == MessageBoxResult.Yes)
                {
                    this.ActionResult = await this.FdApplianceRepo.Delete(appliance);
                    if (!this.ActionResult.Success)
                    {
                        LogHelper.Error(this.ActionResult.Cause, $"Delete FusionDirector ({appliance.HostIP}) Faild:" + this.ActionResult.Message);
                        MessageBox.Show(this.ActionResult.Message);
                    }
                    else
                    {
                        LogHelper.Info($"Delete FusionDirector ({appliance.HostIP}) Success.");
                    }
                }
            }
        }

        private void ShowEditFdDialog(object sender, RoutedEventArgs e)
        {
            if (Grid.SelectedIndex > -1 && Grid.SelectedIndex < this.FdApplianceRepo.FilteredItems.Count)
            {
                this.Effect = new BlurEffect();
                EditFdDialog dialog = new EditFdDialog(this.FdApplianceRepo);

                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.SetItem(this.FdApplianceRepo.FilteredItems[Grid.SelectedIndex]);
                dialog.ShowDialog();
                this.Effect = null;
            }
        }

        private async void ShowAddFdDialog(object sender, RoutedEventArgs e)
        {
            this.Effect = new BlurEffect();
            AddFdDialog dialog = new AddFdDialog(this.FdApplianceRepo);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var eventUserName = await GetEventUserName();
            dialog.SetEventUserName(eventUserName);
            dialog.ShowDialog();
            this.Effect = null;
        }

        private async Task<string> GetEventUserName()
        {
            try
            {
                var result = await this.FdApplianceRepo.LoadAll();
                int index = GetIndex(result.Data);
                if (index == -1)
                {
                    this.ActionResult = Result.Failed(-1, "No eventUserName can be used.");
                    return "";
                }
                return $"SCOMPlugin{index}";
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex, "GetEventUserName failed: ");
                this.ActionResult = Result.Failed(-1, "GetEventUserName failed");
                return "";
            }
        }

        private int GetIndex(List<FdAppliance> list)
        {
            var indexs = list.Where(x => !string.IsNullOrEmpty(x.EventUserName))
                .Select(x => Convert.ToInt32(x.EventUserName.Replace("SCOMPlugin", "")))
                .ToList();
            for (int i = 1; i < 11; i++)
            {
                if (!indexs.Contains(i))
                {
                    return i;
                }
            }

            return -1;
        }
        public void OnEditFd(string host, string alias, string port, string systemId, string account, string password)
        {
            if (Grid.SelectedIndex > -1 && Grid.SelectedIndex < this.FdApplianceRepo.FilteredItems.Count)
            {
                // Items[grid.SelectedIndex] = new ServerData(host, alias, port, systemId, account, password);
                OnSearchFd();
            }
        }

        public void OnAddFd(string host, string alias, string port, string systemId, string account, string password)
        {
            // Items.Add(new ServerData(host, alias, port, systemId, account, password));
            OnSearchFd();
        }


        private void OnSearchKeywordDataContextChanged(object Sender, DependencyPropertyChangedEventArgs e)
        {
            OnSearchFd();
        }

        private void OnSearchKeywordChanged(object sender, TextChangedEventArgs e)
        {
            OnSearchFd();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
