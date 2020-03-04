using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [AttributeUsage(
        AttributeTargets.Field |
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    /// <summary>
    /// 视图属性 用于返回数据时带回 外连值
    /// </summary>
    public class ViewAsPropertyAttribute : Attribute
    {
        public ViewAsPropertyAttribute(string LeftJoinFieldName, string TableName, string ShortName, string TakeFieldName)
        {
            this.LeftJoinFieldName = LeftJoinFieldName;
            this.TableName = TableName;
            this.ShortName = ShortName;
            this.TakeFieldName = TakeFieldName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LeftJoinFieldName"></param>
        /// <param name="TableName"></param>
        /// <param name="ShortName"></param>
        /// <param name="TakeFieldName"></param>
        /// <param name="AsName">别名</param>
        public ViewAsPropertyAttribute(string LeftJoinFieldName, string TableName, string ShortName, string TakeFieldName, string AsName, string P_RightEqualFieldName  ="")
        {
            this.LeftJoinFieldName = LeftJoinFieldName;
            this.TableName = TableName;
            this.ShortName = ShortName;
            this.TakeFieldName = TakeFieldName;
            this.TakeFieldAsName = AsName;
            this.RightEqualFieldName = P_RightEqualFieldName;
        }

        /// <summary>
        /// 本表的字段
        /// </summary>
        public string LeftJoinFieldName { get; private set; }

        /// <summary>
        /// 连接表的字段,如果为空取【Id】
        /// </summary>
        public string RightEqualFieldName { get; private set; }

        /// <summary>
        /// 外键表名
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// 表 简写
        /// </summary>
        public string ShortName { get; private set; }
        /// <summary>
        /// 提取 字段
        /// </summary>
        public string TakeFieldName { get; private set; }
        /// <summary>
        /// 提取 字段后的别名 
        /// </summary>
        public string TakeFieldAsName { get; private set; }

        
    }

}
