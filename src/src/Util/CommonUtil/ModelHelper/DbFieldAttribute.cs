using System;

namespace CommonUtil.ModelHelper
{
    /// <summary>
    /// 是否映射到数据库特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        public DbFieldAttribute()
        {
        }
        public DbFieldAttribute(bool isDbField)
        {
            _isDbField = isDbField;
        }
        private bool _isDbField;
        public virtual bool IsDbField { get { return _isDbField; } set { _isDbField = value; } }
    }
}
