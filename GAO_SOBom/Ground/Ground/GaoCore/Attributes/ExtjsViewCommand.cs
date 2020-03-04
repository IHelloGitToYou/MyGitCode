using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.Attributes
{
    /// <summary>
    /// Extjs 按钮
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class ,
        AllowMultiple = false,
        Inherited = true)]
    public class ViewCommandAttribute : Attribute
    {
        public ViewCommandAttribute()
        {
        }
    }
}
