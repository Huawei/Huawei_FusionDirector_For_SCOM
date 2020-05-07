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
// Assembly         : FusionDirectorPlugin.Model
// Author           : panwei
// Created          : 12-26-2018
//
// Last Modified By : mike
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Server.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// Class ServerInfo.
    /// </summary>
    public class Server
    {
        public Server()
        {
            this.ODataId = string.Empty;
            this.UUID = string.Empty;
            this.HostName = string.Empty;
            this.Model = string.Empty;
            this.BMCVersion = string.Empty;
            this.BiosVersion = string.Empty;
            this.PowerState = string.Empty;
            this.SerialNumber = string.Empty;
            this.ServerState = string.Empty;
            this.StateReason = string.Empty;
            this.IPv4Address = new IPv4Address();
            this.Tag = string.Empty;
            this.AssetTag = string.Empty;
            this.Status = new Status();
            this.ProcessorSummary = new List<ProcessorSummary>();
            this.MemorySummary = new MemorySummary();
            this.StorageSummary = new StorageSummary();
        }

        public Server(ServerSummary serverSummary)
        {
            this.ODataId = serverSummary.ODataId;
            this.UUID = serverSummary.UUID;
            this.HostName = string.Empty;
            this.Model = serverSummary.Model;
            this.BMCVersion = string.Empty;
            this.BiosVersion = string.Empty;
            this.PowerState = serverSummary.PowerState;
            this.SerialNumber = serverSummary.SerialNumber;
            this.ServerState = serverSummary.State;
            this.StateReason = string.Empty;
            this.IPv4Address = serverSummary.IPv4Address;
            this.Tag = serverSummary.Tag;
            this.AssetTag = string.Empty;
            this.Status = serverSummary.Status;

            this.MemorySummary = serverSummary.MemorySummary;
            this.StorageSummary = serverSummary.StorageSummary;
            this.ProcessorSummary = new List<ProcessorSummary>() { serverSummary.ProcessorSummary };

            //this.DeviceID = serverSummary.DeviceID;
            // this.Name = serverSummary.Name;
            // this.Alias = serverSummary.Alias;
            // this.Group = serverSummary.Group;
            // this.Manufacturer = string.Empty;
            // this.Slot = string.Empty;
        }

        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.Id}";

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// Node资源模型的OData描述信息。
        /// </summary>
        /// <value>The odatacontext.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// 指定Node资源节点的访问路径。
        /// </summary>
        /// <value>The odataid.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 指定Node资源类型。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id => ODataId.Split('/').Last();

        /// <summary>
        /// Node的UUID。
        /// </summary>
        /// <value>The UUID.</value>
        [JsonProperty("UUID")]
        public string UUID { get; set; }

        /// <summary>
        /// nodes/{id}/Manager接口获取
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Node的型号。
        /// </summary>
        /// <value>The model.</value>
        [JsonProperty("Model")]
        public string Model { get; set; }

        /// <summary>
        /// The BMC version
        /// 调用Nodes/id/Manager接口查询 FirmwareVersion
        /// </summary>
        public string BMCVersion { get; set; }

        /// <summary>
        /// 指定Node资源BIOS版本号。
        /// </summary>
        /// <value>The bios version.</value>
        [JsonProperty("BiosVersion")]
        public string BiosVersion { get; set; }

        public int TotalSystemMemoryGiB => MemorySummary?.TotalSystemMemoryGiB ?? 0;

        public int TotalSystemStorageGiB => StorageSummary?.TotalSystemStorageGiB ?? 0;

        /// <summary>
        /// Node的CPU电源状态。
        /// </summary>
        /// <value>The state of the power.</value>
        [JsonProperty("PowerState")]
        public string PowerState { get; set; }

        public string ProcessorInfo
        {
            get
            {
                int count = this.ProcessorSummary.FirstOrDefault()?.Count ?? 0;
                if (count == 0)
                {
                    return string.Empty;
                }
                string type = this.ProcessorSummary.FirstOrDefault()?.Type ?? string.Empty;
                return $"{count} processor,{type}";
            }
        }

        /// <summary>
        /// 指定Node资源所属机框的序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Node状态。
        /// </summary>
        /// <value>The state of the server.</value>
        [JsonProperty("State")]
        public string ServerState { get; set; }

        /// <summary>
        /// StateReason
        /// </summary>
        /// <value>The state reason.</value>
        [JsonProperty("StateReason")]
        public string StateReason { get; set; }

        /// <summary>
        /// ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string iBMCIPv4Address => IPv4Address?.Address ?? string.Empty;

        /// <summary>
        /// Node上的设备标签。
        /// </summary>
        /// <value>The tag.</value>
        [JsonProperty("Tag")]
        public string Tag { get; set; }

        public string ProfileModelName => ProfileModelData?.Name ?? string.Empty;

        /// <summary>
        /// 指定Node资源资产标签。
        /// </summary>
        /// <value>The asset tag.</value>
        [JsonProperty("AssetTag")]
        public string AssetTag { get; set; }

        /// <summary>
        /// Gets the health.
        /// </summary>
        /// <value>The health.</value>
        public Health Health => Status.Health;

        /// <summary>
        /// Node上的BMC IPv4地址信息。
        /// </summary>
        /// <value>The i PV4 address.</value>
        [JsonProperty("IPv4Address")]
        public IPv4Address IPv4Address { get; set; }

        /// <summary>
        /// ProfileModel信息。
        /// </summary>
        /// <value>The profile model.</value>
        [JsonProperty("ProfileModel")]
        public ProfileModel ProfileModelData { get; set; }

        /// <summary>
        /// 指定系统资源的CPU信息。
        /// </summary>
        /// <value>The processor summary.</value>
        [JsonProperty("ProcessorSummary")]
        public List<ProcessorSummary> ProcessorSummary { get; set; } = new List<ProcessorSummary>();

        /// <summary>
        /// 内存大小。
        /// </summary>
        /// <value>The memory summary.</value>
        [JsonProperty("MemorySummary")]
        public MemorySummary MemorySummary { get; set; }

        /// <summary>
        /// 指定Node资源RAID卡信息。
        /// </summary>
        /// <value>The storage summary.</value>
        [JsonProperty("StorageSummary")]
        public StorageSummary StorageSummary { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [JsonProperty("Status")]
        public Status Status { get; set; }

        public void MakeDetails(string fusionDirectorIp)
        {
            this.FusionDirectorIp = fusionDirectorIp;
        }

        #region 暂无用

        #region Childs

        //public List<Processor> Processors { get; set; } = new List<Processor>();

        //public List<Memory> Memorys { get; set; } = new List<Memory>();

        //public List<NetworkAdapter> NetworkAdapters { get; set; } = new List<NetworkAdapter>();

        //public List<RaidCard> RaidCards { get; set; } = new List<RaidCard>();

        //public List<Drive> Drives { get; set; } = new List<Drive>();

        //public List<PCIeCard> PCIeCards { get; set; } = new List<PCIeCard>();

        //public List<Power> Powers { get; set; } = new List<Power>();

        //public List<Fan> Fans { get; set; } = new List<Fan>();

        #endregion

        ///// <summary>
        ///// Node的使能状态。支持的状态包括：Enabled、Absent。
        ///// </summary>
        ///// <value>The health.</value>
        //public string EnableState => Status.State;

        ///// <summary>
        ///// Node上的设备ID。
        ///// </summary>
        ///// <value>The device identifier.</value>
        //[JsonProperty("DeviceID")]
        //public string DeviceID { get; set; }

        ///// <summary>
        ///// Node的GPU总数。
        ///// </summary>
        ///// <value>The gpu summary.</value>
        //[JsonProperty("GPUSummary")]
        //public object GPUSummary { get; set; }

        ///// <summary>
        ///// 指定Node资源网卡个数信息。
        ///// </summary>
        ///// <value>The network adapter summary.</value>
        //[JsonProperty("NetworkAdapterSummary")]
        //public NetworkAdapterSummary NetworkAdapterSummary { get; set; }


        ///// <summary>
        ///// 指定Node资源上FPGA信息。
        ///// </summary>
        ///// <value>The fpga summary.</value>
        //[JsonProperty("FPGASummary")]
        //public object FPGASummary { get; set; }

        ///// <summary>
        ///// Node上OS信息。
        ///// </summary>
        ///// <value>The os summary.</value>
        //[JsonProperty("OSSummary")]
        //public object OSSummary { get; set; }

        ///// <summary>
        ///// 指定Node资源BIOS信息。
        ///// </summary>
        ///// <value>The bios.</value>
        //[JsonProperty("BIOS")]
        //public ODataId BIOS { get; set; }

        ///// <summary>
        ///// 指定Node资源BMC信息。
        ///// </summary>
        ///// <value>The BMC.</value>
        //[JsonProperty("BMC")]
        //public ODataId BMC { get; set; }

        ///// <summary>
        ///// 指定Node资源Catalogue信息。
        ///// </summary>
        ///// <value>The catalogue.</value>
        //[JsonProperty("Catalogue")]
        //public CatalogueItem Catalogue { get; set; }

        ///// <summary>
        ///// 指定Node资源FPGA信息。
        ///// </summary>
        ///// <value>The fpga.</value>
        //[JsonProperty("FPGA")]
        //public ODataId FPGA { get; set; }

        ///// <summary>
        ///// 指定Node资源Firmware信息。
        ///// </summary>
        ///// <value>The firmware.</value>
        //[JsonProperty("Firmware")]
        //public ODataId Firmware { get; set; }

        ///// <summary>
        ///// 指定Node资源Power信息。
        ///// </summary>
        ///// <value>The power.</value>
        //[JsonProperty("Power")]
        //public ODataId Power { get; set; }

        ///// <summary>
        ///// 指定Node资源Thermal信息。
        ///// </summary>
        ///// <value>The thermal.</value>
        //[JsonProperty("Thermal")]
        //public ODataId Thermal { get; set; }

        ///// <summary>
        ///// 指定Node资源GPU信息。
        ///// </summary>
        ///// <value>The gpu.</value>
        //[JsonProperty("GPU")]
        //public ODataId GPU { get; set; }

        ///// <summary>
        ///// 指定Node资源IndicatorLED信息。
        ///// </summary>
        ///// <value>The indicator led.</value>
        //[JsonProperty("IndicatorLED")]
        //public ODataId IndicatorLED { get; set; }

        ///// <summary>
        ///// 指定Node资源Processor信息的。
        ///// </summary>
        ///// <value>The processor.</value>
        //[JsonProperty("Processor")]
        //public ODataId Processor { get; set; }

        ///// <summary>
        ///// 指定Node资源Memory信息。
        ///// </summary>
        ///// <value>The memory.</value>
        //[JsonProperty("Memory")]
        //public ODataId Memory { get; set; }

        ///// <summary>
        ///// 指定Node资源NetworkAdapter信息。
        ///// </summary>
        ///// <value>The network adapter.</value>
        //[JsonProperty("NetworkAdapter")]
        //public ODataId NetworkAdapter { get; set; }

        ///// <summary>
        ///// 指定Node资源NetworkInterface信息。
        ///// </summary>
        ///// <value>The network interface.</value>
        //[JsonProperty("NetworkInterface")]
        //public ODataId NetworkInterface { get; set; }

        ///// <summary>
        ///// 指定Node资源PCIe信息。
        ///// </summary>
        ///// <value>The pc ie.</value>
        //[JsonProperty("PCIe")]
        //public ODataId PCIe { get; set; }

        ///// <summary>
        ///// 指定Node资源Storage信息。
        ///// </summary>
        ///// <value>The storage.</value>
        //[JsonProperty("Storage")]
        //public ODataId Storage { get; set; }

        ///// <summary>
        ///// Gets or sets the link.
        ///// </summary>
        ///// <value>The link.</value>
        //[JsonProperty("Link")]
        //public object Link { get; set; }

        ///// <summary>
        ///// Profile信息。
        ///// </summary>
        ///// <value>The profile.</value>
        //[JsonProperty("Profile")]
        //public object Profile { get; set; }

        ///// <summary>
        ///// Node的名称。
        ///// </summary>
        ///// <value>The name.</value>
        //[JsonProperty("Name")]
        //public string Name { get; set; }

        ///// <summary>
        ///// Node的生产厂商信息。
        ///// </summary>
        ///// <value>The manufacturer.</value>
        //[JsonProperty("Manufacturer")]
        //public string Manufacturer { get; set; }

        ///// <summary>
        ///// Node的槽号。
        ///// </summary>
        ///// <value>The slot.</value>
        //[JsonProperty("Slot")]
        //public string Slot { get; set; }

        ///// <summary>
        ///// Node上的设备别名。
        ///// </summary>
        ///// <value>The alias.</value>
        //[JsonProperty("Alias")]
        //public string Alias { get; set; }

        ///// <summary>
        ///// Node所属用户组。
        ///// string / List string 
        ///// </summary>
        ///// <value>The group.</value>
        //[JsonProperty("Group")]
        //public object Group { get; set; }

        ///// <summary>
        ///// Gets or sets the group object.
        ///// </summary>
        ///// <value>The group object.</value>
        //[JsonIgnore]
        //public string GroupTxt => this.GetGroupString();

        //public string GetGroupString()
        //{
        //    if (Group == null)
        //    {
        //        return string.Empty;
        //    }
        //    var t = Group.GetType();
        //    if (t.Name == "JArray")
        //    {
        //        JArray array = Group as JArray;
        //        return string.Join(",", array.Select(x => x.ToString()));
        //    }
        //    if (t.Name == "JValue")
        //    {
        //        return Group.ToString();
        //    }
        //    return Group.ToString();
        //}
        #endregion
    }
}