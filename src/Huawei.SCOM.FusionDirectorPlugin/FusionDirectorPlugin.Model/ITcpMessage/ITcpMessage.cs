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
// Last Modified On : 12-28-2018
// ***********************************************************************
// <copyright file="ITcpMessage.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// The TcpMessage interface.
    /// </summary>
    public interface ITcpMessage
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        ///     Gets or sets the msg type.
        /// </summary>
        TcpMessageType MsgType { get; set; }
    }
}