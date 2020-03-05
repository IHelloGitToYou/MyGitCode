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

        [Label("默认仓库")]
        public string Wh { get; set; }

        [Label("默认仓库名称")] //??能否直接字段?>
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string WhName { get; set; }

        [Label("储位")]
        public string Prd_Loc { get; set; }

        [Label("储位名")]
        public string Prd_Loc_Name { get; set; }
    }
}
