using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    [SugarTable("MF_BOM")]
    public class MF_BOM_Simple
    {
        /// <summary>
        /// BOM
        /// </summary>
        [SugarColumn(ColumnName = "bom_no", ColumnDescription = "BOM")]
        public string bom_no { get; set; } = "";
        
        /// <summary>
        /// 货号
        /// </summary>
        [SugarColumn(ColumnName = "prd_no", ColumnDescription = "货号")]
        public string prd_no { get; set; } = "";

        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(ColumnName = "pf_no", ColumnDescription = "版本号",DefaultValue ="0")]
        public string pf_no { get; set; } = "0";

        [SugarColumn(IsIgnore = true)]
        public List<TF_BOM_Simple> TF_BOMS { get; set; }

    }

    [SugarTable("TF_BOM")]
    public class TF_BOM_Simple
    {
        /// <summary>
        /// BOM
        /// </summary>
        [SugarColumn(ColumnName = "bom_no", ColumnDescription = "BOM")]
        public string bom_no { get; set; } = "";


        [SugarColumn(ColumnName = "itm", ColumnDescription = "项次")]
        public int itm { get; set; } = 0;

        /// <summary>
        /// 货号
        /// </summary>
        [SugarColumn(ColumnName = "prd_no", ColumnDescription = "货号")]
        public string prd_no { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnName = "qty", ColumnDescription = "qty")]
        public double qty { get; set; } = 0.0;

        /// <summary>
        /// 副单位数量
        /// </summary>
        [SugarColumn(ColumnName = "qty1", ColumnDescription = "qty1")]
        public double qty1 { get; set; } = 0.0;
    }
}
