using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    [SugarTable("PSWD")]
    public class EmployeeModel
    {
        /// <summary>
        /// 员工代号
        /// </summary>
        [SugarColumn(ColumnName = "usr", ColumnDescription = "员工代号")]
        public string usr { get; set; } = "";
        /// <summary>
        /// 员工名称
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "员工名称")]
        public string name { get; set; } = "";

        /// <summary>
        /// 帐套号
        /// </summary>
        [SugarColumn(ColumnName = "compno", ColumnDescription = "帐套号")]
        public string compno { get; set; } = "";
    }

    //[Serializable]
    //public class UserInfo
    //{
    //    public string usr { get; set; }
    //    public string name { get; set; }
    //}

}
