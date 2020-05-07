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
// Author           : yayun
// Created          : 02-26-2019
//
// Last Modified By : yayun
// Last Modified On : 02-26-2019
// ***********************************************************************
// <copyright file="PushData.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************


using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model.Event
{
    public class PushData
    {
        [JsonProperty("data")]
        public AlarmData Data { get; set; }

    }
}

