using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    /// <summary>
    /// 用于描述某个类型或成员在界面上显示的字符
    /// </summary>
    public class LabelAttribute : Attribute
    {
        public LabelAttribute(string label)
        {
            Label = label;
        }

        public string Label { get; private set; }
    }
}
