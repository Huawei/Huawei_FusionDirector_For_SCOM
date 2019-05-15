using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
    /// <summary>
    /// 升级计划
    /// </summary>
    [CefURL("huawei/upgradePlan/index.html")]
    [Bound("NetBound")]
    public class UpgradePlanHandler : BaseHandler, IWebHandler
    {
    }
}
