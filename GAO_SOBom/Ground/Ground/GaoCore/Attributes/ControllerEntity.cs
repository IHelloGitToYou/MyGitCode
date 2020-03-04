using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public class ControllerEntity : Attribute
    {
        /// <summary>
        /// 静态数据库, 不可变的,如果本值有值,DBNameByXLoginID必定是Null 二选一
        /// </summary>
        public string ControllerDBName { get; private set; }

        /// <summary>
        /// 当前数据库 按登录用户的DB决定
        /// </summary>
        public bool DBNameByXLoginID { get; private set; }
        public ControllerEntity(string DBName)
        {
            ControllerDBName = DBName;

            var jObject = new JsonConfigHelper("GaoSettings.json").jObject;
            var jToken = jObject.SelectToken("ConnectionStrings").SelectToken(DBName);
            if (jToken == null)
                throw new StructureException("数据库[{0}]不存在".FormatOrg(DBName));
            string connectionString = jToken.ToString();
            //SqlSugarBase.DB_ConnectionStringFormatOnDB = jObject.SelectToken("帐套数据库连接格式").ToString();
            SqlSugarBase.Register(DBName, connectionString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsByLoginId">必须是True</param>
        public ControllerEntity(bool IsByLoginId)
        {
            if (IsByLoginId == false)
                throw new InValidException("参数必须是True");
            DBNameByXLoginID = true;
            ControllerDBName = string.Empty;
        }
    }
}
