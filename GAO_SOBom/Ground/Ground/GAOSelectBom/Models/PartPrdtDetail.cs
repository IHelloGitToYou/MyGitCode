using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Gao_PartPrdt_Detail")]
    public class PartPrdtDetail : Entity
    {
        [Label("所属Id")]
        public int PartPrdtId { get; set; }

        [Label("是否排除")]
        [Require]
        public bool IsExcept { get; set; } 

        [IsCombobox("Prd_No", "Prdt", "a")]
        [Label("货号Id")]
        [Require]
        public int PrdId { get; set; }

        [Label("货号代号")]
        [Require]
        public string Prd_No { get; set; }

        [Label("货品名称")]
        [ViewAsProperty("PrdId", "Prdt", "b", "Name")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string PrdName { get; set; }

        [Label("货品规格")]
        [ViewAsProperty("PrdId", "Prdt", "c", "Spc")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string PrdSpc { get; set; }
    }
}
