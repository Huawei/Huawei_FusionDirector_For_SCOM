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
// <copyright file="ServerPerformanceColloction.cs" company="Huawei Technologies Co. Ltd">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace FusionDirectorPlugin.Model
{
    public class ServerPerformanceColloction
    {
        /// <summary>
        /// CPU使用率
        /// </summary>
        public List<PercentItem> CPUUsagePercent { get; set; }

        /// <summary>
        /// 电源里实时功率
        /// </summary>
        public List<PercentItem> PowerConsumedWatts { get; set; }

        /// <summary>
        /// 入风口的温度
        /// </summary>
        public List<PercentItem> InletTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> CPUTemp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> FanReading { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> FanSpeedLevelPercents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> MemoryUsagePercent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> PowerInputWatts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> PowerOutputWatts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<PercentItem> TemperatureReadingCelsius { get; set; }
    }
}