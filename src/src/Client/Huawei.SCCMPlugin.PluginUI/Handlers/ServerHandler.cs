using CommonUtil;
using Huawei.SCCMPlugin.Core;
using Huawei.SCCMPlugin.Core.Exceptions;
using Huawei.SCCMPlugin.Core.Model.Request;
using Huawei.SCCMPlugin.Core.Model.Response;
using Huawei.SCCMPlugin.Models;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Helper;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
    /// <summary>
    /// 服务器管理
    /// </summary>
    [CefURL("huawei/server/list.html")]
    [Bound("NetBound")]
    public class ServerHandler : BaseHandler, IWebHandler
    {

    }
}
