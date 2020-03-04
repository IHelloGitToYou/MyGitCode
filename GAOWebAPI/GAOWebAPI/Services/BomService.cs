using GAOWebAPI.Controllers;
using GAOWebAPI.Models;
using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Services
{
    public class BomService : SqlSugarBase
    {
        private BaseController _BaseController;
        public BomService(BaseController baseController)
        {
            _BaseController = baseController;
        }

        public new SqlSugarClient DB
        {
            get
            {
                string logId = _BaseController.Get_X_LOGIN_ID();
                return DB2(logId);
            }
        }

        //public SqlSugarClient DB
        //{
        //    string logId = _BaseController.Get_X_LOGIN_ID();
        //    return DB2(logId);
        //}

        /// <summary>
        /// 
        /// </summary>
        public static EnumBomConflictWay BomConflictHandleWay;
        public static Dictionary<string, string> DefaultBomHeaderSection;
        public static Dictionary<string, string> DefaultBomBodySection;
        public static Dictionary<string, string> DefaultPrdtSection;
        public static BomLevelWay BomLevelWay;

        public List<ExcelFormatHeader> SearchExcelFormatList(string SearchFormatNo)
        {
            string logId = _BaseController.Get_X_LOGIN_ID();
            var q = DB2(logId).Queryable<ExcelFormatHeader>();
            if (!string.IsNullOrEmpty(SearchFormatNo))
            {
                q.Where(o => o.FormatNo.Contains(SearchFormatNo));
            }
            return q.ToList();
        }

        public ExcelFormatHeader FetchExcelFormat(string FormatNo)
        {
            var q = DB.Queryable<ExcelFormatHeader>();
            q.Where(o => o.FormatNo == FormatNo);

            ExcelFormatHeader excelHeader = q.First();
            if (excelHeader != null)
            {
                excelHeader.Details = DB.Queryable<BomFormatDetail>()
                    .Where(o => o.excel_id == excelHeader.Id).OrderBy(o => o.diy_type)
                    .ToList();
            }

            return excelHeader;
        }

        /// <summary>
        /// 附加默认的明细
        /// </summary>
        public void AttachStandardBomDetails(ExcelFormatHeader NewingHeader)
        {
            var d1 = new BomFormatDetail() { diy_type = "3", cell_type = "SYS", field_no = "prd_no", cell_index = 1, field_name = "物料编码", check_exist = "PRDT.PRD_NO" };
            var d2 = new BomFormatDetail() { diy_type = "3", cell_type = "SYS", field_no = "name", cell_index = 1, field_name = "物料名称", check_exist = "" };
            var d3 = new BomFormatDetail() { diy_type = "3", cell_type = "SYS", field_no = "qty", cell_index = 1, field_name = "标准用量", check_exist = "" };
            var d4 = new BomFormatDetail() { diy_type = "1", cell_type = "SYS", field_no = "dep", cell_index = 1, field_name = "部门编码", check_exist = "DEPT.DEP" };
            var d5 = new BomFormatDetail() { diy_type = "1", cell_type = "SHOW", field_no = "dep_name", cell_index = 1, field_name = "部门名称" };
            var d6 = new BomFormatDetail() { diy_type = "3", cell_type = "SYS", field_no = "rem", cell_index = 1, field_name = "备注" };
            d1.excel_id = d2.excel_id = d3.excel_id = d4.excel_id = d5.excel_id = d6.excel_id = NewingHeader.Id;


            DB.Insertable<BomFormatDetail>(d1).IgnoreColumns(o => o.Id).ExecuteCommand();
            DB.Insertable<BomFormatDetail>(d2).IgnoreColumns(o => o.Id).ExecuteCommand();
            DB.Insertable<BomFormatDetail>(d3).IgnoreColumns(o => o.Id).ExecuteCommand();
            DB.Insertable<BomFormatDetail>(d4).IgnoreColumns(o => o.Id).ExecuteCommand();
            DB.Insertable<BomFormatDetail>(d5).IgnoreColumns(o => o.Id).ExecuteCommand();
            DB.Insertable<BomFormatDetail>(d6).IgnoreColumns(o => o.Id).ExecuteCommand();

            //            模版编码    "标准样式"
            //                配方号 * -1      BOM_NO
            //                物料编码 * 3       PRD_NO 检查存在 PRDT.PRD_NO
            //                物料名称 * 4       PRD_NAME
            //                标准用量 * 5       QTY
            //                部门编码 * 7       DEP_NO 检查存在 DEPT.DEP_NO
            //部门名称    6       DEP_NAME 外(2)
            //				Z_颜色        8       Z_COLOR 系统自定义(1)  
            //				Z_机头型号  9       Z_MA_NUMBER 系统自定义(1)  
            //				备注      10      REM
        }

        public ExcelFormatHeader SaveExcelFormat(ExcelFormatHeader Header)
        {
            DB.BeginTran();
            try
            {
                if (Header.Id <= 0)
                {
                    Header.Id = DB.Insertable<ExcelFormatHeader>(Header).IgnoreColumns(o => o.Id).ExecuteReturnIdentity();
                    AttachStandardBomDetails(Header);
                }
                else
                {
                    bool change = DB.Updateable<ExcelFormatHeader>(Header)
                        .IgnoreColumns(o => o.Id)
                        .Where(o => o.Id == Header.Id)
                        .ExecuteCommandHasChange();
                }

                DB.CommitTran();
            }
            catch (Exception ex)
            {
                DB.RollbackTran();

                throw ex;
            }

            return Header;
        }


        public bool RemoveExcelFormat(int HeaderId)
        {
            bool result = DB.Deleteable<ExcelFormatHeader>().Where(o => o.Id == HeaderId).ExecuteCommand() > 0 ? true : false;
            if (result == false)
                throw new Exception("删除失败，可能记录已被删除!");

            return result;
        }


        public List<BomFormatDetail> GetDtls(int HeaderId)
        {
            var q = DB.Queryable<BomFormatDetail>();
            return q.Where(o => o.excel_id == HeaderId).ToList();
        }

        public void CheckFieldExsistInTableStruct(string TableName, string FieldNo)
        {
            bool isOk = false;
            try
            {
                DB.Ado.GetDataTable("select {1} from {0} where 1 <>1 ".FormatOrg(TableName, FieldNo));
                isOk = true;
            }
            catch (Exception ex)
            {

            }

            if (isOk == false)
            {
                throw new Exception("表[{0}].[{1}]不存在字段".FormatOrg(TableName, FieldNo));
            }
        }

        public bool CheckValueExsistInTable(string CheckExist, string Value)
        {
            if (CheckExist.IsNullOrEmpty()) return true;

            var list = CheckExist.TrimEx().Split('.').ToList();
            if (list.Count != 2)
            {
                throw new Exception("检查资料格式不正确! 表名.对应字段");
            }

            string tableName = list[0];
            string fieldName = list[1];

            var dt = DB.Ado
                .GetDataTable("select 1 from {0} where {1}=@FileValue ".FormatOrg(tableName, fieldName),
                      new SugarParameter("@FileValue", Value) //参数
            );

            return dt.Rows.Count >= 1;
        }


        public bool SaveBomFormatDetail(int HeaderId, List<BomFormatDetail> Dtls)
        {
            bool result = false;
            DB.BeginTran();
            try
            {
                if (Dtls.Where(o => o.field_no == "prd_no").Count() <= 0)
                    throw new Exception("必须有[prd_no]字段");
                if (Dtls.Where(o => o.field_no == "dep").Count() <= 0)
                    throw new Exception("必须有[dep]字段");
                if (Dtls.Where(o => o.field_no == "qty").Count() <= 0)
                    throw new Exception("必须有[qty]字段");

                foreach (var item in Dtls.Where(o => o.cell_type != "SHOW"))
                {
                    if (item.diy_type == "3")
                    {
                        CheckFieldExsistInTableStruct("MF_BOM", item.field_no);
                        CheckFieldExsistInTableStruct("TF_BOM", item.field_no);
                    }
                    else if (item.diy_type == "1")
                    {
                        CheckFieldExsistInTableStruct("MF_BOM", item.field_no);
                    }
                    else if (item.diy_type == "1")
                    {
                        CheckFieldExsistInTableStruct("TF_BOM", item.field_no);
                    }
                }

                var oldList = GetDtls(HeaderId);
                foreach (var item in oldList)
                {
                    DB.Deleteable<BomFormatDetail>().Where(o => o.Id == item.Id).ExecuteCommand();
                }

                foreach (var item in Dtls)
                {
                    if (item.check_exist != null && item.check_exist.TrimEx().IsNotEmpty())
                    {
                        var list = item.check_exist.TrimEx().Split('.').ToList();
                        if (list.Count != 2)
                        {
                            throw new Exception("检查资料格式不正确! 表名.对应字段");
                        }

                        CheckFieldExsistInTableStruct(list[0], list[1]);
                    }

                    item.excel_id = HeaderId;
                    DB.Insertable<BomFormatDetail>(item).IgnoreColumns(o => o.Id).ExecuteCommand();
                }

                DB.CommitTran();
                result = true;
            }
            catch (Exception ex)
            {
                DB.RollbackTran();
                throw ex;
            }
            return result;
        }


        /// <summary>
        /// 配方号Excel中未指定，去找子货号的Bom 然后递归检查Excel与系统已有的Bom结构一致吗？
        /// </summary>
        /// <param name="ExcelBomInfo"></param>
        /// <returns></returns>
        public bool ValidateSubBom(BomInfo ExcelBomInfo, bool IsTopLevelBom, bool  IsCheck)
        {
            var prdt = DB.Queryable<PrdtModel>().Where(o => o.prd_no == ExcelBomInfo.prd_no).ToList().FirstOrDefault();
            if (prdt == null)
                throw new MissPrdtException("货品[{0}]不存在!".FormatOrg(ExcelBomInfo.prd_no));
            ExcelBomInfo.prd_knd = prdt.knd;

            if (IsTopLevelBom == true)
            {
                if (ExcelBomInfo.bom_no.IsNullOrEmpty())
                    ExcelBomInfo.bom_no = ExcelBomInfo.prd_no + "->";
            }
            else
            {
                //查 HeaderRow in_no ?
                string headerBomNo = "";
                headerBomNo = ExcelBomInfo.HeadRow.GetStingWithTry("id_no");
                var headerBomInDB = GetBom(ExcelBomInfo.prd_no, headerBomNo);
                if (headerBomInDB != null)
                {
                    ExcelBomInfo.bom_no = headerBomInDB.bom_no;
                }
                else
                {
                    //货号初次新建BOM(Excel上确认了Bom代号)
                    ExcelBomInfo.bom_no = headerBomNo.IsNotEmpty() 
                                                ? headerBomNo 
                                                    : ExcelBomInfo.prd_no + "->";
                }
            }

            foreach (var item in ExcelBomInfo.BodyInfos)
            {
                string belongBomNo = "";
                var belongDataRow = ExcelBomInfo.BodyRows.Where(o => o.GetSting("prd_no") == item.prd_no).First();
                belongBomNo = belongDataRow.GetStingWithTry("id_no");
                var prdt2 = DB.Queryable<PrdtModel>().Where(o => o.prd_no == item.prd_no).ToList().FirstOrDefault();
                if (prdt2 == null)
                    throw new MissPrdtException("货品[{0}]不存在!".FormatOrg(ExcelBomInfo.prd_no));
                item.prd_knd = prdt.knd;

                //比对Excel中的Bom与系统是否一致 (prd_no + qty )
                var subBomInDB = GetBom(item.prd_no, belongBomNo);
                if (subBomInDB != null && false == TFBomIsSame(subBomInDB.TF_BOMS, item.BodyRows))
                {
                    item.bom_no = subBomInDB.bom_no;
                    if (IsCheck == true)
                    {
                        item.HeadRow["check_result"] = "与系统BOM[{0}]不一致".FormatOrg(subBomInDB.bom_no);
                        item.HeadRow["check_err_code"] = "BOM_DIFFERENT";
                        item.HeadRow["check_ask_radio"] = "0";  //1.覆盖 2.升级BOM版本
                        return false;
                    }
                    
                    //如果无冲突处理方式
                    if (item.HeadRow.GetSting("check_ask_radio").IsNullOrEmpty()
                                || item.HeadRow.GetSting("check_ask_radio").GetInt() <= 0)
                    {
                        throw new MissPrdtException("Bom[{0}]未选择冲突处理方式!".FormatOrg(ExcelBomInfo.prd_no));
                    }
                }

                if (subBomInDB != null)
                {
                    item.bom_no = subBomInDB.bom_no;
                }
                else
                {
                    //货号初次新建BOM
                    item.bom_no = belongBomNo.IsNotEmpty() ? belongBomNo : item.prd_no + "->";
                }


                foreach (var item2 in item.BodyInfos)
                {
                    bool isOk = ValidateSubBom(item2, false, IsCheck);
                    if (isOk == false)
                        return false;
                }
            }

            return true;
        }

        public bool ValidateInNoBomConflit(BomInfo ExcelBomInfo)
        {
            foreach (var item in ExcelBomInfo.BodyRows)
            {
                string rowPrdNo = item["prd_no"].ObjToString();
                string belongBomNo = item.GetStingWithTry("id_no");

                var prdt = DB.Queryable<PrdtModel>().Where(o => o.prd_no == rowPrdNo).ToList().FirstOrDefault();
                if (prdt == null)
                    throw new MissPrdtException("货品[{0}]不存在!".FormatOrg(rowPrdNo));

                bool hadExcelBom = ExcelBomInfo.BodyInfos.FirstOrDefault(o => o.prd_no == rowPrdNo) != null ? true : false;

                if (prdt.knd == "2" || prdt.knd == "3")
                {
                    if (belongBomNo.IsNotEmpty())
                    {
                        var mfBom = GetBom(rowPrdNo, belongBomNo);
                        if (mfBom != null)
                            item["id_no"] = mfBom.bom_no;
                        else if (hadExcelBom == true)
                            item["id_no"] = belongBomNo;
                    }
                    else
                    {
                        var mfBom = GetBom(rowPrdNo);
                        if (mfBom != null)
                            item["id_no"] = mfBom.bom_no;
                        else if (hadExcelBom == true)
                            item["id_no"] = rowPrdNo + "->";
                    }
                }
            }

            foreach (var item2 in ExcelBomInfo.BodyInfos)
            {
                bool isOk = ValidateInNoBomConflit(item2);
                if (isOk == false)
                    return false;
            }

            return true;
        }

        public void SetDefaultWh(BomInfo ExcelBomInfo)
        {
            foreach (DataRow item in ExcelBomInfo.BodyRows)
            {
                if (item["wh_no"].ObjToString().IsNotEmpty())
                    continue;

                string wh = GetPrdtWh(item.GetSting("prd_no"));
                item["wh_no"] = wh.IsNotEmpty() ? wh : "0000";
            }

            foreach (var item in ExcelBomInfo.BodyInfos)
            {
                SetDefaultWh(item);
            }
        }

        public MF_BOM_Simple GetBom(string Prd_No, string Bom_No = "")
        {
            var q = DB.Queryable<MF_BOM_Simple>().Where(o => o.prd_no == Prd_No);
            if (Bom_No.IsNotEmpty())
                q.Where(o => o.bom_no == Bom_No);

            var mfs = q.OrderBy(o=>o.pf_no, OrderByType.Desc).ToList();
            MF_BOM_Simple mf = null;
            if (mfs.Count == 0)
                return null;
            if (mfs.Count >= 2)
            {
                if (BomService.BomConflictHandleWay == EnumBomConflictWay.Error) {
                    throw new BomConflictException("货号[{}]存在多个Bom版本".FormatOrg(Prd_No));
                }
            }

            mf = mfs.First();
            mf.TF_BOMS = DB.Queryable<TF_BOM_Simple>().Where(o => o.bom_no == mf.bom_no)
                            .OrderBy(o => o.itm, OrderByType.Desc)
                            .ToList();

            return mf;
        }

        /// <summary>
        /// 取最大的版本号的 BOM
        /// </summary>
        /// <param name="Prd_No"></param>
        /// <param name="Bom_No"></param>
        /// <returns></returns>
        public MF_BOM_Simple GetMaxPF_Bom(string Prd_No)
        {
            var q = DB.Queryable<MF_BOM_Simple>().Where(o => o.prd_no == Prd_No);
            var mf = q.OrderBy(o => o.pf_no, OrderByType.Desc).First();
            return mf;
        }



        /// <summary>
        /// Bom的明细 是否一致(匹配 行数， 每行中的PRD_NO + Qty )
        /// </summary>
        /// <param name="body1"></param>
        /// <param name="body2"></param>
        /// <returns></returns>
        public bool TFBomIsSame(List<TF_BOM_Simple> body1, List<DataRow> body2)
        {
            if (body1.Count != body2.Count)
                return false;

            var list1 = body1.Select(o => o.prd_no + "|" + o.qty).ToList();
            var list2 = body2.Select(o => o["prd_no"] + "|" + o["qty"].GetDouble()).ToList();

            if (list1.Except(list2).Count() > 0)
            {
                return false;
            }

            if (list2.Except(list1).Count() > 0)
            {
                return false;
            }

            return true;
        }

        public string GetPrdtName(string Prd_No)
        {
            var prdt = DB.Queryable<PrdtModel>().Where(o => o.prd_no == Prd_No).ToList().FirstOrDefault();
            if (prdt != null)
                return prdt.name;

            return "";
        }

        public string GetPrdtWh(string Prd_No)
        {
            var prdt = DB.Queryable<PrdtModel>().Where(o => o.prd_no == Prd_No).ToList().FirstOrDefault();
            if (prdt != null)
                return prdt.wh;

            return "";
        }

        public List<string> GetPrdtUTs()
        {
            return DB.Queryable<PrdtModel>().Select(o => o.ut).Distinct().ToList();
        }


        public bool CreateNewPrdt(List<PrdtModel> NewPrdts, List<SqlCommand> defaultValueCmds)
        {
            bool result = false;
            DB.BeginTran();
            try
            {
                foreach (var item in NewPrdts)
                {
                    DB.Insertable<PrdtModel>(item).ExecuteCommand();
                }

                this.ExeCmds(defaultValueCmds);

                DB.CommitTran();
                result = true;
            }
            catch (Exception ex)
            {
                DB.RollbackTran();
                throw ex;
            }
            return result;
        }

        public bool ExeCmds(List<SqlCommand> Cmds)
        {
            return base.ExeCmds(Cmds, P_DB: DB);
        }
    }

    /// <summary>
    /// 层级类型
    /// </summary>
    public enum BomLevelWay
    {
        /// <summary>
        ///   纯数字 1, 2, 3, 4
        /// </summary>
        Number,
        /// <summary>
        /// 带点号 1.1 1.2. 1.2.1
        /// </summary>
        Split
    }
}

//var headers = DB.Queryable<ExcelFormatHeader>().Where(O => O.Id == 100).ToList();

//var tempA = headers.FirstOrDefault();
//tempA.FormatNo += "－复制";

//            DB.UseTran(() =>
//            {
//                tempA.Id = DB.Insertable<ExcelFormatHeader>(tempA).IgnoreColumns(o => o.Id).ExecuteReturnIdentity();
//tempA.FormatNo += "－复制 Update";
//                tempA.PrdNoPos += "－复制 Update";

//                bool change = DB.Updateable<ExcelFormatHeader>(tempA)
//                    .IgnoreColumns(o => o.Id)
//                     .UpdateColumns(o => new { o.FormatNo })
//                    .Where(o => o.Id == tempA.Id)
//                    .ExecuteCommandHasChange();

//                //throw new Exception("");
//                //DB.Deleteable<ExcelFormatHeader>().Where(o => o.Id > 100).ExecuteCommand();
//            }, (e) => {
//                headers = null;
//            });

//            if (headers == null)
//                throw new Exception("aaa");

//            return headers;
