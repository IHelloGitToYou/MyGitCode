using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Register
{
    [SugarTable("GAO_Register")]
    public class RegisterInfoModel
    {
        /// <summary>
        /// system_no
        /// </summary>
        [SugarColumn(ColumnName = "system_no", ColumnDescription = "序列号")]
        public string system_no { get; set; } = "";

        /// <summary>
        /// valid_date
        /// </summary>
        [SugarColumn(ColumnName = "valid_date", ColumnDescription = "有效日期")]
        public DateTime valid_date { get; set; }

        /// <summary>
        /// md5
        /// </summary>
        [SugarColumn(ColumnName = "md5", ColumnDescription = "md5", DefaultValue = "0")]
        public string md5 { get; set; } = "0";
    }
}
