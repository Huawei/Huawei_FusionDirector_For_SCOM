using System;

namespace CommonUtil.ModelHelper
{
    /// <summary>
    /// 根据实体上的DbTableNameAttribute获取数据库表名
    /// </summary>
    public static class TableConvention
    {

        public static string Resolve(Type t)
        {
            string _tablename = "";
            DbTableNameAttribute tableName;
            var name = t.Name;
            foreach (Attribute attr in t.GetCustomAttributes(true))
            {
                tableName = attr as DbTableNameAttribute;
                if (tableName != null)
                    _tablename = tableName.Name;
            }
            return _tablename;
        }

        public static string Resolve(object o)
        {
            return Resolve(o.GetType());
        }
    }
}
