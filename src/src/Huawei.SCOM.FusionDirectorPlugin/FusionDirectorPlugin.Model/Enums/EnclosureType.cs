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
// Created          : 12-27-2018
//
// Last Modified By : panwei
// Last Modified On : 12-27-2018
// ***********************************************************************
// <copyright file="EnclosureType.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using FusionDirectorPlugin.Model.EnumSe;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    /// <summary>
    /// 机框的类型。
    /// E9000 E9000机框。
    /// X6000 X6000系列机框。
    /// Kunlun Kunlun系列机框。
    /// </summary>
   [JsonConverter(typeof(EnclosureTypeEnumConverter))]
    public enum EnclosureType
    {
        /// <summary>
        /// The e9000
        /// </summary>
        E9000 = 0,

        /// <summary>
        /// The X6000
        /// </summary>
        X6000 = 1,

        /// <summary>
        /// The kunlun
        /// </summary>
        Kunlun = 2,

        Unknown=-1
    }
}