using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.Models
{
    public class ErrorCode 
    {
        /// <summary>
        /// 华为鉴权失败，返回正文如下
        /// {"description":"Auth failed","data":null,"code":1204}
        /// </summary>
        public const string HW_LOGIN_AUTH = "1204";
        
        /// <summary>
        ///  系统未知错误，API使用
        /// </summary>
        public const string SYS_UNKNOWN_ERR = "-99999"; //未知错误

        //系统内部错误 9xxxx
        /// <summary>
        /// 请先初始化
        /// </summary>
        public const string SYS_UNINIT = "-90001";

        /// <summary>
        /// 请先配置FD
        /// </summary>
        public const string SYS_NO_FD = "-90002";

      

        /// <summary>
        /// FD用户名或者密码错误
        /// </summary>
        public const string SYS_USER_LOGING = "-90005";

        /// <summary>
        /// FD用户被加锁
        /// </summary>
        public const string SYS_USER_LOCK = "-90006";

        //网络错误  8xxxx
        /// <summary>
        /// String.Format("Accessing[{0}] , Error occurred: {1}", url, hrm.StatusCode)// 访问FDhttp错误。
        /// </summary>
        public const string NET_EISGHT_HTTP = "-80009";
        /// <summary>
        /// No connection could be made because the target machine actively refused.
        /// </summary>
        public const string NET_SOCKET_REFUSED = "-80010";
        /// <summary>
        /// Network connection error.
        /// </summary>
        public const string NET_SOCKET_UNKNOWN = "-81001";
        /// <summary>
        /// 没有找到对应的FD.
        /// </summary>
        public const string NET_FD_NOFOUND = "-80011";

        //数据库错误 7xxxx
        /// <summary>
        /// 数据库没有找到对应的数据。[{0}]
        /// </summary>
        public const string DB_NOTFOUND = "-70001";

        /// <summary>
        /// 未找到文件
        /// </summary>
        public const string FILE_NOTFOUND = "-40404";

        /// <summary>
        /// 请先上传证书
        /// </summary>
        public const string CERT_NOTFOUND = "-80002";

        /// <summary>
        /// 上传证书失败
        /// </summary>
        public const string CERT_SAVE_FAILED = "-30000";

        /// <summary>
        /// 证书错误
        /// </summary>
        public const string CERT_ERROR = "-90008";




    }
}
