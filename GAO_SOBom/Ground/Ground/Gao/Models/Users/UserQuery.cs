using GaoCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Gao.Models
{
    [RootEntity]
    public class UserQuery: QueryEntity
    {
        [Label("登录编码")]
        public string UserNo { get; set; }
        [Label("名称")]
        public string Name { get; set; }

       
        public override ApiQueryDataResponse Fetch(QueryEntity vthis)
        {
            UserQuery userQuery = vthis as UserQuery;
            string DbName = RunningControllers.CacheAssemblyDBs[typeof(User).Assembly];
            var db = SqlSugarBase.GetDB(DbName);

            var q = RF<User>.GetSqlQueryWithViewProperty();
            if (userQuery.UserNo.IsNotEmpty())
            {
                q.Where(o => o.UserNo.Contains("%" + userQuery.UserNo + "%"));
            }
            if (userQuery.Name.IsNotEmpty())
            {
                q.Where(o => o.Name.Contains("%" + userQuery.Name + "%"));
            }

            var dtCount = q.Clone().Count();
            DataTable dt = null;
            if (vthis.page != null)
            {
                dt = q.ToDataTablePage(vthis.page.Value, vthis.limit.Value);
            }
            else
            {
                dt = q.ToDataTable();
            }

            return new ApiQueryDataResponse()
            {
                total = dtCount,
                items = RF.ToDictionary(dt)
            };
        }
    }
}
