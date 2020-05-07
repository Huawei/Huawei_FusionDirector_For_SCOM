//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System.Linq;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    public partial class CommonResponse
    {
        [JsonProperty("error")]
        public Error Error { get; set; } = new Error();

        /// <summary>
        /// 错误消息
        /// </summary>
        /// <value>The error message.</value>
        public string ErrorMessage => Error.MessageExtendedInfo.FirstOrDefault()?.Message;

        /// <summary>
        /// 解决方案
        /// </summary>
        /// <value>The resolution.</value>
        public string Resolution => Error.MessageExtendedInfo.FirstOrDefault()?.Resolution;

    }
}