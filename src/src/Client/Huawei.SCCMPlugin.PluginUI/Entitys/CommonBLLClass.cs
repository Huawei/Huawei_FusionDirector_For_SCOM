using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys
{
    /// <summary>
    /// 删除任务API参数对象，用于升级任务、模板任务
    /// </summary>
    public class ParamOfDeleteTask
    {
        [JsonProperty("taskId")]
        public int TaskId { get; set; }
    }

    /// <summary>
    /// 分页参数对象，用于只有页码和页大小两个参数的UI
    /// </summary>
    public class ParamOnlyPagingInfo
    {
        [JsonProperty("pageNo")]
        public int PageNo { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 分页参数对象，用于查询FD列表
    /// </summary>
    public class ParamPagingOfQueryFD
    {
        [JsonProperty("pageNo")]
        public int PageNo { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("hostIp")]
        public string HostIp { get; set; }
    }
    
}
