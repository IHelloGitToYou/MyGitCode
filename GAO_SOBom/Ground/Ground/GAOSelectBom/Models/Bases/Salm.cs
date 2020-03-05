using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("SALM")]
    public class Salm : Entity
    {
        [Label("员工代码")] 
        [Require]
        public string SAL_NO { get; set; }
        [Label("名称")]
        public string NAME { get; set; }

        [Label("部门代号")]
        public string DEP { get; set; }
    }
}
