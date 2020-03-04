using GaoCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GAOSelectBom.Models.Bases
{
    [RootEntity]
    public class BomSOQuery : QueryEntity
    {
        [Label("BOM类型")]
        public BomType SearchBom { get; set; } = BomType.Bom;

        [Label("Bom")]
        public string BOM_NO { get; set; }

        [Label("产品名称")]
        public string NAME { get; set; }

        [Label("产品编码")]
        public string PRD_NO { get; set; }
        
        public override ApiQueryDataResponse Fetch(QueryEntity vthis)
        {
            BomSOQuery query = vthis as BomSOQuery;
            var db = SqlSugarBase.GetDB(vthis.CurrentXLoginDB);
            DataTable dt = null;
            if (query.SearchBom == BomType.Bom)
            {
                var q = RF<Bom>.GetSqlQueryWithViewProperty(vthis.CurrentXLoginDB);
                if (query.BOM_NO.IsNotEmpty())
                    q.Where(o => o.BOM_NO.Contains("%" + query.BOM_NO + "%"));
                if (query.PRD_NO.IsNotEmpty())
                    q.Where(o => o.PRD_NO.Contains("%" + query.PRD_NO + "%"));
                if (query.NAME.IsNotEmpty())
                    q.Where(o => o.NAME.Contains("%" + query.NAME + "%"));

                var dtCount = q.Clone().Count();
                if (vthis.page != null)
                    dt = q.ToDataTablePage(vthis.page.Value, vthis.limit.Value);
                else
                    dt = q.ToDataTable();
            }
         
            if (query.SearchBom == BomType.Bom_SO)
            {
                var q = RF<BomSO>.GetSqlQueryWithViewProperty(vthis.CurrentXLoginDB);
                if (query.BOM_NO.IsNotEmpty())
                    q.Where(o => o.BOM_NO.Contains("%" + query.BOM_NO + "%"));
                if (query.PRD_NO.IsNotEmpty())
                    q.Where(o => o.PRD_NO.Contains("%" + query.PRD_NO + "%"));
                if (query.NAME.IsNotEmpty())
                    q.Where(o => o.NAME.Contains("%" + query.NAME + "%"));

                var dtCount = q.Clone().Count();
                
                if (vthis.page != null)
                    dt = q.ToDataTablePage(vthis.page.Value, vthis.limit.Value);
                else
                    dt = q.ToDataTable();
            }

            return new ApiQueryDataResponse()
            {
                total = dt.Rows.Count,
                items = RF.ToDictionary(dt)
            };
        }
    }
}
