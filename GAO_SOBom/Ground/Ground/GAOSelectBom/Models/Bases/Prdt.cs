using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Prdt")]
    public class Prdt : Entity
    {
        [Label("货号代码")]
        [Require]
        public string Prd_No { get; set; }
        [Label("名称")]
        public string Name { get; set; }

        [Label("简称")]
        public string Snm { get; set; }

        [Label("规格")]
        public string Spc { get; set; }
    }
}
