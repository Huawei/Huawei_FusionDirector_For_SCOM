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
using Huawei.SCCMPlugin.FusionDirector.PluginUI.FDScheme;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
   
    public class BaseHandler
    {
      public  FDBrowser FDBrowser { get; set; }

        public virtual string Execute(string eventName, object eventData)
        {
            try
            {
                LogUtil.HWLogger.UI.Info("Executing the method of Server page...");
                object result = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "unknown");
                switch (eventName)
                {
                    case "requestFusionDirectorAPI":
                        result = RequestFusionDirectorAPI(eventData);
                        break;

                    case "loadFDList":
                        result = LoadFDList(eventData);
                        break;
                    default: break;
                }
                return JsonConvert.SerializeObject(result);
            }
            catch (JsonSerializationException ex)
            {
                LogUtil.HWLogger.UI.Error("Call JsonConvert.SerializeObject failed.", ex);
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Execute the method in Server page failed.", ex);
            }
            return "{code:"+ErrorCode.SYS_UNKNOWN_ERR+",data:[]}";
        }

        /// <summary>
        ///请求FD接口 
        /// </summary>
        public virtual CommonResponse RequestFusionDirectorAPI(object eventData)
        {
            var ret = new CommonResponse("0");
            try
            {
                var jsData = JsonUtil.SerializeObject(eventData);
                var request = JsonUtil.DeserializeObject<CommonRequest>(jsData);
                return HttpHelper.Instance.CommonHttpHandle(request);
            }
            catch (BaseException ex)
            {
                LogUtil.HWLogger.UI.Error("ExeFDApi failed: ", ex);
                ret.Code = ex.Code;

            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("ExeFDApi failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
            }
            return ret;

        }

        /// <summary>
        /// 加载FD
        /// </summary>
        public ApiListResult<FusionDirectorModel> LoadFDList(object eventData)
        {
            try
            {
                //检查密码 刷新密码
                RefreshPassword refreshPassword = new RefreshPassword();
                refreshPassword.CheckAndUpgradeKey();
                return CommonBLLMethodHelper.LoadFDList();
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("LoadFDList failed: ", ex);
                return new ApiListResult<FusionDirectorModel>(ErrorCode.SYS_UNKNOWN_ERR, "");
            }

        }
    }
}
