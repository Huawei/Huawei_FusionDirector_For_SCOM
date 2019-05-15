using CommonUtil.ModelHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.Models
{
    [Serializable]
    [DbTableName("Certificates")]
   public class CertificatesModel : BaseModel
    {
        [JsonProperty(PropertyName = "Content")]
        [DbColumn("CONTENT")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "createTime")]
        [DbColumn("CREATETIME")]
        public DateTime CreateTime { get; set; }

        [JsonProperty(PropertyName = "updateTime")]
        [DbColumn("UPDATETIME")]
        public DateTime UpdateTime { get; set; }
    }
}
