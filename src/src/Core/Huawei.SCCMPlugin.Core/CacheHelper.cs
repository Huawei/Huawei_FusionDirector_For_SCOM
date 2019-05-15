using CommonUtil;
using Huawei.SCCMPlugin.Core.Workers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Huawei.SCCMPlugin.Core
{
    public class CacheHelper
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static CacheHelper Instance
        {
            get { return SingletonProvider<CacheHelper>.Instance; }
        }

        public List<X509Certificate2> CertList { get; set; }

        public void SetCertList()
        {
            this.CertList = CertificatesWorker.Instance.GetCerts();
        }

       
    }
}
