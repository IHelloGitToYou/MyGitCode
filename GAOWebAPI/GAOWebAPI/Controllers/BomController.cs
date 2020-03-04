using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAOWebAPI.Models;
using GAOWebAPI.Services;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace GAOWebAPI.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class BomController : BaseController
    {
        BomService Service;
        public BomController()
        {
            Service = new BomService(this);
        }

        #region Excel模版 
        [HttpGet]
        public IEnumerable<ExcelFormatHeader> SearchExcelFormatList(string SearchFormatNo)
        {
            string logId = Get_X_LOGIN_ID();
            var result = Service.SearchExcelFormatList(SearchFormatNo);
            return result;
        }

        [HttpGet]
        public ExcelFormatHeader FetchExcelFormat(string FormatNo)
        {
            var result = Service.FetchExcelFormat(FormatNo);
            return result;
        }


        [HttpPost]
        public ExcelFormatHeader SaveExcelFormat([FromBody]ExcelFormatHeader Header)
        {
            var result = Service.SaveExcelFormat(Header);
            return result;
        }


        [HttpGet]
        public bool RemoveExcelFormat(int idxxx)
        {
            var result = Service.RemoveExcelFormat(idxxx);
            return result;
        }


        [HttpGet]
        public IEnumerable<BomFormatDetail> GetDtls(int HeaderId)
        {
            var result = Service.GetDtls(HeaderId);
            return result;
        }

        [HttpPost]
        public bool SaveBomFormatDetail([FromBody]RData data) //int HeaderId, [FromBody]List<BomFormatDetail> Dtls
        {
            var result = Service.SaveBomFormatDetail(data.HeaderId, data.Dtls);
            return result;
        }

        #endregion


        #region 导入 、检查 BOM 
        [HttpPost]
        public RDataBom LoadExcel()
        {
            string formatNo = Request.Query["formatNo"].ToString();
            if (Request.Form.Files == null || Request.Form.Files.Count == 0)
                throw new Exception("Excel不存在");

            var excelFormat = Service.FetchExcelFormat(formatNo);
            if (excelFormat == null)
            {
                throw new Exception("导入格式 不存在!");
            }

            var sheet = NpoiExcelHelper.GetSheet(Request.Form.Files[0], "0");
            if (sheet == null)
            {
                throw new Exception("Sheet 找不到!");
            }

            RDataBom rInfo = new RDataBom();
            rInfo.prd_no = NpoiExcelHelper.GetCellStringValue(excelFormat.PrdNoPos, sheet);
            rInfo.prd_name = NpoiExcelHelper.GetCellStringValue(excelFormat.PrdNamePos, sheet);
            
            rInfo.dep_no = NpoiExcelHelper.GetCellStringValue(excelFormat.DeptNoPos, sheet);

            var fieldList = Service.GetDtls(excelFormat.Id).OrderBy(o => o.cell_index).ToList();
            DataTable dt = new DataTable();
            foreach (var item in fieldList)
            {
                dt.Columns.Add(item.field_no);
            }
            dt.Columns.Add("bom_level");
            //物料大类 是节点Bom头 设3半成品，其他是4物料
            dt.Columns.Add("bom_prdt_knd");
            dt.Columns.Add("check_result");
            dt.Columns.Add("check_err_code");
            dt.Columns.Add("check_ask_radio");

            for (int i = excelFormat.StartRow - 1; i <= sheet.LastRowNum; ++i)
            {
                var newRow = dt.NewRow();
                var row = sheet.GetRow(i);
                if (row == null) break;

                newRow["bom_level"] = NpoiExcelHelper.GetCellStringValue(row.GetCell(excelFormat.BomStructCell - 1));
                newRow["check_result"] = "";

                string bomLevel = NpoiExcelHelper.GetCellStringValue(row.GetCell(excelFormat.BomStructCell - 1));
                //阶数中断了，代表最后
                if (bomLevel.IsNullOrEmpty() )
                    break;

                foreach (var field in fieldList)
                {
                    string val = NpoiExcelHelper.GetCellStringValue(row.GetCell(field.cell_index - 1));
                    //阶数中断了，代表最后
                    if (val.IsNullOrEmpty() && field.cell_index == excelFormat.BomStructCell)
                    {
                        break;
                    }

                    newRow[field.field_no] = val;
                }
                dt.Rows.Add(newRow);
            }

            rInfo.boms = dt;
            return rInfo;
        }

        [HttpPost]
        public RDataBom ImportToBom([FromBody]RDataBom RDataBom)
        {
            var dtBoms = RDataBom.boms;
            Debug.WriteLine(RDataBom.boms.Rows.Count);

            if (dtBoms == null || dtBoms.Rows.Count <= 0)
                throw new Exception("表格没有记录");

            var excelFormat = Service.FetchExcelFormat(RDataBom.format_no);
            if (excelFormat == null)
            {
                throw new Exception("导入格式 不存在!");
            }
            var excelFormatDtls = Service.GetDtls(excelFormat.Id);

            //检查Bom阶级
            ImportBomFormExcel importTool = new ImportBomFormExcel();
            var headBomRow = dtBoms.NewRow();
            headBomRow["prd_no"] = RDataBom.prd_no;
            headBomRow["dep"] = RDataBom.dep_no;
            headBomRow["name"] = RDataBom.prd_name;

            //dtBoms.Rows.InsertAt(headBomRow, 0);
            if (dtBoms.Columns.IndexOf("id_no") < 0)
                dtBoms.Columns.Add(new DataColumn("id_no"));
            if (dtBoms.Columns.IndexOf("wh_no") < 0)
                dtBoms.Columns.Add(new DataColumn("wh_no"));
            
            ///清空之前的检测结果
            foreach (DataRow row in dtBoms.Rows)
            {
                row["Check_Result"] = "";
                row["check_err_code"] = "";
            }

            BomInfo bomInfo = importTool.DoCheckBomLevel(dtBoms, dtBoms.Rows[0], headBomRow, "", BomLevelType: "Split");

            bool hitError = false;
            //检测ERP中字段是否存在
            var checkFields = excelFormatDtls.Where(o => o.check_exist.IsNotEmpty()).ToList();
            foreach (DataRow item in dtBoms.Rows)
            {
                foreach (var item2 in checkFields)
                {
                    string value = item[item2.field_no].ObjToString();
                    if (value.IsNotEmpty() && Service.CheckValueExsistInTable(item2.check_exist, value) == false)
                    {
                        item["Check_Result"] = "不存在:" + item2.check_exist + " 值:" + value;
                        if(item2.field_no == "prd_no")
                            item["check_err_code"] = "MISS_PRDT";
                        
                        hitError = true;
                        break;
                    }
                }
            }

            if (hitError == false)
            {
                hitError = !Service.ValidateSubBom(bomInfo, true, RDataBom.is_check);
            }

            if (hitError == false)
            {
                hitError = !Service.ValidateInNoBomConflit(bomInfo);
            }

            if (hitError == false)
            {
                Service.SetDefaultWh(bomInfo);
            }

            if (hitError == false && RDataBom.is_check == false)
            {
                var cmds = DoImportToBom(bomInfo, excelFormatDtls, true);
                foreach (var item in cmds)
                {
                    Debug.WriteLine(item.CommandText);
                }
                Service.ExeCmds(cmds);
            }

            if(RDataBom.is_check == false && hitError == true)
            {
                throw new Exception("检测失败不允许导入Bom");
            }

            return RDataBom;
        }


        private List<SqlCommand> DoImportToBom(BomInfo BomInfo, List<BomFormatDetail> ExcelFormatDtls, bool IsTopLevelBom)
        {
            //导入到BOM上，
            //   要不要区分开 T8， Sunlike 不同，做工厂模式？ 答：不用区分，使用：DefaultFieldValue
            //   先从 顶->叶  还是 叶->顶生成 Bom?
            //          非顶级的，要看 子BOM是否存在，如果已存在则不用生成子BOM
            //   字段　SYS + DefaultFieldValue + Z 
            //          SYS + DefaultFieldValue冲突时 考虑优先级
            //      防呆：如何BOM已存在， BOM 是否本人的？  是否已使用[MRP, MO]
            //      In_NO 要不要计算？
            List<SqlCommand> cmds = new List<SqlCommand>();
            string bomNo = BomInfo.bom_no;
            if (bomNo.IsNullOrEmpty())
                throw new Exception("配方号[{0}]为空!".FormatOrg(BomInfo.HeadRow.GetSting("prd_no")));
            bool isNew = Service.GetBom(BomInfo.prd_no, BomInfo.bom_no) == null ? true : false;
            if (IsTopLevelBom == true && isNew == false)
            {
                isNew = true;
                //二次导入时,重构顶级Bom 
                cmds.AddRange(removeBom(bomNo));
            }

            //2019-12-25 考虑升级BOM与覆盖BOM
            if (IsTopLevelBom == false && isNew == false)
            {
                if (BomInfo.HeadRow.GetSting("check_ask_radio").IsNotEmpty()
                     && BomInfo.HeadRow.GetSting("check_ask_radio").GetInt() > 0)
                {
                    //1.覆盖 2.升级BOM版本
                    int bomConflictHandleWay = BomInfo.HeadRow.GetSting("check_ask_radio").GetInt();
                    if (bomConflictHandleWay == 1)
                    {
                        isNew = true;
                        cmds.AddRange(removeBom(bomNo));
                    }
                    else
                    {
                        throw new Exception("BOM冲突未处理!");
                    }
                    //else
                    //{   有问题，如果系统上存在 多个BOM，也会报错
                    //    //升级BOM版本，找出最新的版本号 + 1
                    //    var maxPF_Bom = Service.GetMaxPF_Bom(BomInfo.prd_no);
                    //    int maxPF = maxPF_Bom == null ? 2 : ++maxPF_Bom.pf_no;
                        
                    //}
                }
            }

           
            List<SqlCommand> thisCmds = new List<SqlCommand>();

            if (isNew == true)
            {
                thisCmds.AddRange(createBomHeaderCmd(bomNo, BomInfo, BomInfo.HeadRow, ExcelFormatDtls));
            }

            foreach (BomInfo item in BomInfo.BodyInfos)
            {
                cmds.AddRange(DoImportToBom(item, ExcelFormatDtls, false ));
            }

            if (isNew == true)
            {
                thisCmds.AddRange(createBomBodyCmd(bomNo, BomInfo.BodyRows, ExcelFormatDtls));
            }

            cmds.AddRange(thisCmds);
            return cmds;
        }

        private List<SqlCommand> createBomHeaderCmd(string BomNo, BomInfo BomInfo, DataRow HeadRow, List<BomFormatDetail> ExcelFormatDtls)
        {
            string str = @"Insert Into MF_BOM({0}) values ({1})";
            string strZ = @"Insert Into MF_BOM_Z({0}) values ({1})";

            List<SqlCommand> cmds = new List<SqlCommand>();
            var headerFields = ExcelFormatDtls
                                    .Where(o => o.cell_type == "SYS" && (o.diy_type == "1" || o.diy_type == "3"))
                                    .ToList();

            Dictionary<string, string> dicts = new Dictionary<string, string>();
            dicts.Add("bom_no", BomNo);
            
            foreach (var item in headerFields)
            {
                //Bom头主数量=1
                if(item.field_no == "qty")
                    dicts.Add(item.field_no, "1");
                //货名以 ERP的为准
                else if (item.field_no == "name")
                    dicts.Add(item.field_no, Service.GetPrdtName(HeadRow.GetSting("prd_no")));
                else
                    dicts.Add(item.field_no, HeadRow.GetSting(item.field_no));
            }

            if(dicts.ContainsKey("prd_knd") == false)
                dicts.Add("prd_knd", BomInfo.prd_knd);


            foreach (var item in BomService.DefaultBomHeaderSection)
            {
                //有了设值，覆盖默认值
                if (dicts.ContainsKey(item.Key.ToLower()) == true)
                    continue;

                dicts.Add(item.Key, GetDefaultVariable(item.Value));
            }

            cmds.Add(new SqlCommand(str.FormatOrg(
                            string.Join(",", dicts.Select(o => o.Key)),
                            string.Join(",", dicts.Select(o => "'" + o.Value + "'"))
                    )));



            var headerZFields = ExcelFormatDtls
                                .Where(o => o.cell_type == "DIY" && (o.diy_type == "1" || o.diy_type == "3"))
                                .ToList();

            if(headerZFields.Count > 0)
            {
                Dictionary<string, string> zDicts = new Dictionary<string, string>();
                zDicts.Add("bom_no", BomNo);
                foreach (var item in headerZFields)
                {
                    zDicts.Add(item.field_no, HeadRow.GetSting(item.field_no));
                }

                cmds.Add(new SqlCommand(strZ.FormatOrg(
                          string.Join(",", zDicts.Select(o => o.Key)),
                          string.Join(",", zDicts.Select(o => "'" + o.Value + "'"))
                  )));
            }

            return cmds;
        }

        private List<SqlCommand> createBomBodyCmd(string BomNo, List<DataRow> Bodys, List<BomFormatDetail> ExcelFormatDtls)
        {
            string str = @"Insert Into TF_BOM({0}) values ({1})";
            string strZ = @"Insert Into TF_BOM_Z({0}) values ({1})";

            List<SqlCommand> cmds = new List<SqlCommand>();
            var bodyFields = ExcelFormatDtls
                                .Where(o => o.cell_type == "SYS" && (o.diy_type == "2" || o.diy_type == "3"))
                                .ToList();

            int index = 1;
            foreach (DataRow itemTop in Bodys)
            {
                Dictionary<string, string> dicts = new Dictionary<string, string>();
                dicts.Add("bom_no", BomNo);
                dicts.Add("itm", index.ToString());
                foreach (var item in bodyFields)
                {
                    // 货名以 ERP的为准
                    if (item.field_no == "name")
                        dicts.Add(item.field_no, Service.GetPrdtName(itemTop.GetSting("prd_no")));
                    else
                        dicts.Add(item.field_no, itemTop.GetSting(item.field_no));
                }
                if (bodyFields.FirstOrDefault(o => o.field_no == "id_no") == null)
                    dicts.Add("id_no", itemTop.GetSting("id_no"));


                foreach (var item in BomService.DefaultBomBodySection)
                {
                    //有了设值，覆盖默认值
                    if (dicts.ContainsKey(item.Key.ToLower()) == true)
                        continue;

                    dicts.Add(item.Key, GetDefaultVariable(item.Value));
                }
                cmds.Add(new SqlCommand(str.FormatOrg(
                                string.Join(",", dicts.Select(o => o.Key)),
                                string.Join(",", dicts.Select(o => "'" + o.Value + "'"))
                            )));


                var headerZFields = ExcelFormatDtls
                                        .Where(o => o.cell_type == "DIY" && (o.diy_type == "2" || o.diy_type == "3"))
                                        .ToList();
                if (headerZFields.Count > 0)
                {
                    Dictionary<string, string> zDicts = new Dictionary<string, string>();
                    zDicts.Add("bom_no", BomNo);
                    zDicts.Add("itm", index.ToString());
                    foreach (var item in headerZFields)
                    {
                        zDicts.Add(item.field_no, itemTop.GetSting(item.field_no));
                    }

                    cmds.Add(new SqlCommand(strZ.FormatOrg(
                                  string.Join(",", zDicts.Select(o => o.Key)),
                                  string.Join(",", zDicts.Select(o => "'" + o.Value + "'"))
                                )));
                }

                ++index;
            }
            
            return cmds;
        }

        private List<SqlCommand> removeBom(string BomNo)
        {
            List<SqlCommand> cmds = new List<SqlCommand>();
            var cmd = new SqlCommand("Delete TF_BOM Where Bom_no = '{0}'".FormatOrg(BomNo));
            var cmd2 = new SqlCommand("Delete MF_BOM Where Bom_no = '{0}'".FormatOrg(BomNo));
            
            cmds.Add(cmd);
            cmds.Add(cmd2);
            return cmds;
        }

        #endregion


        [HttpGet]
        public List<string> GetPrdtUTs()
        {
            return Service.GetPrdtUTs();
        }


        [HttpPost]
        public bool CreateNewPrdt([FromBody]List<PrdtModel> NewPrdts) //int HeaderId, [FromBody]List<BomFormatDetail> Dtls
        {
            string str = @"Update Prdt set {1} Where prd_no = '{0}'";
            List<SqlCommand> defaultValueCmds = new List<SqlCommand>();
            if (BomService.DefaultPrdtSection.Count > 0)
            {
                foreach (PrdtModel item in NewPrdts)
                {
                    List<string> setters = new List<string>();
                    foreach (var item2 in BomService.DefaultPrdtSection)
                    {
                        setters.Add("{0} = '{1}'".FormatOrg(item2.Key, GetDefaultVariable(item2.Value)));
                    }

                    defaultValueCmds.Add(new SqlCommand(str.FormatOrg(item.prd_no, string.Join(",", setters))));
                }
            }

            var result = Service.CreateNewPrdt(NewPrdts, defaultValueCmds);
            return result;
        }
        


        private string GetDefaultVariable(string ConfigDefaultValue)
        {
            if (ConfigDefaultValue == "当前日期")
                return DateTime.Now.Date.ToString("yyyy-MM-dd");
            else if (ConfigDefaultValue == "当前时间")
                return DateTime.Now.Date.ToString("yyyy-MM-dd hh:mm:ss");
            else if (ConfigDefaultValue == "登录人")
            {
                string logId = Get_X_LOGIN_ID();
                return LoginService.NowLogins[logId].username;
            }

            return ConfigDefaultValue;
        }

    }

    [Serializable]
    public class RData
    {
        public int HeaderId;
        public List<BomFormatDetail> Dtls;
    }

    [Serializable]
    public class RDataBom
    {
        public string prd_no { get; set; }
        public string prd_name { get; set; }
        
        public string dep_no { get; set; }
        public bool is_check { get; set; }
        public string format_no { get; set; }
        public DataTable boms;
    }
}