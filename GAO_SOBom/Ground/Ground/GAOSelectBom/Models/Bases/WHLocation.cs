using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("WH_LOC")]
    public class WHLocation : Entity
    {
        [Label("储位代码")]
        [Require]
        public string PRD_LOC { get; set; }

        [Label("名称")]
        public string NAME { get; set; }

        [Label("仓库")]
        [Require]
        public string WH { get; set; }
    }
}
