using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    /// <summary>
    /// 用于设置字符串属性 值必填
    /// </summary>
    [AttributeUsage(
           AttributeTargets.Property,
           AllowMultiple = false,
           Inherited = false)]
    public class RequireAttribute : Attribute
    {
        public RequireAttribute()
        {
        }
    }
}
