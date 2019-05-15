using CefSharp;
using CommonUtil;
using Huawei.SCCMPlugin.Core;
using Huawei.SCCMPlugin.Core.Exceptions;
using Huawei.SCCMPlugin.Core.Model.Request;
using Huawei.SCCMPlugin.Core.Model.Response;
using Huawei.SCCMPlugin.Core.Workers;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Attributes;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys;
using Huawei.SCCMPlugin.FusionDirector.PluginUI.FDScheme;
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
using System.Windows.Forms;
namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Handlers
{

    /// <summary>
    /// OS镜像管理
    /// </summary>
    [CefURL("huawei/osImage/index.html")]
    [Bound("NetBound")]
    public class OSImageHandler : BaseHandler, IWebHandler
    {
        HttpWebRequest httpWebRequest;

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
                    case "upload":
                        result = Upload(eventData);
                        break;
                    case "cancelUpload":
                        result = CancelUpload();
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



        public CommonResponse CancelUpload()
        {
            var ret = new CommonResponse("0");
            try
            {
                httpWebRequest.Abort();
            }
            catch (Exception ex)
            {

                LogUtil.HWLogger.UI.Error("Exe CancelUpload failed: ", ex);
            }
            return ret;
        }

        ///上传镜像
        /// </summary>
        public CommonResponse Upload(object eventData)
        {
            var ret = new CommonResponse("0");
            var jsData = JsonUtil.SerializeObject(eventData);
            var request = JsonUtil.DeserializeObject<CommonRequest>(jsData);
            if (!File.Exists(request.Data.ToString()))
            {
                ret.Code = ErrorCode.FILE_NOTFOUND;
                LogUtil.HWLogger.UI.Error("Upload OS Image failed: File path error，error path " + request.Data.ToString());
                return ret;
            }
            var fusionDirector = FusionDirectorWorker.Instance.FindByIP(request.Ips[0]);
            if (fusionDirector == null)
            {
                LogUtil.HWLogger.UI.Error("Upload VersionWarehouse failed: not found Fd" + request.Ips[0].ToString());
                ret.Code = ErrorCode.NET_FD_NOFOUND;
                return ret;
            }
            Task.Factory.StartNew(() =>
            {
                try
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fusionDirector.LoginAccount + ":" + fusionDirector.LoginPwd);
                    var authStr = "Basic " + Convert.ToBase64String(bytes);
                    var remoteUrl = "https://" + fusionDirector.HostIP + ":" + fusionDirector.Port + "/redfish/v1" + request.Endpoint;

                    FileStream fs = new FileStream(request.Data.ToString(), FileMode.Open, FileAccess.Read);
                    ret.Description = Math.Round((decimal)fs.Length / (1024 * 1024), 0).ToString();
                    BinaryReader r = new BinaryReader(fs);
                    var fileName = request.Data.ToString().Substring(request.Data.ToString().LastIndexOf('\\') + 1);
                    //时间戳 
                    string strBoundary = "--" + DateTime.Now.Ticks.ToString("x");
                    byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + strBoundary + "\r\n");
                    //请求头部信息 
                    StringBuilder sb = new StringBuilder();
                    sb.Append("--");
                    sb.Append(strBoundary);
                    sb.Append("\r\n");
                    sb.Append("Content-Disposition: form-data; name=\"");
                    sb.Append("tiFile");
                    sb.Append("\"; filename=\""+fileName+"");
                    sb.Append("\";");
                    sb.Append("\r\n");
                    sb.Append("Content-Type: ");
                    sb.Append("application/octet-stream;");
                    sb.Append("\r\n");
                    sb.Append("\r\n");
                    string strPostHeader = sb.ToString();
                    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(strPostHeader);
                    //默认忽略证书
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    //兼容所有ssl协议
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(3072 | 768 | 192 | 48);
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(remoteUrl));
                    httpWebRequest.Method = "POST";
                    ////创建证书文件
                    //X509Certificate objx509 = new X509Certificate(Application.StartupPath + "\\123.cer");
                    ////添加到请求里
                    //request.ClientCertificates.Add(objx509);
                    httpWebRequest.AllowWriteStreamBuffering = false;
                    httpWebRequest.Timeout = 1000*60*60*3;
                    httpWebRequest.Headers.Add("Authorization", authStr);
                    httpWebRequest.ContentType = "multipart/form-data; boundary=" + strBoundary;
                    long length = fs.Length + postHeaderBytes.Length + boundaryBytes.Length;
                    long fileLength = fs.Length;
                    httpWebRequest.ContentLength = length;

                    //每次上传4k 
                    int bufferLength = 4096;
                    byte[] buffer = new byte[bufferLength];
                    //已上传的字节数 
                    long offset = 0;
                    //开始上传时间 
                    DateTime startTime = DateTime.Now;
                    int size = r.Read(buffer, 0, bufferLength);
                    Stream postStream = httpWebRequest.GetRequestStream();

                    //发送请求头部消息 
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    while (size > 0)
                    {
                        postStream.Write(buffer, 0, size);
                        offset += size;
                        Console.WriteLine("已上传：" + (offset * 100.0 / length).ToString("F2") + "%");
                        var Progress = Convert.ToInt32(Math.Round((decimal)offset / length, 2) * 100);
                        FDBrowser.ExecuteScriptAsync("setUploadFileProgress", Progress);
                        Application.DoEvents();
                        size = r.Read(buffer, 0, bufferLength);
                    }
                    //添加尾部的时间戳 
                    postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    postStream.Close();
                    //获取服务器端的响应 
                  
                    WebResponse webRespon = httpWebRequest.GetResponse();
                    Stream requestStream = webRespon.GetResponseStream();
                    StreamReader sr = new StreamReader(requestStream, System.Text.Encoding.UTF8);
                    var content = sr.ReadToEnd();
                    LogUtil.HWLogger.API.Info(content);
                    requestStream.Close();
                    var response = httpWebRequest.GetResponse() as HttpWebResponse;
                  
                    var statusCode = ((int)response.StatusCode).ToString();
                    var task = response.Headers.GetValues("Task");
                    if (task != null)
                    {
                        ret.Headers = new
                        {
                            Task = task[0]
                        };
                    }
                    ret.Data.Add(new Item()
                    {
                        Code = statusCode,
                        Data = JsonConvert.DeserializeObject(content)
                    });
                    if (!statusCode.StartsWith("2"))
                    {
                        throw new Exception(statusCode);
                    }
                }
                catch (BaseException ex)
                {
                    LogUtil.HWLogger.UI.Error("Upload OS Image failed: ", ex);
                    ret.Code = "-1";
                }
                catch (Exception ex)
                {
                    LogUtil.HWLogger.UI.Error("Upload OS Image failed: ", ex);
                    ret.Code = ErrorCode.SYS_UNKNOWN_ERR;
                }

                FDBrowser.ExecuteScriptAsync("uploadFileSuccess", JsonConvert.SerializeObject(ret));
            });
            return ret;

        }
      

    }
}
