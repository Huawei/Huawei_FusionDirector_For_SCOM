//**************************************************************************  
//Copyright (C) 2019 Huawei Technologies Co., Ltd. All rights reserved.
//This program is free software; you can redistribute it and/or modify
//it under the terms of the MIT license.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//MIT license for more detail.
//*************************************************************************  
﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace FusionDirectorPlugin.Model
{
    public partial class MessageExtendedInfo
    {
        [JsonProperty("MessageArgs")]
        public object MessageArgs { get; set; } 

        [JsonProperty("RelatedProperties")]
        public object RelatedProperties { get; set; } 

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("@odata.type")]
        public string OdataType { get; set; }

        [JsonProperty("Severity")]
        public string Severity { get; set; }

        [JsonProperty("MessageId")]
        public string MessageId { get; set; }

        [JsonProperty("Resolution")]
        public string Resolution { get; set; }


    }
}