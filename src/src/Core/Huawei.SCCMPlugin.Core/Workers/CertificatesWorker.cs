using CommonUtil;
using Huawei.SCCMPlugin.DAO;
using Huawei.SCCMPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Huawei.SCCMPlugin.Core.Workers
{
    public class CertificatesWorker
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static CertificatesWorker Instance
        {
            get { return SingletonProvider<CertificatesWorker>.Instance; }
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(CertificatesModel entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.UpdateTime = DateTime.Now;
            return CertificatesDal.Instance.InsertEntity(entity) > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(CertificatesModel entity)
        {
            entity.UpdateTime = DateTime.Now;
            return CertificatesDal.Instance.UpdateEntity(entity) > 0;
        }

        /// <summary>
        /// 获取全部数据
        /// </summary>
        /// <returns></returns>
        public List<CertificatesModel> GetList()
        {
            return CertificatesDal.Instance.GetList().ToList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fdId">FusionDirector Id</param>
        public bool Delete(int id)
        {
            return CertificatesDal.Instance.DeleteEntityById(id) > 0;
        }

        /// <summary>
        /// 获取证书列表
        /// </summary>
        /// <returns></returns>
        public List<X509Certificate2> GetCerts()
        {
            var list = GetList();
            var certs = new List<X509Certificate2>();
            list.ForEach(x =>
            {
                try
                {
                    certs.Add(new X509Certificate2(Encoding.Default.GetBytes(x.Content)));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
            return certs;
        }
    }
}
