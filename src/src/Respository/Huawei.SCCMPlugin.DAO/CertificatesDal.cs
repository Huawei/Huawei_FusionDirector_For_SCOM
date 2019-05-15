using CommonUtil;
using Huawei.SCCMPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.DAO
{
    public class CertificatesDal : BaseRepository<CertificatesModel>, ICertificatesDal
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static CertificatesDal Instance
        {
            get { return SingletonProvider<CertificatesDal>.Instance; }
        }
    }
}
