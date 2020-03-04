using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.Models.Users
{
    [Serializable]
    [RootEntity]
    [SqlSugar.SugarTable("Gao_User_Comp")]
    public class UserComp : Entity
    {
        [Require]
        [Label("用户Id")]
        public int UserId { get; set; }

        //[Require]
        [IsCombobox("CompNo", "Gao_Comp", "a")]
        [Label("帐套Id")]
        public int CompId { get; set; }
        
        [Label("MD5密码")]
        public string PassWork { get; set; }

        [Label("帐套代码")]
        [ViewAsProperty("CompId", "Gao_Comp", "b", "CompNo")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string CompNo { get; set; }

        [ViewAsProperty("CompId", "Gao_Comp", "c", "Name")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string CompName { get; set; }
    }
}
