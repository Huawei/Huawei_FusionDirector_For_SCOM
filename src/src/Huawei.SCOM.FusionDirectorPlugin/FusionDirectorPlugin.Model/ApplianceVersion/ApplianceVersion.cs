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
    public class ApplianceVersion
    {
        /// <summary>
        /// Node资源模型的OData描述信息。
        /// </summary>
        /// <value>The o data context.</value>
        [JsonProperty("@odata.context")]
        public string ODataContext { get; set; }

        /// <summary>
        /// 指定Node资源节点的访问路径。
        /// </summary>
        /// <value>The o data identifier.</value>
        [JsonProperty("@odata.id")]
        public string ODataId { get; set; }

        /// <summary>
        /// 指定Node资源类型。
        /// </summary>
        /// <value>The type of the o data.</value>
        [JsonProperty("@odata.type")]
        public string ODataType { get; set; }

        /// <summary>
        /// CurrentVersion
        /// </summary>
        [JsonProperty("CurrentVersion")]
        public string CurrentVersion { get; set; }

        /// <summary>
        /// InActiveVersion
        /// </summary>
        [JsonProperty("InActiveVersion")]
        public string InActiveVersion { get; set; }

        /// <summary>
        /// MinimunVersion
        /// </summary>
        [JsonProperty("MinimunVersion")]
        public string MinimunVersion { get; set; }

        /// <summary>
        /// UpgradeTime
        /// </summary>
        [JsonProperty("UpgradeTime")]
        public string UpgradeTime { get; set; }

        /// <summary>
        /// ActivatedTime
        /// </summary>
        [JsonProperty("ActivatedTime")]
        public string ActivatedTime { get; set; }
    }
}
