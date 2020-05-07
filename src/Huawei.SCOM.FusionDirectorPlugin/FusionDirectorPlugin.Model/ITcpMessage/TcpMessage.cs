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
// Assembly         : FusionDirectorPlugin.Dal
// Author           : panwei
// Created          : 12-28-2018
//
// Last Modified By : panwei
// Last Modified On : 01-02-2019
// ***********************************************************************
// <copyright file="TcpMessage.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// The tcp messge type.
    /// </summary>
    public enum TcpMessageType
    {
        /// <summary>
        /// alarm.
        /// </summary>
        Alarm,
       
    }

    /// <summary>
    /// The tcp message.
    /// </summary>
    /// <typeparam name="T">object</typeparam>
    /// <seealso cref="FusionDirectorPlugin.Dal.Model.ITcpMessage" />
    /// <seealso cref="FusionDirectorPlugin.WebServer.Models.ITcpMessage" />
    public class TcpMessage<T> : ITcpMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpMessage{T}"/> class.
        /// </summary>
        /// <param name="hostIp">The host ip.</param>
        /// <param name="msgType">Type of the MSG.</param>
        /// <param name="data">The data.</param>
        public TcpMessage(string auth, TcpMessageType msgType, T data)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Auth = auth;
            this.MsgType = msgType;
            this.Data = data;
        }

        /// <summary>
        /// Gets or sets the auth.
        /// </summary>
        [JsonProperty("auth")]
        public string Auth { get; set; }

        /// <summary>
        /// Gets or sets the msg type.
        /// </summary>
        [JsonProperty("tcpMessageType")]
        public TcpMessageType MsgType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value>The desc.</value>
        public string Desc => this.MsgType.ToString();

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public T Data { get; set; }
    }
}