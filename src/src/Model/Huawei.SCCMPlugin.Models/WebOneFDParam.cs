using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.Models
{
    public class WebOneFDParam<T>
    {
        [JsonProperty(PropertyName = "fd")]
        public string FDIP { get; set; }

        [JsonProperty(PropertyName = "param")]
        public T Param { get; set; }
    }
}
