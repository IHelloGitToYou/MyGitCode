using GaoCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gao.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Gao_User")]
    public class User: Entity
    {
        [Label("登录编码")]
        [Require]
        public string UserNo { get; set; }
        [Label("名称")]
        [Require]
        public string Name { get; set; }

        //[Label("MD5密码")]
        //public string PassWork { get; set; }

        /////// <summary>
        /////// 测试 字段 
        /////// </summary>
        ////public int CityId { get; set; }
        ////[IngnorePropery("XXX")]
        ////[SqlSugar.SugarColumn(IsIgnore = true)]
        ////public City CityObj { get; set; }

        ////[ViewAsProperty("CityId", "Gao_City", "c", "Name")]
        ////[SqlSugar.SugarColumn(IsIgnore = true)]
        ////public string CityName { get; set; }
    }

    ////[RootEntity]
    ////[SqlSugar.SugarTable("Gao_City")]
    ////public class City : Entity
    ////{
    ////    [Label("City编码")]
    ////    public string CityNo { get; set; }
    ////    [Label("名称")]
    ////    public string Name { get; set; }
    ////}


    ////public class ViewModelUser : User
    ////{
    ////    [Label("City编码别名")]
    ////    public string CityCityNo { get; set; }

    ////    [Label("City名称别名")]
    ////    public string CityName {get;set;}
    ////}

}
