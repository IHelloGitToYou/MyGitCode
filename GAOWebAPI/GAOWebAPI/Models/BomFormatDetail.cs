using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GAOWebAPI.Models
{
    [SugarTable("GAO_BomFormat_Dtl")]
    public class BomFormatDetail
    {
        [SugarColumn(ColumnDescription = "自增Id")]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "Excel_Id", ColumnDescription = "所属父Id")]
        public int excel_id { get; set; }

        /// <summary>
        /// 列 Y坐标
        /// </summary>
        [SugarColumn(ColumnName = "Cell_Index", ColumnDescription = "列Y坐标")]
        public int cell_index { get; set; } = -1;

        /// <summary>
        /// 储存位置[1.表头 2.BOM表身, 3.二者]
        /// </summary>
        [SugarColumn(ColumnName = "Diy_Type", ColumnDescription = "储存位置[1.表头 2.BOM表身, 3.二者]")]
        public string diy_type { get; set; } = "";

        /// <summary>
        /// 字段名
        /// </summary>
        [SugarColumn(ColumnName = "field_name", ColumnDescription = "字段名")]
        public string field_name { get; set; }

        /// <summary>
        /// 字段编码
        /// </summary>
        [SugarColumn(ColumnName = "field_no", ColumnDescription = "字段编码")]
        public string field_no { get; set; }

        /// <summary>
        /// 数据类型 SYS:必须, DIY:系统自定义, SHOW:显示字段
        /// </summary>
        [SugarColumn(ColumnName = "cell_type", ColumnDescription = "SYS:必须, DIY:系统自定义, SHOW:显示字段")]
        public string cell_type { get; set; }

        /// <summary>
        /// 检查资料 是否完善 如PrdNo "PRDT.PRD_NO"
        /// </summary>
        [SugarColumn(ColumnName = "Check_Exist", ColumnDescription = "检查资料")]
        public string check_exist { get; set; }
    }
}
