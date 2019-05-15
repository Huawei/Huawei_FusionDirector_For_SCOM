using CommonUtil;
using Huawei.SCCMPlugin.Core.Exceptions;
using Huawei.SCCMPlugin.Core.Model.Request;
using Huawei.SCCMPlugin.Core.Model.Response;
using Huawei.SCCMPlugin.Core.Workers;
using Huawei.SCCMPlugin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Huawei.SCCMPlugin.Core
{
    public class HttpHelper
    {

        /// <summary>
        /// 单例
        /// </summary>
        public static HttpHelper Instance
        {
            get { return SingletonProvider<HttpHelper>.Instance; }
        }
        /// <summary>
        /// 定义需要使用的系统枚举。
        /// </summary>
        [Flags]
        private enum MySecurityProtocolType
        {
            //
            // Summary:
            //     Specifies the Secure Socket Layer (SSL) 3.0 security protocol.
            Ssl3 = 48,
            //
            // Summary:
            //     Specifies the Transport Layer Security (TLS) 1.0 security protocol.
            Tls = 192,
            //
            // Summary:
            //     Specifies the Transport Layer Security (TLS) 1.1 security protocol.
            Tls11 = 768,
            //
            // Summary:
            //     Specifies the Transport Layer Security (TLS) 1.2 security protocol.
            Tls12 = 3072
        }

        private void TrustCertificate()
        {
            //默认忽略证书
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //兼容所有ssl协议
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(MySecurityProtocolType.Tls12 | MySecurityProtocolType.Tls11 | MySecurityProtocolType.Tls | MySecurityProtocolType.Ssl3); ;
        }

        /// <summary>
        /// 处理请求FD业务API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CommonResponse CommonHttpHandle(CommonRequest request)
        {
            var res = new CommonResponse("0");
            try
            {
                if (!HttpWhiteListHepler.Instance.Validate(request.Endpoint, request.HttpMethod))
                {
                    res.Code = ErrorCode.SYS_UNKNOWN_ERR;
                    return res;
                }
                var fusionDirector = FusionDirectorWorker.Instance.FindByIP(request.Ips[0]);
                if (fusionDirector == null)
                {
                    res.Code = ErrorCode.NET_FD_NOFOUND;
                    return res;
                }
                /*if (CacheHelper.Instance.CertList == null || CacheHelper.Instance.CertList.Count == 0)
                {
                    CacheHelper.Instance.SetCertList();
                }*/
                TrustCertificate();
                //SetCertificateValidation();
                HttpResponseMessage response = new HttpResponseMessage();
                using (HttpClient httpClient = new HttpClient())
                {
                    LogUtil.HWLogger.API.Info(JsonConvert.SerializeObject(request));
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fusionDirector.LoginAccount + ":" + fusionDirector.LoginPwd);
                    var authStr = "Basic " + Convert.ToBase64String(bytes);
                    httpClient.DefaultRequestHeaders.Add("Authorization", authStr);
                    var remoteUrl = "https://" + fusionDirector.HostIP + ":" + fusionDirector.Port + "/redfish/v1" + request.Endpoint;
                    switch (request.HttpMethod)
                    {
                        case "GET":
                            response = httpClient.GetAsync(remoteUrl).Result;
                            break;
                        case "POST":
                            var content = new StringContent(request.Data.ToString(), Encoding.UTF8, "application/json");
                            response = httpClient.PostAsync(remoteUrl, content).Result;
                            break;
                        case "DELETE":
                            response = httpClient.DeleteAsync(remoteUrl).Result;
                            break;
                        case "PATCH":
                            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), remoteUrl)
                            {
                                Content = new StringContent(request.Data.ToString(), Encoding.UTF8, "application/json")
                            };
                            response = httpClient.SendAsync(requestMessage).Result;
                            break;
                    }
                    var statusCode = ((int)response.StatusCode).ToString();
                    var task = response.Headers.FirstOrDefault(x => x.Key == "Task").Value;
                    if (task != null)
                    {
                        res.Headers = new
                        {
                            Task = task
                        };
                    }
                    res.Description = response.Headers.Date.Value.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    var resultStr = response.Content.ReadAsStringAsync().Result;
                    LogUtil.HWLogger.API.Info("URL:" + remoteUrl + "HttpMethod:" + request.HttpMethod + "Data:" + (request.Data != null ? request.Data.ToString() : "null") + " Api Result:" + resultStr);
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                    {
                        //日期类型默认格式化处理
                        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                        setting.DateFormatString = "yyyy-MM-ddTHH:mm:sszzz";
                        return setting;
                    });
                    res.Data.Add(new Item()
                    {
                        Code = statusCode,
                        Data = JsonConvert.DeserializeObject(resultStr, setting)
                    });
                    if (!statusCode.StartsWith("2"))
                    {
                        throw new Exception(statusCode);
                    }
                }

            }
            catch (CryptographicException ex)
            {
                LogUtil.HWLogger.UI.Error("CommonHttpHandle failed: cert error:", ex);
                res.Code = ErrorCode.CERT_ERROR;
                res.Description = ex.Message;
            }
            catch (AggregateException ex)
            {
                LogUtil.HWLogger.API.Error("CommonHttpHandle Error", ex);
                throw HandleException(ex);
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.API.Error("CommonHttpHandle Error", ex);
                res.Code = "-1";
                res.Description = ex.Message;
            }
            return res;

        }


        /// <summary>
        /// 测试链接FD
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public bool TestLinkFD(FusionDirectorModel fusionDirector)
        {
            try
            {
               /* if (CacheHelper.Instance.CertList == null || CacheHelper.Instance.CertList.Count == 0)
                {
                    CacheHelper.Instance.SetCertList();
                }
                if (CacheHelper.Instance.CertList.Count == 0)
                {
                    throw new BaseException(ErrorCode.CERT_NOTFOUND, "请先上传证书");
                }*/
                TrustCertificate();
                //SetCertificateValidation();
                HttpResponseMessage response = new HttpResponseMessage();
                using (HttpClient httpClient = new HttpClient())
                {
                    //httpClient.Timeout = new TimeSpan(0,0,20);
                    var remoteUrl = "https://" + fusionDirector.HostIP + ":" + fusionDirector.Port + "/redfish/v1/SessionService/Sessions";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fusionDirector.LoginAccount + ":" + fusionDirector.LoginPwd);
                    var authStr = "Basic " + Convert.ToBase64String(bytes);
                    httpClient.DefaultRequestHeaders.Add("Authorization", authStr);
                    response = httpClient.GetAsync(remoteUrl).Result;
                    var statusCode = ((int)response.StatusCode).ToString();
                    if (statusCode.StartsWith("2"))
                    {
                        return true;
                    }
                    else if (statusCode == "430" || statusCode == "400")
                    {
                        throw new BaseException(ErrorCode.SYS_USER_LOGING, "FD用户名或者密码错误");
                    }
                    else if (statusCode == "445")
                    {
                        throw new BaseException(ErrorCode.SYS_USER_LOCK, "FD用户被加锁");
                    }
                    else if (statusCode == "431")
                    {
                        throw new BaseException(ErrorCode.SYS_USER_LOCK, "FD用户类型错误");
                    }
                    else if (statusCode == "429")
                    {
                        throw new BaseException(ErrorCode.HW_LOGIN_AUTH, "系统错误");
                    }
                    else if (statusCode == "432")
                    {
                        throw new BaseException(ErrorCode.HW_LOGIN_AUTH, "会话创建达到上限");
                    }
                    else
                    {
                        throw new BaseException(ErrorCode.SYS_USER_LOGING, "FD用户名或者密码错误");
                    }
                }
            }
            catch (AggregateException ex)
            {
                LogUtil.HWLogger.API.Error("TestLinkFD Error", ex);
                throw HandleException(ex);
            }
            catch (Exception ex)
            {
                LogUtil.HWLogger.API.Error("TestLinkFD Error", ex);
                throw ex;
            }

        }

        public void SetCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
            {
                return CacheHelper.Instance.CertList.Any(x => Equals(x, cert));
            };
            //兼容所有ssl协议
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(MySecurityProtocolType.Tls12 | MySecurityProtocolType.Tls11 | MySecurityProtocolType.Tls | MySecurityProtocolType.Ssl3);
        }

        /// <summary>
        /// 解析复合Exception里的innerException,返回innerException
        /// </summary>
        /// <param name="ae">复合Exception</param>
        /// <returns>inner Exception 列表</returns>
        private List<Exception> GetFlattenAggregateException(AggregateException ae)
        {
            // Initialize a collection to contain the flattened exceptions.
            List<Exception> flattenedExceptions = new List<Exception>();

            // Create a list to remember all aggregates to be flattened, this will be accessed like a FIFO queue
            List<AggregateException> exceptionsToFlatten = new List<AggregateException>();
            exceptionsToFlatten.Add(ae);
            int nDequeueIndex = 0;

            // Continue removing and recursively flattening exceptions, until there are no more.
            while (exceptionsToFlatten.Count > nDequeueIndex)
            {
                // dequeue one from exceptionsToFlatten
                IList<Exception> currentInnerExceptions = exceptionsToFlatten[nDequeueIndex++].InnerExceptions;

                for (int i = 0; i < currentInnerExceptions.Count; i++)
                {
                    Exception currentInnerException = currentInnerExceptions[i];

                    if (currentInnerException == null)
                    {
                        continue;
                    }
                    AggregateException currentInnerAsAggregate = currentInnerException as AggregateException;

                    // If this exception is an aggregate, keep it around for later.  Otherwise,
                    // simply add it to the list of flattened exceptions to be returned.
                    if (currentInnerAsAggregate != null)
                    {
                        exceptionsToFlatten.Add(currentInnerAsAggregate);
                    }
                    else
                    {
                        flattenedExceptions.Add(currentInnerException);
                        flattenedExceptions.AddRange(GetFlattenException(currentInnerException));

                    }
                }
            }
            return flattenedExceptions;
        }
        /// <summary>
        /// 解析Exception里的innerException,返回innerException
        /// </summary>
        /// <param name="se">Exception</param>
        /// <returns>inner Exception 列表</returns>
        private List<Exception> GetFlattenException(Exception se)
        {
            List<Exception> exs = new List<Exception>();

            Exception currentInnerException = se.InnerException;
            if (currentInnerException == null)
                return new List<Exception>();
            else
            {
                exs.Add(currentInnerException);
                exs.AddRange(GetFlattenException(currentInnerException));
            }
            return exs;
        }
        /// <summary>
        /// 解析复合Exception里的innerException,返回解析过的内部Exception，方便前台判断。
        /// </summary>
        /// <param name="ae">复合Exception</param>
        /// <returns> 解析过的内部Exception，方便前台判断。</returns>
        public Exception HandleException(AggregateException ae)
        {
            StringBuilder sb = new StringBuilder();
            LogUtil.HWLogger.API.Error(ae);
            List<Exception> flattenedExceptions = GetFlattenAggregateException(ae);
            foreach (var ex in flattenedExceptions)
            {
                if (ex is WebException)
                {
                    WebException we = (WebException)ex;
                    LogUtil.HWLogger.API.Error(we.Response);
                    if (we.Response != null)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        StreamReader sr = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                        string backstr = sr.ReadToEnd();
                        sr.Close();
                        response.Close();
                        LogUtil.HWLogger.API.Error(backstr);
                    }
                }
                sb.AppendLine(ex.Message);
            }
            LogUtil.HWLogger.API.Error(sb.ToString());
            int errCnt = flattenedExceptions.Count;
            for (int i = errCnt - 1; i >= 0; i--)
            {
                var ex = flattenedExceptions[i];
                if (ex is SocketException)//是否socket连接错误
                {
                    var sex = ex as SocketException;
                    if (sex.NativeErrorCode == 10061)//是否socket拒绝连接
                    {
                        throw new BaseException(ErrorCode.NET_SOCKET_REFUSED, ex.Message);
                    }
                    else
                    {
                        throw new BaseException(ErrorCode.NET_SOCKET_UNKNOWN, ex.Message);
                    }
                }
                else if (ex is AuthenticationException)
                {
                    throw new BaseException(ErrorCode.CERT_ERROR, ex.Message);
                }
                else if (ex is CryptographicException)
                {
                    throw new BaseException(ErrorCode.CERT_ERROR, ex.Message);
                }
                else if (ex is WebException)
                {
                    throw new BaseException(ErrorCode.SYS_UNKNOWN_ERR, ex.Message);
                }
            }
            throw new BaseException(ErrorCode.SYS_UNKNOWN_ERR, sb.ToString());
        }
    }
}
