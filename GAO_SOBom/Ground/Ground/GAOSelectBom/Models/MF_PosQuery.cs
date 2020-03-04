using GaoCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    public class MF_PosQuery : QueryEntity
    {
        [Label("订单日期")]
        public DateTime? os_dd_begin { get; set; }

        [Label("至")]
        public DateTime? os_dd_end { get; set; }

        [Label("订单号码")]
        public string os_no { get; set; } = "";

        [Label("客户")]
        public string cus_no { get; set; } = "";

        [Label("客户订单")]
        public string cus_os_no { get; set; } = "";

        [Label("批号")]
        public string bat_no { get; set; } = "";


        public override ApiQueryDataResponse Fetch(QueryEntity vthis)
        {
            MF_PosQuery query = vthis as MF_PosQuery;
            var db = SqlSugarBase.GetDB(vthis.CurrentXLoginDB);

            var q = RF<MF_Pos>.GetSqlQueryWithViewProperty(vthis.CurrentXLoginDB);
            q.Where(o => o.OS_ID == "SO");

            if (query.cus_no.IsNotEmpty())
                q.Where(" CUS_NO LIKE '%"+ query.cus_no+ "%' OR CUS_NO_DISPLAY LIKE '%" + query.cus_no + "%'"); //.Where(o => o.CUS_NO.Contains("%" + query.cus_no + "%"));
            
            if (query.os_no.IsNotEmpty())
                q.Where(o => o.OS_NO.Contains("%" + query.os_no + "%"));
            if (query.os_dd_begin != null)
                q.Where(o => o.OS_DD >= query.os_dd_begin.Value.Date);
            if (query.os_dd_end != null)
                q.Where(o => o.OS_DD <= query.os_dd_end.Value.Date);
            if (query.cus_os_no.IsNotEmpty())
                q.Where(o => o.CUS_OS_NO.Contains("%" + query.cus_os_no + "%"));
            if (query.bat_no.IsNotEmpty())
                q.Where(o => o.BAT_NO.Contains("%" + query.bat_no + "%"));

            q.OrderBy(o => o.OS_DD).OrderBy(o => o.OS_NO);

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
