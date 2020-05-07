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
// Created          : 02-18-2019
//
// Last Modified By : yayun
// Last Modified On : 02-18-2019
// ***********************************************************************
// <copyright file="ServerRealTimePerformance.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    public class ServerRealTimePerformance
    {
        /// <summary>
        /// 服务器Id
        /// </summary>
        public string ServerID { get; set; }

        /// <summary>
        /// 查询时间
        /// </summary>
        public long OccurTime { get; set; }

        /// <summary>
        /// 结果集
        /// </summary>
        [JsonProperty("Results")]
        public ServerPerformanceColloction PercentItems { get; set; }
      
    }
}
