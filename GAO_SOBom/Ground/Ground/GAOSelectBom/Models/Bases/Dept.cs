using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Dept")]
    public class Dept : Entity
    {
        [Label("部门代码")]
        [Require]
        public string DEP { get; set; }
        [Label("名称")]
        public string NAME { get; set; }
    }
}
