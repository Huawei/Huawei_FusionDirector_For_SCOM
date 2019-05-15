using System;

namespace CommonUtil.ModelHelper
{

    /// <summary>
    /// 实体属性映射数据库表字段特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumnAttribute : Attribute
    {
        public DbColumnAttribute()
        {
        }
        public DbColumnAttribute(string columnName)
        {
            _columnName = columnName;
        }
        private string _columnName;
        public virtual string ColumnName { get { return _columnName; } set { _columnName = value; } }
    }
}
