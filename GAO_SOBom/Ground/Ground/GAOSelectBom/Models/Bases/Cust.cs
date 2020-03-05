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
        [Label("客户类型")]
        public string OBJ_ID { get; set; }

        [Label("货号代码")]
        public string CUS_NO { get; set; }

        [Label("名称")]
        public string NAME { get; set; }

        [Label("简称")]
        public string SNM { get; set; }

        [Label("扣税类别")]
        public EnumTax ID1_TAX { get; set; }

        [Label("立帐方式")]
        public EnumZhangType CLS2 { get; set; }
    }
}
