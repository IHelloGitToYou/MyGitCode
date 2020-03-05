using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("MY_WH")]
    public class MY_WH : Entity
    {
        [Label("仓库代码")]
        [Require]
        public string WH { get; set; } 
        [Label("名称")]
        public string NAME { get; set; }
    }
}
