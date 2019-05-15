using CefSharp;
using CommonUtil;
using Huawei.SCCMPlugin.Core.Exceptions;
using Huawei.SCCMPlugin.Core.Model.Request;
using Huawei.SCCMPlugin.Core.Model.Response;
using Huawei.SCCMPlugin.Core.Workers;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Helper;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Interface;
using Huawei.SCCMPlugin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{
    /// <summary>
    /// 升级仓库
    /// </summary>
    [CefURL("huawei/upgradePackageWarehouse/index.html")]
    [Bound("NetBound")]
    public class VersionWarehouseHandler : BaseHandler, IWebHandler
    {
        static event EventHandler<AggregateExceptionArgs> AggregateExceptionCatched1;
        static event EventHandler<AggregateExceptionArgs> AggregateExceptionCatched2;

        CancellationTokenSource TokenSource1 = new CancellationTokenSource();
        CancellationTokenSource TokenSource2 = new CancellationTokenSource();

        public override string Execute(string eventName, object eventData)
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
                    case "uploadPackage":
                        result = UploadPackage(eventData);
                        break;
                    case "uploadSignature":
                        result = UploadSignature(eventData);
                        break;
                    case "cancelPackageUpload":
                        result = CancelPackageUpload();
                        break;
                    case "cancelSignatureUpload":
                        result = CancelSignatureUpload();
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
            return "{code:" + ErrorCode.SYS_UNKNOWN_ERR + ",data:[]}";
        }


        /// <summary>
        /// 取消上传升级包
        /// </summary>
        /// <returns></returns>
        public CommonResponse CancelPackageUpload()
        {
            var ret = new CommonResponse("0");
            try
            {
                LogUtil.HWLogger.UI.Error("Exe CancelPackageUpload");
                TokenSource1.Cancel();
            }
            catch (Exception ex)
            {

                LogUtil.HWLogger.UI.Error("Exe CancelPackageUpload failed: ", ex);
            }
            return ret;
        }

        /// <summary>
        /// 取消上传签名
        /// </summary>
        /// <returns></returns>
        public CommonResponse CancelSignatureUpload()
        {
            var ret = new CommonResponse("0");
            try
            {
                LogUtil.HWLogger.UI.Error("Exe CancelSignatureUpload");
                TokenSource2.Cancel();
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.UI.Error("Exe CancelSignatureUpload failed: ", ex);
            }
            return ret;
        }

        ///上传升级包
        /// </summary>
        public CommonResponse UploadPackage(object eventData)
        {
            var ret = new CommonResponse("0");
            var jsData = JsonUtil.SerializeObject(eventData);
            var request = JsonUtil.DeserializeObject<CommonRequest>(jsData);
            if (!File.Exists(request.Data.ToString()))
            {
                ret.Code = ErrorCode.FILE_NOTFOUND;
                LogUtil.HWLogger.UI.Error("Upload Package failed: File path error，error path " + request.Data.ToString());
                return ret;
            }
            var fusionDirector = FusionDirectorWorker.Instance.FindByIP(request.Ips[0]);
            if (fusionDirector == null)
            {
                ret.Code = ErrorCode.NET_FD_NOFOUND;
                LogUtil.HWLogger.UI.Error("Upload Package failed: not found Fd" + request.Ips[0].ToString());
                return ret;
            }
            AggregateExceptionCatched1 += new EventHandler<AggregateExceptionArgs>(Program_AggregateExceptionCatched1);
            TokenSource1 = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fusionDirector.LoginAccount + ":" + fusionDirector.LoginPwd);
                    var authStr = "Basic " + Convert.ToBase64String(bytes);
                    var remoteUrl = "https://" + fusionDirector.HostIP + ":" + fusionDirector.Port + "/redfish/v1" + request.Endpoint;

                    //默认忽略证书
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    //兼容所有ssl协议
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(3072 | 768 | 192 | 48);
                    var file = File.Open(request.Data.ToString(), FileMode.Open, FileAccess.Read);
                    ret.Description = Math.Round((decimal)file.Length / (1024 * 1024), 0).ToString();
                    var fileContent = new StreamContent(file)
                    {
                        Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue("application/octet-stream") }
                    };
                    var formDataContent = new MultipartFormDataContent();
                    var fileName = file.Name.Substring(file.Name.LastIndexOf('\\') + 1);
                    formDataContent.Add(fileContent, "tiFile", fileName);
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, remoteUrl);
                    var progressContent = new ProgressableStreamContent(formDataContent, 4096 * 100, (sent, total) =>
                    {
                        try
                        {
                            Console.WriteLine("Uploading {0}/{1}", sent, total);
                            var Progress = Convert.ToInt32(Math.Round((decimal)sent / total, 2) * 100);
                            FDBrowser.ExecuteScriptAsync("setUploadPackageProgress", Progress);//设置升级包上传进度  setUploadPackageProgress是UI js方法名称 Progress是进度 0-100
                        }
                        catch (Exception ex)
                        {
                            AggregateExceptionArgs args = new AggregateExceptionArgs()
                            {
                                AggregateException = new AggregateException(ex)
                            };
                            //使用主线程委托代理，处理子线程 异常
                            //这种方式没有阻塞 主线程或其他线程
                            AggregateExceptionCatched1?.Invoke(null, args);
                            throw ex;
                        }

                    });
                    httpRequestMessage.Content = progressContent;
                    httpRequestMessage.Headers.Add("Authorization", authStr);
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var httpResponse = httpClient.SendAsync(httpRequestMessage, TokenSource1.Token).Result;
                        var statusCode = ((int)httpResponse.StatusCode).ToString();
                        var resultStr = httpResponse.Content.ReadAsStringAsync().Result;
                        LogUtil.HWLogger.API.Info("URL:" + remoteUrl + " Api Result:" + resultStr);
                        ret.Data.Add(new Item()
                        {
                            Code = statusCode,
                            Data = JsonConvert.DeserializeObject(resultStr)
                        });
                        if (!statusCode.StartsWith("2"))
                        {
                            throw new BaseException(statusCode, "");
                        }
                    }
                }
                catch (BaseException ex)
                {
                    LogUtil.HWLogger.UI.Error("Upload Package failed: ", ex);
                    ret.Code = ex.Code;
                }
                catch (Exception ex)
                {
                    LogUtil.HWLogger.UI.Error("Upload Package failed: ", ex);
                    ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                    AggregateExceptionArgs args = new AggregateExceptionArgs()
                    {
                        AggregateException = new AggregateException(ex)
                    };
                    //使用主线程委托代理，处理子线程 异常
                    //这种方式没有阻塞 主线程或其他线程
                    AggregateExceptionCatched1?.Invoke(null, args);
                }

                FDBrowser.ExecuteScriptAsync("uploadPackageFileSuccess", JsonConvert.SerializeObject(ret));//执行升级包上传成功方法  uploadPackageFileSuccess是UI js升级包上传成功方法
            });
            return ret;

        }

        ///上传签名
        /// </summary>
        public CommonResponse UploadSignature(object eventData)
        {
            var ret = new CommonResponse("0");
            var jsData = JsonUtil.SerializeObject(eventData);
            var request = JsonUtil.DeserializeObject<CommonRequest>(jsData);
            if (!File.Exists(request.Data.ToString()))
            {
                ret.Code = ErrorCode.FILE_NOTFOUND;
                LogUtil.HWLogger.UI.Error("Upload Signatur failed: File path error，error path " + request.Data.ToString());
                return ret;
            }
            var fusionDirector = FusionDirectorWorker.Instance.FindByIP(request.Ips[0]);
            if (fusionDirector == null)
            {
                ret.Code = ErrorCode.NET_FD_NOFOUND;
                LogUtil.HWLogger.UI.Error("Upload Signatur failed: not found Fd" + request.Ips[0].ToString());
                return ret;
            }
            AggregateExceptionCatched2 += new EventHandler<AggregateExceptionArgs>(Program_AggregateExceptionCatched2);
            TokenSource2 = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                try
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fusionDirector.LoginAccount + ":" + fusionDirector.LoginPwd);
                    var authStr = "Basic " + Convert.ToBase64String(bytes);
                    var remoteUrl = "https://" + fusionDirector.HostIP + ":" + fusionDirector.Port + "/redfish/v1" + request.Endpoint;

                    //默认忽略证书
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    //兼容所有ssl协议
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(3072 | 768 | 192 | 48);
                    var file = File.Open(request.Data.ToString(), FileMode.Open, FileAccess.Read);
                    ret.Description = Math.Round((decimal)file.Length / (1024 * 1024), 0).ToString();
                    var fileContent = new StreamContent(file)
                    {
                        Headers = { ContentLength = file.Length, ContentType = new MediaTypeHeaderValue("application/octet-stream") }
                    };
                    var formDataContent = new MultipartFormDataContent();
                    var fileName = file.Name.Substring(file.Name.LastIndexOf('\\') + 1);
                    formDataContent.Add(fileContent, "tiFile", fileName);
                    var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, remoteUrl);
                    var progressContent = new ProgressableStreamContent(formDataContent, 4096 * 100, (sent, total) =>
                    {
                        try
                        {
                            Console.WriteLine("Uploading {0}/{1}", sent, total);
                            var Progress = Convert.ToInt32(Math.Round((decimal)sent / total, 2) * 100);
                            FDBrowser.ExecuteScriptAsync("setUploadSignaturProgress", Progress);//设置升级包上传进度  setUploadSignaturProgress是UI js方法名称 Progress是进度 0-100
                        }
                        catch (Exception ex)
                        {
                            AggregateExceptionArgs args = new AggregateExceptionArgs()
                            {
                                AggregateException = new AggregateException(ex)
                            };
                            //使用主线程委托代理，处理子线程 异常
                            //这种方式没有阻塞 主线程或其他线程
                            AggregateExceptionCatched1?.Invoke(null, args);
                            throw ex;
                        }

                    });
                    httpRequestMessage.Content = progressContent;
                    httpRequestMessage.Headers.Add("Authorization", authStr);
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var httpResponse = httpClient.SendAsync(httpRequestMessage, TokenSource2.Token).Result;
                        var statusCode = ((int)httpResponse.StatusCode).ToString();
                        var resultStr = httpResponse.Content.ReadAsStringAsync().Result;
                        LogUtil.HWLogger.API.Info("URL:" + remoteUrl + " Api Result:" + resultStr);
                        ret.Data.Add(new Item()
                        {
                            Code = statusCode,
                            Data = JsonConvert.DeserializeObject(resultStr)
                        });
                        if (!statusCode.StartsWith("2"))
                        {
                            throw new BaseException(statusCode, "");
                        }
                    }
                }
                catch (BaseException ex)
                {
                    LogUtil.HWLogger.UI.Error("Upload Signatur failed: ", ex);
                    ret.Code = ex.Code;
                }
                catch (Exception ex)
                {
                    LogUtil.HWLogger.UI.Error("Upload Signatur failed: ", ex);
                    ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                    AggregateExceptionArgs args = new AggregateExceptionArgs()
                    {
                        AggregateException = new AggregateException(ex)
                    };
                    //使用主线程委托代理，处理子线程 异常
                    //这种方式没有阻塞 主线程或其他线程
                    AggregateExceptionCatched2?.Invoke(null, args);
                }

                FDBrowser.ExecuteScriptAsync("uploadSignaturFileSuccess", JsonConvert.SerializeObject(ret));//执行升级包上传成功方法  uploadSignaturFileSuccess是UI js升级包上传成功方法
            });
            return ret;

        }


        void Program_AggregateExceptionCatched1(object sender, AggregateExceptionArgs e)
        {
            foreach (var item in e.AggregateException.InnerExceptions)
            {
                LogUtil.HWLogger.UI.ErrorFormat("异常类型：{0}{1}来自：{2}{3}异常内容：{4}",
                    item.GetType(), Environment.NewLine, item.Source,
                    Environment.NewLine, item.Message);
            }
        }

        void Program_AggregateExceptionCatched2(object sender, AggregateExceptionArgs e)
        {
            foreach (var item in e.AggregateException.InnerExceptions)
            {
                LogUtil.HWLogger.UI.ErrorFormat("异常类型：{0}{1}来自：{2}{3}异常内容：{4}",
                    item.GetType(), Environment.NewLine, item.Source,
                    Environment.NewLine, item.Message);
            }
        }
    }
}
