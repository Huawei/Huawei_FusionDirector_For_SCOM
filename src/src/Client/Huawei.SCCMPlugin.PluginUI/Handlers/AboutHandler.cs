using CommonUtil;
using Huawei.SCCMPlugin.Core.Exceptions;
using Huawei.SCCMPlugin.Core.Model.Response;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.FDScheme;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using Huawei.SCCMPlugin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
    /// <summary>
    /// 关于
    /// </summary>
    [CefURL("huawei/about/about.html")]
    [Bound("NetBound")]
    public class AboutHandler : IWebHandler
    {
        public FDBrowser FDBrowser { get; set; }
        public string Execute(string eventName, object eventData)
        {
            object result = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "unknown");
            try
            {
                LogUtil.HWLogger.UI.Info("Executing the method of FDsetting page...");
                switch (eventName)
                {
                    case "getVersion":
                        result = GetVersion(eventData);
                        break;
                    default: break;
                }
                return JsonUtil.SerializeObject(result);
            }
            catch (JsonSerializationException ex)
            {
                LogUtil.HWLogger.UI.Error("Execute the method in FDsetting page fail.", ex);
            }
            catch (NullReferenceException ex)
            {
                LogUtil.HWLogger.UI.Error("Execute the method in FDsetting page fail.", ex);
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Execute the method in FDsetting page fail.", ex);
            }
            return JsonUtil.SerializeObject(result);
        }

        /// <summary>
        ///请求FD接口 
        /// </summary>
        public ApiResult<string> GetVersion(object eventData)
        {
            ApiResult<string> ret = new ApiResult<string>(ErrorCode.SYS_UNKNOWN_ERR, "");
            string version = "1.0.0.0";
            try
            {
                //1. 获取数据 
                string fileVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string[] arrVersion = fileVersion.Split('.');
                if (arrVersion.Length >= 3)
                {
                    version = string.Format("{0}.{1}.{2}", arrVersion[0], arrVersion[1], arrVersion[2]);
                }
                else
                {
                    version = fileVersion;
                }
                //2. 返回数据
                ret.Code = "0";
                ret.Data = version;
                LogUtil.HWLogger.UI.InfoFormat("Get version successful, the ret is [{0}]", JsonUtil.SerializeObject(ret));
            }
            catch (BaseException ex)
            {
                LogUtil.HWLogger.UI.Error("Get version failed: ", ex);
                ret.Code = ex.Code;

            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Get version failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
            }
            return ret;
        }
    }
}
