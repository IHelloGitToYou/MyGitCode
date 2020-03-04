using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [Serializable]
    public enum EnumTax
    {
        //[Label("扣税方式 1.不计税 2.含税价 3.不含税价")]
        [Label("1.不计税")]
        NoTax = 1,
        [Label("2.含税价")]
        inTax = 2,
        [Label("3.不含税价")]
        outTax = 3
    }
}
