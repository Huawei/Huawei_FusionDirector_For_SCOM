using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
    /// <summary>
    /// 设备版本状态
    /// </summary>
    [CefURL("huawei/deviceVersionStatus/index.html")]
    [Bound("NetBound")]
    public class DeviceVersionStatusPlanHandler : BaseHandler, IWebHandler
    {
    }
}
