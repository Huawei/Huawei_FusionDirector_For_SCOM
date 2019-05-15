using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{

    /// <summary>
    /// OS部署管理
    /// </summary>
    [CefURL("huawei/osDeploy/index.html")]
    [Bound("NetBound")]
    public class OSDeployHandler : BaseHandler, IWebHandler
    {
    }
}
