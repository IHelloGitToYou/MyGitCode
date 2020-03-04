using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    [SugarTable("GAO_BomFormat_Body")]
    public class BomFormatBody
    {
        [SugarColumn(ColumnDescription = "自增Id")]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "Excel_Id", ColumnDescription = "所属父Id")]
        public int ExcelId { get; set; }

        /// <summary>
        /// 字段名
        /// </summary>
        [SugarColumn(ColumnName = "field_name", ColumnDescription = "字段名")]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段编码
        /// </summary>
        [SugarColumn(ColumnName = "field_no", ColumnDescription = "字段编码")]
        public string FieldNo { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [SugarColumn(ColumnName = "cell_type", ColumnDescription = "SYS:必须, DIY:系统自定义(1), SHOW:显示字段")]
        public string CellType { get; set; }

        /// <summary>
        /// 检查资料 是否完善 如PrdNo "PRDT.PRD_NO"
        /// </summary>
        [SugarColumn(ColumnName = "Check_Exist", ColumnDescription = "检查资料")]
        public string CheckExist { get; set; }
    }
}
