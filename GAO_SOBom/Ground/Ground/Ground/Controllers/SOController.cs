using GaoCore;
using GAOSelectBom.Models;
using GAOSelectBom.Services.SO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Ground.Controllers
{


    [Route("api/[controller]/[action]/{id?}")]
    public class SOController : BaseController
    {
        [HttpGet]
        public TabelSOApiDataDictory GetTableSO(string OS_ID, string OS_NO)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var mf = RF<MF_Pos>.GetSqlQueryWithViewProperty(GetDB_ON_X_LOGIN_ID())
                        .Where(o => o.OS_ID == OS_ID && o.OS_NO == OS_NO)
                        .ToDataTable();

            var tfs = RF<TF_Pos>.GetSqlQueryWithViewProperty(GetDB_ON_X_LOGIN_ID())
                       .Where(o => o.OS_ID == OS_ID && o.OS_NO == OS_NO)
                       .OrderBy(O=>O.ITM)
                       .ToDataTable();

            var tfZ_s = RF<TF_Pos_Z>.GetSqlQueryWithViewProperty(GetDB_ON_X_LOGIN_ID())
                      .Where(o => o.OS_ID == OS_ID && o.OS_NO == OS_NO)
                      .OrderBy(O => O.ITM)
                      .ToDataTable();

            TabelSOApiDataDictory apiData = new TabelSOApiDataDictory();
            apiData.Header = RF.ToDictionary(mf).First();
            apiData.DetailList = RF.ToDictionary( tfs);
            apiData.DetailZList = RF.ToDictionary(tfZ_s);
            return apiData;
        }


        [HttpGet]
        public string GetNewOSNo(DateTime Day, string SOFormat)
        {
            SOServices service = new SOServices(SqlSugarBase.GetDB(GetLoginUser()));

            return service.GetNewOSNo(Day, SOFormat);
        }


        /// <summary>
        /// 取员工的部门
        /// </summary>
        /// <param name="SalNo"></param>
        [HttpGet]
        public Dept GetUserDept(string SalNo)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<Salm>(@"select row_number() over( order by SAL_NO) Id ,
                                    t.SAL_NO, t.NAME, t.DEP,
                                    -1 CreateId, getdate() CreateDD,
                                    -1 UpdateId, getdate() UpdateDD
                                    from Salm t");

            var salm = q.Where(o => o.SAL_NO == SalNo).First(); 
            if (salm == null || salm.DEP.IsNullOrEmpty())
                return null;

            var q2 = db.SqlQueryable<Dept>(@"select row_number() over( order by DEP) Id ,t.DEP, t.NAME,
                                    -1 CreateId, getdate() CreateDD,
                                    -1 UpdateId, getdate() UpdateDD
                                    from Dept t");
            return q2.Where(o => o.DEP == salm.DEP).First();
        }
    }
}
