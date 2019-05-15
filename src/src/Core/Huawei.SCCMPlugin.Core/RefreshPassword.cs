using CommonUtil;
using Huawei.SCCMPlugin.Core.Workers;
using Huawei.SCCMPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.Core
{
    public class RefreshPassword
    {
        private static object _lockRefreshPwds = new object();

        /// <summary>
        /// 隔天更新密钥。
        /// 1. 当启动时更新。
        /// 2. 启动后，距离上次更新超过一天时更新。
        /// </summary>
        public void RefreshPwdsThirtyDay()
        {
            DateTime now = DateTime.Now;
            TimeSpan d = now.Subtract(EncryptUtil.GetLatestKeyChangeDate());
            if (d.Days > 30)
            {
                RefreshPwds();
            }
        }
        /// <summary>
        /// 检查并升级密钥
        /// </summary>
        public void CheckAndUpgradeKey()
        {
            if (!EncryptUtil.IsCompatibleVersion())
            {
                RefreshPwds();
            }
            else
            {
                RefreshPwdsThirtyDay();
            }
        }
        /// <summary>
        /// 刷新密码。时机是每次启动时，这里会加在这个单例的初始化。
        /// 重置密钥，并且更新密码。
        /// 规则1：密钥须支持可更新，并明确更新周期，在一次性可编程的芯片中保存的密钥除外
        /// 说明：工作密钥及密钥加密密钥在使用过程中，都应保证其可以更新。对于根密钥暂不要求必须支持可更新。
        /// </summary>
        public void RefreshPwds()
        {
            LogUtil.HWLogger.DEFAULT.InfoFormat("Refresh password with encryption...");
            lock (_lockRefreshPwds)
            {
                using (var mutex = new System.Threading.Mutex(false, "huawei.sccmplugin.engine"))
                {
                    if (mutex.WaitOne(TimeSpan.FromSeconds(60), false))
                    {
                        string oldMainKey = "";
                        //2017-10-11 检查是否需要升级的密钥。
                        if (!EncryptUtil.IsCompatibleVersion())
                        {
                            oldMainKey = EncryptUtil.GetMainKey1060();
                            LogUtil.HWLogger.DEFAULT.InfoFormat("oldMainKey:{0}", oldMainKey);
                            if (string.IsNullOrEmpty(oldMainKey)) return;
                            EncryptUtil.ClearAndUpgradeKey();
                        }
                        else
                        {
                            //旧的key
                            oldMainKey = EncryptUtil.GetMainKeyWithoutInit();
                            if (string.IsNullOrEmpty(oldMainKey)) return;
                            //重新初始化主密钥。
                            EncryptUtil.InitMainKey();
                        }
                        string newMainKey = EncryptUtil.GetMainKeyFromPath();
                        // LogUtil.HWLogger.DEFAULT.InfoFormat("Change key,oldMainKey={1},newMainKey={1}",oldMainKey,newMainKey);
                        //遍历所有session.
                        var hostlist = FusionDirectorWorker.Instance.GetList();
                        foreach (FusionDirectorModel fd in hostlist)
                        {
                            string pwd = EncryptUtil.DecryptWithKey(oldMainKey, fd.LoginPwd);
                            string enPwd = EncryptUtil.EncryptWithKey(newMainKey, pwd);
                            fd.LoginPwd = enPwd;
                            FusionDirectorWorker.Instance.Update(fd);
                        }

                    }
                }
            }
            LogUtil.HWLogger.DEFAULT.InfoFormat("Refresh password with encryption successful!");
        }
    }
}
