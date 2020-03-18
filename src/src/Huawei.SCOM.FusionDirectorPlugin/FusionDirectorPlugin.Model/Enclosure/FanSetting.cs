//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    public class FanSetting
    {
        /// <summary>
        /// FanSpeedAdjustmentMode
        /// </summary>
        [JsonProperty("FanSpeedAdjustmentMode")]
        public string FanSpeedAdjustmentMode { get; set; }

        /// <summary>
        /// FanSmartCoolingMode
        /// </summary>
        [JsonProperty("FanSmartCoolingMode")]
        public string FanSmartCoolingMode { get; set; }
    }
}
