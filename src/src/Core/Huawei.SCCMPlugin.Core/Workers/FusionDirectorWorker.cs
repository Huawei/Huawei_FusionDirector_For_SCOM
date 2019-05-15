using CommonUtil;
using Huawei.SCCMPlugin.DAO;
using Huawei.SCCMPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.Core.Workers
{
    public class FusionDirectorWorker
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static FusionDirectorWorker Instance
        {
            get { return SingletonProvider<FusionDirectorWorker>.Instance; }
        }

        /// <summary>
        /// 插入FD
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(FusionDirectorModel entity)
        {
            entity.CreateTime = DateTime.Now;
            entity.LastModifyTime = DateTime.Now;
            entity.LoginPwd = EncryptUtil.EncryptPwd(entity.LoginPwd);
            return FusionDirectorDal.Instance.InsertEntity(entity) > 0;
        }

        /// <summary>
        /// 更新FD
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(FusionDirectorModel entity)
        {
            entity.LastModifyTime = DateTime.Now;
            return FusionDirectorDal.Instance.UpdateEntity(entity) > 0;
        }

        /// <summary>
        /// 获取全部FD数据
        /// </summary>
        /// <returns></returns>
        public List<FusionDirectorModel> GetList()
        {
            return FusionDirectorDal.Instance.GetList().ToList();
        }

        /// <summary>
        /// 获取FD列表
        /// </summary>
        /// <param name="totalRows">总行数</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="pageNum">页码</param>
        /// <param name="orderStr">排序字段</param>
        /// <param name="desc">排序规则</param>
        /// <returns></returns>
        public List<FusionDirectorModel> GetList(out int totalRows, string condition = "1=1 ", int pageSize = 20, int pageNum = 1, string orderStr = "id", bool desc = false)
        {
            return FusionDirectorDal.Instance.GetList(out totalRows, condition,pageSize,pageNum, orderStr, desc).ToList();
        }

        /// <summary>
        /// 根据IP查找FD实体。
        /// </summary>
        /// <param name="fdIP">IP地址</param>
        /// <returns></returns>
        public FusionDirectorModel FindByIP(string fdIP)
        {
            var model = FusionDirectorDal.Instance.FindByIP(fdIP);
            if (model != null)
            {
                model.LoginPwd = EncryptUtil.DecryptPwd(model.LoginPwd);
            }
            return model;
        }

        /// <summary>
        /// 删除FD
        /// </summary>
        /// <param name="fdId">FusionDirector Id</param>
        public bool DeleteFD(int fdId)
        {
            return FusionDirectorDal.Instance.DeleteFD(fdId) > 0;
        }
    }
}
