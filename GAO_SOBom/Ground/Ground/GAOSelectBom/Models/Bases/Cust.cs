using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Cust")]
    public class Cust : Entity
    {
        [Label("货号代码")]
        public string CUS_NO { get; set; }

        [Label("名称")]
        public string NAME { get; set; }

        [Label("简称")]
        public string SNM { get; set; }
    }
}
