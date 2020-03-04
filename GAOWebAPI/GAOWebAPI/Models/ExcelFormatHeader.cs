using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    /// <summary>
    /// 表名 GAO_ExcelFormat_Header
    /// </summary>
    [SugarTable("GAO_ExcelFormat_Header")]
    public class ExcelFormatHeader
    {
        [SugarColumn(ColumnDescription ="自增Id")]
        public int Id { get; set; }
        
        [SugarColumn(ColumnName = "Format_No", ColumnDescription = "模版编码")]
        public string FormatNo { get; set; } //"标准样式"

        /// <summary>
        /// 成品货号[坐标]
        /// </summary>
        [SugarColumn(ColumnName = "Prd_No_Pos", ColumnDescription = "成品货号[坐标]")]
        public string PrdNoPos { get; set; }


        [SugarColumn(ColumnName = "Prd_Name_Pos", ColumnDescription = "成品名[坐标]")]
        public string PrdNamePos { get; set; }

        /// <summary>
        /// 生产部门 坐标 A3
        /// </summary>
        [SugarColumn(ColumnName = "Dept_No_Pos", ColumnDescription = "生产部门[坐标]")]
        public string DeptNoPos { get; set; }

        /// <summary>
        /// 配方号 坐标 A1
        /// </summary>
        [SugarColumn(ColumnName = "Id_No_Pos", ColumnDescription = "配方号[坐标]")]
        public string IdNoPos { get; set; }


        /// <summary>
        /// 仓库取值 	    1.货品信息默认仓库
        /// </summary>
        [SugarColumn(ColumnName = "WH_Form_Type", ColumnDescription = "仓库取值[1.货品信息默认仓库]")]
        public string WHFormType { get; set; } = "1";

        /// <summary>
        /// Bom储存位置		1.标准BOM  2.订单BOM
        /// </summary>
        [SugarColumn(ColumnName = "Bom_Place", ColumnDescription = "Bom储存位置[1.标准BOM  2.订单BOM]")]
        public string BomPlace { get; set; } = "1";

        /// <summary>
        /// 数据开始行  	3 
        /// </summary>
        [SugarColumn(ColumnName = "Start_Row", ColumnDescription = "数据开始行")]
        public int StartRow { get; set; } = 3;

        /// <summary>
        /// 数据阶数列  	2 
        /// </summary>
        [SugarColumn(ColumnName = "Bom_Struct_Cell", ColumnDescription = "数据阶数列")]
        public int BomStructCell { get; set; } = 2;

        [SugarColumn(IsIgnore = true)]
        public List<BomFormatDetail> Details { get; set; }
    }
}
