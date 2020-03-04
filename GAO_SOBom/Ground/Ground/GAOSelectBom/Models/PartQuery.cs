using GaoCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    public class PartQuery : QueryEntity
    {
        [Label("模块代号")]
        public string PartNo { get; set; }

        [Label("选配模块名称")]
        public string PartName { get; set; }
        
        
        public override ApiQueryDataResponse Fetch(QueryEntity vthis)
        {
            PartQuery query = vthis as PartQuery;
            var db = SqlSugarBase.GetDB(vthis.CurrentXLoginDB);

            var q = RF<Part>.GetSqlQueryWithViewProperty(vthis.CurrentXLoginDB);
            if (query.PartNo.IsNotEmpty())
            {
                q.Where(o => o.PartNo.Contains("%" + query.PartNo + "%"));
            }
            if (query.PartName.IsNotEmpty())
            {
                q.Where(o => o.PartName.Contains("%" + query.PartName + "%"));
            }

            q.OrderBy(o => o.Sort);
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
