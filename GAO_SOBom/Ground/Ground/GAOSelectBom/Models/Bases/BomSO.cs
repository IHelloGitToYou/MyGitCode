using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("mf_bom_so")]
    public class BomSO : Entity
    {
        [Label("Bom")]
        [Require]
        public string BOM_NO { get; set; }
        [Label("产品名称")]
        public string NAME { get; set; }
        [Label("产品编码")]
        public string PRD_NO { get; set; }
    }

    [RootEntity]
    [SqlSugar.SugarTable("mf_bom")]
    public class Bom : Entity
    {
        [Label("Bom")]
        [Require]
        public string BOM_NO { get; set; }
        [Label("产品名称")]
        public string NAME { get; set; }
        [Label("产品编码")]
        public string PRD_NO { get; set; }
    }

}
