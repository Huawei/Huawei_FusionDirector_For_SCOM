using Huawei.SCCMPlugin.Models;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys
{
    /// <summary>
    /// Ajax请求返回结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 成功 失败
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// Exception消息.
        /// </summary>
        public string ExceptionMsg { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public ApiResult(bool success, string errCode, string msg, string exmsg)
        {
            Code = errCode;
            Success = success;
            Msg = msg;
            ExceptionMsg = exmsg;
        }

        public ApiResult(string msg) :
            this(true,ErrorCode.SYS_UNKNOWN_ERR, msg, "")
        { }

        public ApiResult(string errCode, string exmsg)
            : this(false, errCode, "", "")
        { }
    }

    /// <summary>
    /// 请求接口返回结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T>
    {
        /// <summary>
        /// 成功 失败
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// Exception消息.
        /// </summary>
        public string ExceptionMsg { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public ApiResult(bool success, string errCode, string msg, string exmsg, T data)
        {
            Code = errCode;
            Success = success;
            Msg = msg;
            ExceptionMsg = exmsg;
            Data = data;
        }

        public ApiResult(string msg, T data) :
            this(true, ErrorCode.SYS_UNKNOWN_ERR, msg, "", data)
        { }

        public ApiResult(string errCode, string exmsg) :
           this(false, errCode, "", exmsg, default(T))
        { }

        public ApiResult(T data)
            : this(true, ErrorCode.SYS_UNKNOWN_ERR, "", "", data)
        { }

    }

    public class ApiListResult<T>
    {
        /// <summary>
        /// 成功 失败
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>The error code.</value>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// Exception消息.
        /// </summary>
        public string ExceptionMsg { get; set; }


        [JsonProperty("totalNum")]
        public int TotalNum { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; }

        public ApiListResult(bool success, string errCode, string msg, string exmsg, IEnumerable<T> data)
        {
            Code = errCode;
            Success = success;
            Msg = msg;
            ExceptionMsg = exmsg;
            Data = data;
        }

        public ApiListResult(string msg, IEnumerable<T> data) :
            this(true, ErrorCode.SYS_UNKNOWN_ERR, msg, "", data)
        { }

        public ApiListResult(string errCode, string exmsg) :
            this(false, errCode, "", exmsg, default(IEnumerable<T>))
        { }

        public ApiListResult(IEnumerable<T> data)
            : this(true, ErrorCode.SYS_UNKNOWN_ERR, "", "", data)
        { }

    }
}