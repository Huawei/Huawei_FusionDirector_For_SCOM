using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Helper;
using Newtonsoft.Json;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys;
using Huawei.SCCMPlugin.Models;
using CommonUtil;
using Huawei.SCCMPlugin.Core.Workers;
using Huawei.SCCMPlugin.Core;
using Huawei.SCCMPlugin.Core.Exceptions;
using Huawei.SCCMPlugin.Core.Model.Response;
using Huawei.SCCMPlugin.Core.Model.Request;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.FDScheme;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
    /// <summary>
    /// FD配置
    /// </summary>
    [CefURL("huawei/FDConfig/index.html")]
    [Bound("NetBound")]
    public class FDsettingHandler : IWebHandler
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
                    case "getList":
                        result = GetList(eventData);
                        break;
                    case "test":
                        result = Test(eventData);
                        break;
                    case "add":
                        result = Add(eventData);
                        break;
                    case "edit":
                        result = Edit(eventData);
                        break;
                    case "editNoCert":
                        result = EditNoCert(eventData);
                        break;
                    case "delete":
                        result = Delete(eventData);
                        break;
                    case "requestFusionDirectorAPI":
                        result = RequestFusionDirectorAPI(eventData);
                        break;
                    case "saveCert":
                        result = SaveCert(eventData);
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
        /// 获取FD列表数据
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        private ApiListResult<FusionDirectorModel> GetList(object eventData)
        {
            var ret = new ApiListResult<FusionDirectorModel>(false, ErrorCode.SYS_UNKNOWN_ERR, "", "", null);
            try
            {
                //1. 处理参数
                var jsData = JsonUtil.SerializeObject(eventData);
                var webOneFDParam = JsonUtil.DeserializeObject<WebOneFDParam<ParamPagingOfQueryFD>>(jsData);
                LogUtil.HWLogger.UI.InfoFormat("Querying FD list, the param is [{0}]", jsData);
                int pageSize = webOneFDParam.Param.PageSize;
                int pageNo = webOneFDParam.Param.PageNo;
                string hostIp = webOneFDParam.Param.HostIp;
                //2. 获取数据 
                var list = FusionDirectorWorker.Instance.GetList();
                var filterList = list.Where(x => x.HostIP.Contains(hostIp)).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                filterList.ForEach(x =>
                {
                    x.LoginPwd = "";
                });
                //3. 返回数据
                ret.Code = "0";
                ret.Data = filterList;
                ret.TotalNum = list.Count();
                ret.Description = "";
                LogUtil.HWLogger.UI.InfoFormat("Querying FD list successful, the ret is [{0}]", JsonUtil.SerializeObject(ret));
            }

            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error(ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Data = null;
                ret.TotalNum = 0;
                LogUtil.HWLogger.UI.Error("Querying FD list failed: ", ex);
            }
            return ret;
        }

        private ApiResult Test(object eventData)
        {
            ApiResult ret = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "");
            try
            {
                var jsData = JsonUtil.SerializeObject(eventData);
                var newJsData = CommonBLLMethodHelper.HidePassword(jsData);
                LogUtil.HWLogger.UI.InfoFormat("Testing FD connect..., the param is [{0}]", newJsData);
                var fusionDirector = JsonUtil.DeserializeObject<FusionDirectorModel>(jsData);
                var result = HttpHelper.Instance.TestLinkFD(fusionDirector);
                if (result)
                {
                    LogUtil.HWLogger.UI.Info("Testing FD connect successful!");
                    ret.Code = "0";
                    ret.Success = true;
                    ret.Msg = "Testing FD connect successful!";
                }
            }
            catch (BaseException ex)
            {
                LogUtil.HWLogger.UI.Error("Testing FD connect failed: ", ex);
                ret.Code = ex.Code;
                ret.Success = false;
                ret.Msg = "Testing FD connect failed!";
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Testing FD connect failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Success = false;
                ret.Msg = "Testing FD connect failed!";
            }
            return ret;
        }

        private ApiResult Add(object eventData)
        {
            ApiResult ret = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "");
            try
            {
                var jsData = JsonUtil.SerializeObject(eventData);
                var newJsData = CommonBLLMethodHelper.HidePassword(jsData);
                LogUtil.HWLogger.UI.InfoFormat("Saving FD configuration..., the param is [{0}]", newJsData);
                var fusionDirector = JsonUtil.DeserializeObject<FusionDirectorModel>(jsData);
                var result = HttpHelper.Instance.TestLinkFD(fusionDirector);
                if (result)
                {
                    var model = FusionDirectorWorker.Instance.FindByIP(fusionDirector.HostIP);
                    if (model != null)
                    {
                        model.AliasName = fusionDirector.AliasName;
                        model.Port = fusionDirector.Port;
                        model.LoginAccount = fusionDirector.LoginAccount;
                        model.LoginPwd = EncryptUtil.EncryptPwd(fusionDirector.LoginPwd);
                        if (FusionDirectorWorker.Instance.Update(model))
                        {
                            LogUtil.HWLogger.UI.Info("Cover FD configuration successful!");
                            ret.Code = "0";
                            ret.Success = true;
                            ret.Msg = "Cover FD configuration successful!";
                        }
                        else
                        {
                            LogUtil.HWLogger.UI.Info("Cover FD configuration failed!");
                            ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                            ret.Success = true;
                            ret.Msg = "Cover FD configuration failed!";
                        }
                    }
                    else
                    {
                        if (FusionDirectorWorker.Instance.Insert(fusionDirector))
                        {
                            LogUtil.HWLogger.UI.Info("Saving FD configuration successful!");
                            ret.Code = "0";
                            ret.Success = true;
                            ret.Msg = "Saving FD configuration successful!";
                        }
                        else
                        {
                            LogUtil.HWLogger.UI.Info("Saving FD configuration failed!");
                            ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                            ret.Success = true;
                            ret.Msg = "Saving FD configuration failed!";
                        }
                    }
                }
            }
            catch (BaseException ex)
            {
                LogUtil.HWLogger.UI.Error("Testing FD connect failed: ", ex);
                ret.Code = ex.Code;
                ret.Success = false;
                ret.Msg = "Testing FD connect failed!";
                ret.ExceptionMsg = ex.Message;
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Saving FD configuration failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Success = false;
                ret.ExceptionMsg = ex.Message;
            }
            return ret;
        }

        private ApiResult Edit(object eventData)
        {
            ApiResult ret = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "");
            try
            {
                var jsData = JsonUtil.SerializeObject(eventData);
                var newJsData = CommonBLLMethodHelper.HidePassword(jsData);
                LogUtil.HWLogger.UI.InfoFormat("Saving FD configuration..., the param is [{0}]", newJsData);
                var fusionDirector = JsonUtil.DeserializeObject<FusionDirectorModel>(jsData);
                var result = HttpHelper.Instance.TestLinkFD(fusionDirector);
                if (result)
                {
                    var model = FusionDirectorWorker.Instance.FindByIP(fusionDirector.HostIP);
                    model.Port = fusionDirector.Port;
                    model.AliasName = fusionDirector.AliasName;
                    model.LoginAccount = fusionDirector.LoginAccount;
                    model.LoginPwd = EncryptUtil.EncryptPwd(fusionDirector.LoginPwd);
                    if (FusionDirectorWorker.Instance.Update(model))
                    {
                        LogUtil.HWLogger.UI.Info("Edit FD configuration successful!");
                        ret.Code = "0";
                        ret.Success = true;
                        ret.Msg = "Edit FD configuration successful!";
                    }
                    else
                    {
                        LogUtil.HWLogger.UI.Info("Edit FD configuration failed!");
                        ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                        ret.Success = true;
                        ret.Msg = "Edit FD configuration failed!";
                    }
                }
            }
            catch (BaseException ex)
            {
                LogUtil.HWLogger.UI.Error("Edit FD configuration failed: ", ex);
                ret.Code = ex.Code;
                ret.Success = false;
                ret.Msg = "Edit FD configuration failed!";
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Edit FD configuration failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Success = false;
            }
            return ret;
        }

        private ApiResult EditNoCert(object eventData)
        {
            ApiResult ret = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "");
            try
            {
                var jsData = JsonUtil.SerializeObject(eventData);
                var newJsData = CommonBLLMethodHelper.HidePassword(jsData);
                LogUtil.HWLogger.UI.InfoFormat("Saving FD configuration(No Cert)..., the param is [{0}]", newJsData);
                var fusionDirector = JsonUtil.DeserializeObject<FusionDirectorModel>(jsData);
                var model = FusionDirectorWorker.Instance.FindByIP(fusionDirector.HostIP);
                model.AliasName = fusionDirector.AliasName;
                if (model.Port != fusionDirector.Port)
                {
                    fusionDirector.HostIP = model.HostIP;
                    fusionDirector.LoginAccount = model.LoginAccount;
                    fusionDirector.LoginPwd = EncryptUtil.DecryptPwd(model.LoginPwd);
                    var result = HttpHelper.Instance.TestLinkFD(fusionDirector);
                    if (result)
                    {
                        model.Port = fusionDirector.Port;
                        if (FusionDirectorWorker.Instance.Update(model))
                        {
                            LogUtil.HWLogger.UI.Info("Edit FD configuration successful!");
                            ret.Code = "0";
                            ret.Success = true;
                            ret.Msg = "Edit FD configuration successful!";
                        }
                        else
                        {
                            LogUtil.HWLogger.UI.Info("Edit FD configuration failed!");
                            ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                            ret.Success = true;
                            ret.Msg = "Edit FD configuration failed!";
                        }
                    }
                }
                else
                {
                    model.Port = fusionDirector.Port;
                    if (FusionDirectorWorker.Instance.Update(model))
                    {
                        LogUtil.HWLogger.UI.Info("EditNoCert FD configuration(No Cert) successful!");
                        ret.Code = "0";
                        ret.Success = true;
                        ret.Msg = "EditNoCert FD configuration successful!";
                    }
                    else
                    {
                        LogUtil.HWLogger.UI.Info("EditNoCert FD configuration failed!");
                        ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                        ret.Success = true;
                        ret.Msg = "EditNoCert FD configuration failed!";
                    }
                }

            }
            catch (BaseException ex)
            {
                LogUtil.HWLogger.UI.Error("EditNoCert FD configuration failed: ", ex);
                ret.Code = ex.Code;
                ret.Success = false;
                ret.Msg = "EditNoCert FD configuration failed!";
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("EditNoCert FD configuration failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Success = false;
            }
            return ret;
        }

        private ApiResult Delete(object eventData)
        {
            ApiResult ret = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "");
            try
            {
                var jsData = JsonUtil.SerializeObject(eventData);
                LogUtil.HWLogger.UI.InfoFormat("Deleting FD configuration..., the param is [{0}]", jsData);
                var deleteParam = JsonUtil.DeserializeObject<ParamOfDelete>(jsData);
                for (int i = 0; i < deleteParam.FDIDList.Length; i++)
                {
                    FusionDirectorWorker.Instance.DeleteFD(deleteParam.FDIDList[i]);
                }
                LogUtil.HWLogger.UI.Info("Deleting FD configuration successful!");
                ret.Code = "0";
                ret.Success = true;
                ret.Msg = "Deleting FD configuration successful!";
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Deleting FD configuration failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Success = false;
            }
            return ret;
        }

        private ApiResult SaveCert(object eventData)
        {
            ApiResult ret = new ApiResult(ErrorCode.SYS_UNKNOWN_ERR, "");
            try
            {
                LogUtil.HWLogger.UI.InfoFormat("SaveCert , the param is [{0}]", eventData.ToString());
                var cert = new X509Certificate2(Encoding.Default.GetBytes(eventData.ToString()));
                var certs = CertificatesWorker.Instance.GetCerts(); ;
                if (!certs.Contains(cert))
                {
                    var model = new CertificatesModel();
                    model.Content = eventData.ToString();
                    if (!CertificatesWorker.Instance.Insert(model))
                    {
                        CacheHelper.Instance.SetCertList();
                        LogUtil.HWLogger.UI.Info("SaveCert failed!");
                        ret.Code = ErrorCode.CERT_SAVE_FAILED;
                        ret.Success = false;
                        ret.Msg = "SaveCert failed!";
                    }
                }
                LogUtil.HWLogger.UI.Info("SaveCert successful!");
                ret.Code = "0";
                ret.Success = true;
                ret.Msg = "SaveCert successful!";
            }
            catch (CryptographicException ex)
            {
                LogUtil.HWLogger.UI.Error("SaveCert failed: ", ex);
                ret.Code = ErrorCode.CERT_ERROR;
                ret.Success = false;
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("SaveCert failed: ", ex);
                ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                ret.Success = false;
            }
            return ret;
        }

        /// <summary>
        ///请求FD接口 
        /// </summary>
        public CommonResponse RequestFusionDirectorAPI(object eventData)
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

        #region Inner class
        private class ParamOfDelete
        {
            [JsonProperty("ids")]
            public int[] FDIDList { get; set; }
        }
        #endregion
    }
}
