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
// Assembly         : WindowsFormsApp1
// Author           : mike
// Created          : 12-27-2018
//
// Last Modified By : mike
// Last Modified On : 01-07-2019
// ***********************************************************************
// <copyright file="Enclosure.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ************************************************************************

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 该资源表明了一台物理机框以及它的各种信息。
    /// </summary>
    public partial class Enclosure
    {
        public Enclosure()
        {
            this.ODataId = string.Empty;
            this.Name = string.Empty;
            this.Type = EnclosureType.Unknown;
            this.Hostname = string.Empty;
            this.SerialNumber = string.Empty;
            this.PartNumber = string.Empty;
            this.EnclosureState = EnclosureState.Unknown;
            this.StateReason = string.Empty;
            this.Health = new Health();
        }

        public Enclosure(EnclosureSummary enclosureSummary)
        {
            this.ODataId = enclosureSummary.ODataId;
            this.Name = enclosureSummary.Name;
            this.Type = enclosureSummary.Type;
            this.Hostname = string.Empty;
            this.SerialNumber = string.Empty;
            this.PartNumber = string.Empty;
            this.EnclosureState = EnclosureState.Unknown;
            this.StateReason = enclosureSummary.StateReason;
            this.Health = enclosureSummary.Health;
        }
        /// <summary>
        /// Gets the union identifier.
        /// </summary>
        /// <value>The union identifier.</value>
        public string UnionId => $"{FusionDirectorIp}-{this.UUID}";

        /// <summary>
        /// Gets or sets the fusion director ip.
        /// </summary>
        /// <value>The fusion director ip.</value>
        public string FusionDirectorIp { get; set; }

        /// <summary>
        /// 资源唯一标识符。
        /// </summary>
        /// <value>The identifier.</value>
        public string UUID => ODataId.Split('/').Last();

        /// <summary>
        /// 用于odata的资源唯一标识符。
        /// </summary>
        /// <value>The odata identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 资源的名字。
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("Type")]
        public EnclosureType Type { get; set; }

        public string FirmwareVersion => string.Join("/", this.EnabledManagerSlot.Select(x => x.FirmwareVersion));

        /// <summary>
        /// 管理板的主机地址。
        /// </summary>
        /// <value>The hostname.</value>
        [JsonProperty("Hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// 资源的序列号。
        /// </summary>
        /// <value>The serial number.</value>
        [JsonProperty("SerialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 资源的部件码。
        /// </summary>
        /// <value>The part number.</value>
        [JsonProperty("PartNumber")]
        public string PartNumber { get; set; }

        public string ProductName => this.BoardFru.ProductName;

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        [JsonProperty("State")]
        public EnclosureState EnclosureState { get; set; }

        /// <summary>
        /// 机框处于某种状态的原因，例如当机框处于Locked状态时，原因可能是正在下发机框配置。
        /// </summary>
        /// <value>The state reason.</value>
        [JsonProperty("StateReason")]
        public string StateReason { get; set; }

        public string FanSpeedAdjustmentMode => this.EnclosureSetting?.FanSetting?.FanSpeedAdjustmentMode ?? string.Empty;

        public string HMMFloatIPv4Address => this.EnabledManagerSlot.FirstOrDefault(x => x.FloatIPv4Address != null)?.FloatIPv4Address.Address ?? string.Empty;

        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        [JsonProperty("Health")]
        public Health Health { get; set; } = Health.Warning;

        /// <summary>
        /// Gets or sets the slot.
        /// </summary>
        /// <value>The slot.</value>
        [JsonProperty("Slot")]
        public Slot Slot { get; set; } = new Slot();

        /// <summary>
        /// Gets or sets the enclosure setting.
        /// </summary>
        /// <value>The enclosure setting.</value>
        [JsonProperty("EnclosureSetting")]
        public EnclosureSetting EnclosureSetting { get; set; }

        [JsonProperty("BoardFru")]
        public BoardFru BoardFru { get; set; }

        /// <summary>
        /// Gets the enabled manager slot.
        /// </summary>
        /// <value>The enabled manager slot.</value>
        public List<ManagerSlot> EnabledManagerSlot => this.Slot.ManagerSlot.ToList();

        public void MakeDetails(string fusionDirectorIp)
        {
            this.FusionDirectorIp = fusionDirectorIp;
            //this.EnabledServerSlot.ForEach(x =>
            //{
            //    x.FusionDirectorIp = this.FusionDirectorIp;
            //});
            //this.EnabledSwitchSlot.ForEach(x =>
            //{
            //    x.FusionDirectorIp = this.FusionDirectorIp;
            //});
            //this.EnabledManagerSlot.ForEach(x =>
            //{
            //    x.FusionDirectorIp = this.FusionDirectorIp;
            //});
            //this.EnabledFanSlot.ForEach(x =>
            //{
            //    x.FusionDirectorIp = this.FusionDirectorIp;
            //    x.UUID = this.UUID + x.Name;
            //});
            //this.EnabledPowerSlot.ForEach(x =>
            //{
            //    x.FusionDirectorIp = this.FusionDirectorIp;
            //    x.UUID = this.UUID + x.Name;
            //});
        }

        ///// <summary>
        ///// Gets the enabled fan slot.
        ///// </summary>
        ///// <value>The enabled fan slot.</value>
        //public List<FanSlot> EnabledFanSlot => this.Slot.FanSlot.ToList();

        ///// <summary>
        ///// Gets the enabled power slot.
        ///// </summary>
        ///// <value>The enabled power slot.</value>
        //public List<PowerSlot> EnabledPowerSlot => this.Slot.PowerSlot.ToList();

        ///// <summary>
        ///// Gets the enabled server slot.
        ///// </summary>
        ///// <value>The enabled server slot.</value>
        //public List<ServerSlot> EnabledServerSlot => this.Slot.ServerSlot.ToList();

        ///// <summary>
        ///// Gets the enabled switch slot.
        ///// </summary>
        ///// <value>The enabled switch slot.</value>
        //public List<SwitchSlot> EnabledSwitchSlot => this.Slot.SwitchSlot.ToList();

        ///// <summary>
        ///// 添加机框时使用的Port。
        ///// </summary>
        ///// <value>The port.</value>
        //[JsonProperty("Port")]
        //public int Port { get; set; }

        ///// <summary>
        ///// 添加机框时使用的通讯协议。
        ///// </summary>
        ///// <value>The protocol.</value>
        //[JsonProperty("Protocol")]
        //public string Protocol { get; set; }

        ///// <summary>
        ///// 资源的创建的时间。
        ///// </summary>
        ///// <value>The created at.</value>
        //[JsonProperty("CreatedAt")]
        //public string CreatedAt { get; set; }

        ///// <summary>
        ///// 资源的更新的时间。
        ///// </summary>
        ///// <value>The updated at.</value>
        //[JsonProperty("UpdatedAt")]
        //public string UpdatedAt { get; set; }

        ///// <summary>
        ///// 资源的描述。最大255字节。
        ///// </summary>
        ///// <value>The description.</value>
        //[JsonProperty("Description")]
        //public string Description { get; set; }


        ///// <summary>
        ///// 机框编号。机框编号为字符串表示的整数，范围是0-999999。
        ///// </summary>
        ///// <value>The chassis identifier.</value>
        //[JsonProperty("ChassisID")]
        //public string ChassisID { get; set; }

        ///// <summary>
        ///// 机框硬件类型编号。
        ///// </summary>
        ///// <value>The type of the chassis.</value>
        //[JsonProperty("ChassisType")]
        //public string ChassisType { get; set; }

        ///// <summary>
        ///// LCD的版本号。
        ///// </summary>
        ///// <value>The LCD version.</value>
        //[JsonProperty("LCDVersion")]
        //public string LCDVersion { get; set; }

        ///// <summary>
        ///// 机框UID灯的状态。
        ///// Lit 灯亮。
        ///// Off 灯灭。
        ///// </summary>
        ///// <value>The state of the uid.</value>
        //[JsonProperty("UIDState")]

        //public string UIDState { get; set; }

        ///// <summary>
        ///// Gets or sets the state of the enclosure profile usage.
        ///// </summary>
        ///// <value>The state of the enclosure profile usage.</value>
        //[JsonProperty("EnclosureProfileUsageState")]

        //public EnclosureProfileUsageState EnclosureProfileUsageState { get; set; }

        ///// <summary>
        ///// 机框配置文件的链接。
        ///// </summary>
        ///// <value>The enclosure profile URL.</value>
        //[JsonProperty("EnclosureProfileURL")]
        //public string EnclosureProfileURL { get; set; }

        ///// <summary>
        ///// 机框硬件能力的链接。
        ///// </summary>
        ///// <value>The enclosure hardware capability URL.</value>
        //[JsonProperty("EnclosureHardwareCapabilityURL")]
        //public string EnclosureHardwareCapabilityURL { get; set; }

    }
}