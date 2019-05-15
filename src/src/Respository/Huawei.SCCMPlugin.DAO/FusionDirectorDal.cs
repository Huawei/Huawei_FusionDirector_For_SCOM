using Huawei.SCCMPlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtil;

namespace Huawei.SCCMPlugin.DAO
{
   public class FusionDirectorDal : BaseRepository<FusionDirectorModel>, IFusionDirectorDal
    {
        /// <summary>
        /// 单例
        /// </summary>
        public static FusionDirectorDal Instance
        {
            get { return SingletonProvider<FusionDirectorDal>.Instance; }
        }

        /// <summary>
        /// 根据IP查找FD实体。
        /// </summary>
        /// <param name="fdIP">IP地址</param>
        /// <returns></returns>
        public FusionDirectorModel FindByIP(string fdIP)
        {
            return DBUtility.Context.Sql("select * from  FusionDirectors where HOST_IP=@0", fdIP).QuerySingle<FusionDirectorModel>();
        }

        /// <summary>
        /// 删除FD
        /// </summary>
        /// <param name="eSightId">FusionDirector Id</param>
        public int DeleteFD(int fdId)
        {
          return  ExecuteSql("delete from FusionDirectors where ID=" + fdId);
        }
    }
}
