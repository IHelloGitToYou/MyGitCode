using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace GAOWebAPI.Models
{
    [SugarTable("PRDT")]
    public class PrdtModel
    {
        /// <summary>
        /// 货号
        /// </summary>
        [SugarColumn(ColumnName = "prd_no", ColumnDescription = "货号")]
        public string prd_no { get; set; } = "";

        /// <summary>
        /// snm
        /// </summary>
        [SugarColumn(ColumnName = "snm", ColumnDescription = "snm")]
        public string snm { get; set; } = "";

        /// <summary>
        /// ut
        /// </summary>
        [SugarColumn(ColumnName = "ut", ColumnDescription = "ut")]
        public string ut { get; set; } = "";

        /// <summary>
        /// ut1
        /// </summary>
        [SugarColumn(ColumnName = "ut1", ColumnDescription = "ut1")]
        public string ut1 { get; set; } = "";

        /// <summary>
        /// dfu_ut
        /// </summary>
        [SugarColumn(ColumnName = "dfu_ut", ColumnDescription = "dfu_ut")]
        public string dfu_ut { get; set; } = "";

        /// <summary>
        /// name
        /// </summary>
        [SugarColumn(ColumnName = "name", ColumnDescription = "name")]
        public string name { get; set; } = "";

        /// <summary>
        /// spc
        /// </summary>
        [SugarColumn(ColumnName = "spc", ColumnDescription = "spc")]
        public string spc { get; set; } = "";

        /// <summary>
        /// 大类 knd 1.商 2.制成 3.半成 4.物料 5.原料
        /// </summary>
        [SugarColumn(ColumnName = "knd", ColumnDescription = "knd")]
        public string knd { get; set; } = "4";

        /// <summary>
        /// 中类
        /// </summary>
        [SugarColumn(ColumnName = "idx1", ColumnDescription = "idx1")]
        public string idx1 { get; set; } = "";

        /// <summary>
        /// 仓库
        /// </summary>
        [SugarColumn(ColumnName = "wh", ColumnDescription = "wh")]
        public string wh { get; set; } = "";

        /// <summary>
        /// 加工方式
        /// </summary>
        [SugarColumn(ColumnName = "tw_id", ColumnDescription = "tw_id")]
        public int tw_id { get; set; } = 2;
    }
}
