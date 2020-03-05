using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Gao.Models;
using Gao.Models.Users;
using Gao.Services.Users;
using GaoCore;
using GaoCore.Extjs;
using GAOSelectBom.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ground.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class SelectBomController : BaseController
    {
        #region 下拉框 查询
        [HttpGet]
        public ApiQueryDataResponse SearchPrdt(string query, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<Prdt>(@"select t.id, t.Prd_No, t.Name, T.snm, T.Spc,  
                                    T.Wh,W.name as WhName, 
                                    T.Prd_Loc,
                                    -1 CreateId, t.Record_dd CreateDD,
                                    -1 UpdateId, t.Record_dd UpdateDD
                                    from Prdt t
                                    left join My_WH w on w.wh = T.wh");
            if (query.IsNotEmpty())
                q.Where(o => o.Prd_No.Contains(query) || o.Name.Contains(query) || o.Spc.Contains(query));

            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }

        [HttpGet]
        public ApiQueryDataResponse SearchCust(string query, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<Cust>(@"select row_number() over( order by cus_no) Id ,
                                    t.CUS_NO, t.NAME, T.SNM, T.OBJ_ID, T.ID1_TAX , T.CLS2 ,
                                    -1 CreateId, t.create_dd CreateDD,
                                    -1 UpdateId, t.eff_dd UpdateDD
                                    from Cust t");
            if (query.IsNotEmpty())
                q.Where(o => o.CUS_NO.Contains(query) || o.NAME.Contains(query) || o.SNM.Contains(query));

            q.In(it => it.OBJ_ID, new string[] { "1", "3" });

            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }

        [HttpGet]
        public ApiQueryDataResponse SearchDept(string query, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<Dept>(@"select row_number() over( order by DEP) Id ,t.DEP, t.NAME,
                                    -1 CreateId, getdate() CreateDD,
                                    -1 UpdateId, getdate() UpdateDD
                                    from Dept t");
            if (query.IsNotEmpty())
                q.Where(o => o.DEP.Contains(query) || o.NAME.Contains(query));

            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }

        [HttpGet]
        public ApiQueryDataResponse SearchWH(string query, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<MY_WH>(@"select row_number() over( order by WH) Id ,t.WH, t.NAME,
                                    -1 CreateId, EFF_DD CreateDD,
                                    -1 UpdateId, EFF_DD UpdateDD
                                    from MY_WH t");
            if (query.IsNotEmpty())
                q.Where(o => o.WH.Contains(query) || o.NAME.Contains(query));

            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }

        [HttpGet]
        public ApiQueryDataResponse SearchWHLoc(string query, string WH, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.Queryable<WHLocation>();
            if (query.IsNotEmpty())
                q.Where(o => o.PRD_LOC.Contains(query) || o.NAME.Contains(query));
            if (WH.IsNotEmpty())
                q.Where(o => o.WH == WH);
            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }


        [HttpGet]
        public ApiQueryDataResponse SearchSalm(string query, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<Salm>(@"select row_number() over( order by SAL_NO) Id ,
                                    t.SAL_NO, t.NAME, t.DEP,
                                    -1 CreateId, getdate() CreateDD,
                                    -1 UpdateId, getdate() UpdateDD
                                    from Salm t");
            if (query.IsNotEmpty())
                q.Where(o => o.SAL_NO.Contains(query) || o.NAME.Contains(query));

            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }

        /// <summary>
        /// 订单配方
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiQueryDataResponse SearchBomSO(string query, int? page, int? limit)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<BomSO>(@"select row_number() over(order by BOM_NO) Id ,
                                    t.BOM_NO, t.NAME, t.PRD_NO,
                                    -1 CreateId, getdate() CreateDD,
                                    -1 UpdateId, getdate() UpdateDD
                                    from mf_bom_so t");
            if (query.IsNotEmpty())
                q.Where(o => o.BOM_NO.Contains(query) || o.PRD_NO.Contains(query));

            var result = new ApiQueryDataResponse();
            if (page != null)
                result.items = q.ToPageList(page.Value, limit.Value);
            else
                result.items = q.ToList();

            result.total = q.Count();
            return result;
        }
        

        #endregion


        [HttpGet]
        public List<Prdt> GetPrdts(List<string> PrdNos)
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            var q = db.SqlQueryable<Prdt>(@"select t.id, t.Prd_No, t.Name, T.snm, T.Spc, 
                                    -1 CreateId, t.Record_dd CreateDD,
                                    -1 UpdateId, t.Record_dd UpdateDD
                                    from Prdt t");

            q.Where(o => PrdNos.Contains(o.Prd_No));
            return q.ToList();
        }



        /// <summary>
        /// 取新的模块号 T1 -》T2-》T3
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetNewPartName()
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());

            //db.CodeFirst.InitTables(typeof(Part));//Create table
            //db.CodeFirst.InitTables(typeof(PartPrdt));
            //db.CodeFirst.InitTables<PartPrdtDetail>();

            int cnt = db.Queryable<Part>().Count();

            return "T" + (cnt + 1);
        }

        [HttpPost]
        public bool SavePartPrdt([FromBody]List<PartPrdt> PartPrdts)
        {
            if (PartPrdts.Count == 0)
                throw new InValidException("无需要修改的记录！");

            //RF<PartPrdt>.Save(PartPrdts, GetDB_ON_X_LOGIN_ID());
            var db = SqlSugarBase.GetDB(GetLoginUser());
            db.BeginTran();
            try
            {
                
                foreach (var item in PartPrdts)
                {
                    RF<PartPrdt>.doSave(db, item);

                    List<Prdt> prdts = new List<Prdt>();
                    if (item.ValidBomPrdtString.IsNotEmpty())
                        prdts = GetPrdts(item.ValidBomPrdtString.Split(',').ToList());
                    else
                        prdts = GetPrdts(item.ExceptBomPrdtString.Split(',').ToList());

                    db.Deleteable<PartPrdtDetail>().Where(o => o.PartPrdtId == item.Id).ExecuteCommand();

                    foreach (var item2 in prdts)
                    {
                        PartPrdtDetail newDtl = new PartPrdtDetail();
                        newDtl.PersistStatus = PersistStatus.NEW;
                        newDtl.PartPrdtId = item.Id;
                        newDtl.IsExcept = false;
                        newDtl.PrdId  = item2.Id;
                        newDtl.Prd_No = item2.Prd_No;
                        RF<PartPrdtDetail>.doSave(db, newDtl);
                    }
                }

                db.CommitTran();
            }
            catch(Exception ex)
            {
                db.RollbackTran();
                throw ex;
            }

            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Part> GetAllPart()
        {
            var db = Entity.GetDb(GetDB_ON_X_LOGIN_ID());
            return db.Queryable<Part>().ToList();
        }


        [HttpGet]
        public object GetPartPrdts(int PartId)
        {
            //var db = SqlSugarBase.GetDB(GetDB_ON_X_LOGIN_ID());
            var q = RF<PartPrdt>.GetSqlQueryWithViewProperty(GetDB_ON_X_LOGIN_ID());

            var dt = q.Where(o => o.PartId == PartId).ToDataTable();//.ToList();

            return RF.ToDictionary(dt);
        }

        [HttpGet]
        public ApiQueryDataResponse SearchTPartPrdt(string m_prd_no, string PartNo, string query, int? page, int? limit)
        {
            var db = SqlSugarBase.GetDB(GetDB_ON_X_LOGIN_ID());
          
            var q = RF<PartPrdt>.GetSqlQueryWithViewProperty(GetDB_ON_X_LOGIN_ID());
            q.Where(o => o.PartNo == PartNo);
            if(query.IsNotEmpty())
                q.Where(o => o.PrdNo.Contains(query) || o.PrdName.Contains(query));

            var dt = q.ToDataTable();//.ToList();
            dt.Columns.Add("IsSelected");
            foreach (DataRow item in dt.Rows)
            {
                string ValidBomPrdtString = item["ValidBomPrdtString"].ObjToString();
                string ExceptBomPrdtString = item["ExceptBomPrdtString"].ObjToString();
                //仅使用的
                if (ValidBomPrdtString.IsNotEmpty())
                {
                    if(ValidBomPrdtString.Split(",").Contains(m_prd_no) == false)
                    {
                        item["IsSelected"] = "FALSE";
                    }
                }
                //排除的
                if (ExceptBomPrdtString.IsNotEmpty())
                {
                    if (ExceptBomPrdtString.Split(",").Contains(m_prd_no) == true)
                    {
                        item["IsSelected"] = "FALSE";
                    }
                }
            }

            var dt2 = dt.Select("Isnull(IsSelected,'') <> 'FALSE'").ToList();

            var result = new ApiQueryDataResponse();
            if (page != null)
            {
                 var pageRows = dt2.Skip((page.Value - 1) * limit.Value).Take(limit.Value).ToList();
                 result.items = RF.ToDictionary(pageRows, dt); //q.ToPageList(page.Value, limit.Value);
            }
            else
                result.items = RF.ToDictionary(dt2, dt);

            result.total = dt2.Count();
            return result;
            
        }

    }
}
