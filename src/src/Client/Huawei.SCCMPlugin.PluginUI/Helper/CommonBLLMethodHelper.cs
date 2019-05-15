using CommonUtil;
using Huawei.SCCMPlugin.Core.Workers;
using Huawei.SCCMPlugin.Models;

using Huawei.SCCMPlugin.FusionDirector.PluginUI.Entitys;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Huawei.SCCMPlugin.FusionDirector.PluginUI.Helper
{
    public sealed class CommonBLLMethodHelper
    {
        /// <summary>
        /// 加载FD数据，用于UI FD下拉框
        /// </summary>
        /// <returns></returns>
        public static ApiListResult<FusionDirectorModel> LoadFDList()
        {
            var ret = new ApiListResult<FusionDirectorModel>(false,ErrorCode.SYS_UNKNOWN_ERR, "", "", null);
            LogUtil.HWLogger.UI.InfoFormat("Loading FD list...");
           
            List<FusionDirectorModel> list = FusionDirectorWorker.Instance.GetList();
            if (list.Count == 0)
            {
                ret.Code = ErrorCode.SYS_NO_FD;
                ret.Success = false;
                ret.Data = list;
            }
            else
            {
                list.ForEach(x=> {
                    x.LoginPwd = "";
                });
                ret.Code = "0";
                ret.Success = true;
                ret.Data = list;
            }
            LogUtil.HWLogger.UI.InfoFormat("Loading FD list completed, the ret is [{0}]", JsonUtil.SerializeObject(ret));
            return ret;
        }


        /// <summary>
        /// 隐藏Json字符串中的密码
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static string HidePassword(string jsonData)
        {
            string newJsonData = null;
            string replacement = "\"${str}\":\"********\"";
            string pattern1 = "\"(?<str>([A-Za-z0-9_]*)password)\":\"(.*?)\"";
            newJsonData = Regex.Replace(jsonData, pattern1, replacement, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            string pattern2 = "\"(?<str>([A-Za-z0-9_]*)pwd)\":\"(.*?)\"";
            newJsonData = Regex.Replace(newJsonData, pattern2, replacement, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return newJsonData;
        }
    }
}
