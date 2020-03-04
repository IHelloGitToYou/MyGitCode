using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    /// <summary>
    /// 定义是下拉框, 为了增加 xxxx_display 字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IsComboboxAttribute : Attribute
    {
        public string DisplayField { get; set; } = "Name";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="P_DisplayField">前台显示字段,空使用默认Name</param>
        /// <param name="TableName">关联表名</param>
        /// <param name="ShortName">关联表简写</param>
        public IsComboboxAttribute(string P_DisplayField , string TableName, string ShortName)
        {
            if (P_DisplayField.IsNotEmpty())
            {
                this.DisplayField = P_DisplayField;
            }
            this.TableName = TableName;
            this.ShortName = ShortName;
        }

        public IsComboboxAttribute(string P_DisplayField, string TableName, string ShortName, string P_RightEqualFieldName)
        {
            if (P_DisplayField.IsNotEmpty())
            {
                this.DisplayField = P_DisplayField;
            }
            this.TableName = TableName;
            this.ShortName = ShortName;
            this.RightEqualFieldName = P_RightEqualFieldName;
        }
        
        /// <summary>
        /// 外键表名
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// 表 简写
        /// </summary>
        public string ShortName { get; private set; }

        public string RightEqualFieldName { get; private set; }
    }
}
