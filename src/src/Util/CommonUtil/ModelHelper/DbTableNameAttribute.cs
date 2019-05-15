using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtil.ModelHelper
{
    /// <summary>
    /// 数据库表名特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableNameAttribute : Attribute
    {
        public DbTableNameAttribute()
        {
        }

        public DbTableNameAttribute(string name)
        {
            _name = name;
        }
        private string _name; public virtual string Name { get { return _name; } set { _name = value; } }
    }
}
