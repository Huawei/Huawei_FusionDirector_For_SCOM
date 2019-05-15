using Huawei.SCCMPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huawei.SCCMPlugin.DAO
{
   
     /// <summary>
    /// FD数据库管理类
    /// </summary>
    public interface IFusionDirectorDal : IBaseRepository<FusionDirectorModel>
    {
        /// <summary>
        /// 根据IP查找FD实体。
        /// </summary>
        /// <param name="fdIP">IP地址</param>
        /// <returns></returns>
        FusionDirectorModel FindByIP(string fdIP);
        /// <summary>
        /// 删除FD
        /// </summary>
        /// <param name="fdId">FusionDirector Id</param>
        int DeleteFD(int fdId);
    }
}
