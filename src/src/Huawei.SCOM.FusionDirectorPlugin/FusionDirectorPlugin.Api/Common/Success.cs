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
// Assembly         : FusionDirectorPlugin.Api
// Author           : panwei
// Created          : 12-26-2018
//
// Last Modified By : panwei
// Last Modified On : 12-26-2018
// ***********************************************************************
// <copyright file="Success.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Api
{
    /// <summary>
    /// Class Success.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Success 
    {
        /// <summary>
        /// 指示消息注册表中特定消息ID的字符串。
        /// </summary>
        /// <value>The code.</value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 与消息注册表中的消息对应的易读的错误消息。
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>The error.</value>
        [JsonProperty("error")]
        public Error Error { get; set; }
    }

    /// <summary>
    /// Class Error.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class Error 
    {
        /// <summary>
        /// Gets or sets the message extended information.
        /// </summary>
        /// <value>The message extended information.</value>
        [JsonProperty("@Message.ExtendedInfo")]
        public ObservableCollection<MessageExtendedInfo> MessageExtendedInfo { get; set; }
    }

    /// <summary>
    /// Class MessageExtendedInfo.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public partial class MessageExtendedInfo
    {
        /// <summary>
        /// 消息资源的OData描述信息。
        /// </summary>
        /// <value>The odatatype.</value>
        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        /// <summary>
        /// 详细信息。
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty("Message")]
        public string Message { get; set; }

        /// <summary>
        /// 信息参数。
        /// </summary>
        /// <value>The message arguments.</value>
        [JsonProperty("MessageArgs")]
        public ObservableCollection<string> MessageArgs { get; set; }

        /// <summary>
        /// 消息ID。
        /// </summary>
        /// <value>The message identifier.</value>
        [JsonProperty("MessageID")]
        public string MessageID { get; set; }

        /// <summary>
        /// 消息相关属性。
        /// </summary>
        /// <value>The relate properties.</value>
        [JsonProperty("RelateProperties")]
        public ObservableCollection<string> RelateProperties { get; set; }

        /// <summary>
        /// 解决建议。
        /// </summary>
        /// <value>The resolution.</value>
        [JsonProperty("Resolution")]
        public string Resolution { get; set; }

        /// <summary>
        /// 严重性。Redfish支持的严重级别包括：OK、Warning、Critical。
        /// </summary>
        /// <value>The severity.</value>
        [JsonProperty("Severity")]
        public string Severity { get; set; }
    }
}
