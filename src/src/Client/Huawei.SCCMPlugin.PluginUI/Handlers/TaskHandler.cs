using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{

    /// <summary>
    /// 任务管理
    /// </summary>
    [CefURL("huawei/task/index.html")]
    [Bound("NetBound")]
    public class TaskHandler : BaseHandler, IWebHandler
    {
    }
}
