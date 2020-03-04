using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    /// <summary>
    /// 计数
    /// </summary>
    [RootEntity]
    [SqlSugar.SugarTable("GAO_Number_Counter")]
    public class NumberCounter : Entity
    {
        [Label("目标字段")]
        [Require]
        public string Targer { get; set; }
        [Label("当前流水号")]
        public int NowNumber { get; set; }
    }
     
}
