using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    //子表: 所属模块 货号Id, 货号代号 货品名称(视) 货品规格(视)  [添加, 编辑, 删除, 保存]
    [RootEntity]
    [SqlSugar.SugarTable("Gao_PartPrdt")]
    public class PartPrdt : Entity
    {
        [Label("模块外键Id")]
        [Require]
        public int PartId { get; set; }

        [Label("模块代号")]
        [Require]
        public string PartNo { get; set; }

        [IsCombobox("Prd_No", "Prdt", "a")]
        [Label("货号Id")]
        [Require]
        public int PrdId { get; set; }

        [Label("货号代号")]
        [Require]
        public string PrdNo { get; set; }

        [Label("货品名称")]
        [ViewAsProperty("PrdId", "Prdt", "b", "Name")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string PrdName { get; set; }

        [Label("货品规格")]
        [ViewAsProperty("PrdId", "Prdt", "c", "Spc")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string PrdSpc { get; set; }

        [Label("可选配机型")]
        [Require]
        public string ValidBomPrdtString { get; set; }

        [Label("排除机型")]
        [Require]
        public string ExceptBomPrdtString { get; set; }
    }
}
