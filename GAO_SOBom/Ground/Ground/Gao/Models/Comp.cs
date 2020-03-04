using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Gao_Comp")]
    public class Comp : Entity
    {
        [Label("帐套编码")]
        [Require]
        public string CompNo { get; set; }
        [Label("名称")]
        [Require]
        public string Name { get; set; }
    }
}
