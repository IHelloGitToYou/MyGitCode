using GaoCore;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GAOSelectBom.Services.SO
{
    public class SOServices 
    {
        //public string DbName { get; private set; }
        public GaoSqlSugarClient DB { get; private set; }
        public SOServices(GaoSqlSugarClient TranDB)
        {
            DB = TranDB;
        }

        public List<Part> GetAllPart()
        {
            //var DB = Entity.GetDb(DbName);
            return DB.Queryable<Part>().ToList();
        }

        /// <summary>
        /// 把选配项合并Json 拆开
        /// </summary>
        /// <param name="Z_T_JSON"></param>
        /// <returns></returns>
        public Dictionary<Part, string> GetZPartValue(string Z_T_JSON)
        {
            Dictionary<Part, string> result = new Dictionary<Part, string>();
            var a = JsonConvert.DeserializeObject(Z_T_JSON);
            var jObject = ((Newtonsoft.Json.Linq.JObject)a);//.Property("T1").Value
            //var DB = Entity.GetDb(DbName);
            var parts = GetAllPart();
            foreach (var item in parts)
            {
                if (jObject.Property(item.PartNo) != null)
                {
                    result.Add(item, jObject.Property(item.PartNo).Value.ToString());
                }
            }
            return result;
        }

        /// <summary>
        /// 检查BOM模版中 选配项是否对应虚拟件齐全
        /// </summary>
        /// <param name="SamplePartNo"></param>
        /// <param name="PartValues"></param>
        /// <returns>样版BOM</returns>
        public string ValidateBomZParts(string SamplePartNo, Dictionary<Part, string> PartValues)
        {
            //var DB = Entity.GetDb(DbName);
            var tMFBoms = DB.Queryable("MF_BOM", "").Where("PRD_NO = '{0}' and EFF_DD is not null ".FormatOrg(SamplePartNo))
                            .OrderBy("EFF_DD desc ")
                            .ToDataTable();
            if (tMFBoms.Rows.Count == 0)
                throw new InValidException("虚拟成品不存在标准BOM[样版]");

            var TMFBom = tMFBoms.Rows[0];
            string matchBomNo = TMFBom["BOM_NO"].ToString();
            var tTFBoms = DB.Queryable("TF_BOM", "").Where("BOM_NO = '{0}' ".FormatOrg(matchBomNo))
                        .OrderBy("BOM_NO, ITM").ToDataTable();

            foreach (var item in PartValues)
            {
                var tfs = tTFBoms.Select(" PRD_NO = '{0}'".FormatOrg(item.Key.ReplacePrdNo))
                                 .ToList();
                if (tfs.Count == 0)
                    throw new InValidException("虚拟BOM中不存在选配项[{0}]".FormatOrg(item.Key.ReplacePrdNo));
                else if (tfs.Count >= 2)
                    throw new InValidException("虚拟BOM中 选配项[{0}]冲突".FormatOrg(item.Key.ReplacePrdNo));
            }

            return matchBomNo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="SampleBomNo"></param>
        /// <param name="PartValues"></param>
        /// <param name="MF_Pos">订单头,生成BOM提取CUS_NO</param>
        /// <returns></returns>
        //加载样版BOM信息
        // 在TF_BOM中查找这个 Part.ReplacePrdNo 是否存在？ 
        // 替换TF_BOM
        // 生成新的Bom头Prdt No  虚拟成品+流水号？
        // 生成BOM 要不要建实体？ 不建实体,在DataTable操作数据, 再生成SQL
        public Prdt GenarateBomSO(string SampleBomNo, Dictionary<Part, string> PartValues, MF_Pos MF_Pos, string KeepSamePrdtNo)
        {
            var tMFBom = DB.Queryable("MF_BOM", "").Where("BOM_NO = '{0}' ".FormatOrg(SampleBomNo))
                .ToDataTable()
                .Rows[0];
            var tTFBoms = DB.Queryable("TF_BOM", "").Where("BOM_NO = '{0}' ".FormatOrg(SampleBomNo))
                .OrderBy("BOM_NO, ITM").ToDataTable();

            string tPrdNo = tMFBom["PRD_NO"].ToString();
            string newPrdNo = KeepSamePrdtNo;
            //todo: 新的货名 Name+选配1的简称？
            string newPrdtName = CalcNewPrdtName(GetPrdt(tPrdNo).Name, PartValues);

            ///重建BOM 只在这个货号
            if (KeepSamePrdtNo.IsNullOrEmpty())
            {
                newPrdNo = GenarateNewBomPrdtNo(tPrdNo);
                CreatePrdtFromTPrdt(tPrdNo, newPrdNo, newPrdtName);
            }
            else
            {
                //只更新货名
                DB.Ado.ExecuteCommand(" Update Prdt set Name = '{0}' where PRD_NO='{1}'"
                                        .FormatOrg(newPrdNo, newPrdtName));
            }
            

            tMFBom["PRD_NO"] = newPrdNo;
            tMFBom["BOM_NO"] = newPrdNo + "->";
            tMFBom["RECORD_DD"] = DateTime.Now;
            tMFBom["NAME"] = newPrdtName;

            tMFBom["EFF_DD"] = DateTime.Now;
            tMFBom["CLS_DATE"] = DateTime.Now.Date;
            tMFBom["CHK_MAN"] = "1";

            foreach (var item in PartValues)
            {
                var tf = tTFBoms.Select(" PRD_NO = '{0}'".FormatOrg(item.Key.ReplacePrdNo))
                              .First();

                var tfPrdt = GetPrdt(item.Value);
                tf["PRD_NO"] = tfPrdt.Prd_No;
                tf["NAME"] = tfPrdt.Name;
            }

            foreach (DataRow item in tTFBoms.Rows)
            {
                item["BOM_NO"] = tMFBom["BOM_NO"];
                item["START_DD"] = new DateTime(2000, 01, 01);
                item["EFF_DD"] = new DateTime(2000, 01, 01);
                item["CHG_DD"] = new DateTime(2000, 01, 01);
            }

            SyncToBOMSO( tMFBom, tTFBoms, MF_Pos);

            Prdt newPrt = GetPrdt(newPrdNo);
            return newPrt;// tMFBom["BOM_NO"].ToString();
        }


        public string GenarateNewBomPrdtNo( string TPrdtNo)
        {
            var counterService = new NumberCounterService(DB);
            int now = counterService.NextCount(TPrdtNo);

            string str = TPrdtNo + now.ToString().PadLeft(5, '0');
            return str;

            ////var maxPrdt = DB.Queryable<Prdt>()
            ////    .Where(" prd_no like '{0}%' and prd_no <> '{0}' ".FormatOrg(TPrdtNo))
            ////    .First();

            ////int idx = 0;
            ////if(maxPrdt != null)
            ////{
            ////    idx = int.Parse(maxPrdt.Prd_No.Substring(TPrdtNo.Length + 1, 5));
            ////}

            //////return TPrdtNo + string.Format( "00007"; 
        }

        /// <summary>
        ///  创建新的货号 Copy虚拟代号,,变 CHK_MAN,USR, EFF_DD,CLS_DATE,RECORD_DD
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="TPrdtNo"></param>
        /// <param name="NewPrdtNo"></param>
        /// <param name="NewPrdtName"></param>
        /// <returns>新的Prdt.Id</returns>
        public int CreatePrdtFromTPrdt(string TPrdtNo, string NewPrdtNo, string NewPrdtName)
        {
            var dt = DB.Queryable("PRDT", "")
                            .Where("PRD_NO = '{0}'".FormatOrg(TPrdtNo))
                            .ToDataTable();// ("SELECT * FROM PRDT WHERE PRD_NO = '{0}'".FormatOrg(TPrdtNo)).ToDataTable();
            if (dt.Rows.Count <= 0)
                throw new InValidException("货号[{0}]不存在".FormatOrg(TPrdtNo));
            
            Dictionary<string, object> newPrdt = new Dictionary<string, object>();
            foreach (DataColumn item in dt.Columns)
            {
                newPrdt.Add(item.ColumnName, dt.Rows[0][item.ColumnName]);
            }
            newPrdt["PRD_NO"] = NewPrdtNo;
            newPrdt["NAME"] = NewPrdtName;

            newPrdt["CHK_MAN"] = "1";
            newPrdt["USR"] = "1";
            newPrdt["EFF_DD"] = DateTime.Now;
            newPrdt["CLS_DATE"] = DateTime.Now.Date;
            newPrdt["RECORD_DD"] = DateTime.Now;
            
            return DB.Insertable(newPrdt).AS("PRDT").IgnoreColumns(true).IgnoreColumns("Id").ExecuteReturnIdentity();
        }

        private Prdt GetPrdt(string PrdNo)
        {
           return  DB.SqlQueryable<Prdt>(@"select t.id, t.Prd_No, t.Name, T.snm, T.Spc, 
                                    -1 CreateId, t.Record_dd CreateDD,
                                    -1 UpdateId, t.Record_dd UpdateDD
                                    from Prdt t")
                                .Where(O => O.Prd_No == PrdNo).First();
        }

        /// <summary>
        ///  todo: 新的货名 Name+选配1的简称？
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="SroucePrdtName"></param>
        /// <param name="PartValues"></param>
        /// <returns></returns>
        private string CalcNewPrdtName( string SroucePrdtName, Dictionary<Part, string> PartValues)
        {
            string str = string.Empty;
            foreach (var item in PartValues)
            {
                var prdt1 = GetPrdt(item.Value);
                str += prdt1.Snm.IsNotEmpty() ? prdt1.Snm : prdt1.Name;
            }
            return SroucePrdtName + " " +str;
        }

        readonly string BomToBomSoField = @"BOM_NO,NAME,PRD_NO,PRD_MARK,PF_NO,WH_NO,PRD_KND,UNIT,QTY,QTY1,CST_MAKE,CST_PRD,CST_MAN,CST_OUT,USED_TIME,CST,USR_NO,TREE_STRU,DEP,PHOTO_BOM,EC_NO,VALID_DD,END_DD,REM,USR,CHK_MAN,PRT_SW,CPY_SW,CLS_DATE,LOCK_MAN,TIME_CNT,MAN_CNT,MCH_CNT,QTY_DAY,CST_DMAKE,CST_DMAN,CST_DPRD,CST_DOUT,EFF_DD,UPD_DD,MD_NO,RECORD_DD,GY_NAME,UPD_USR";
        //,SPC,CREATE_DD 不存在mf_bom_so
        readonly string BomTFToBomSoTF = @"BOM_NO,ITM,PRD_NO,PRD_MARK,ID_NO,NAME,WH_NO,BOM_ID,UNIT,QTY,QTY1,LOS_RTO,CST,PRD_NO_UP,EXP_ID,PRD_NO_CHG,REM,START_DD,END_DD,ZC_NO,TW_ID,USEIN_NO,QTY_BAS,PZ_ID,CST_MAKE,CST_MAN,CST_PRD,CST_OUT,UP_CST,CHG_DD,EFF_DD,EC_NO,CCC,WHEREUSE,OUTPUT_RTO,FORMULA_CSV,WHEREUSE_REM";


        public bool SyncToBOMSO(DataRow MF, DataTable TF_BOMS, MF_Pos MF_Pos)
        {
            var bomFields = BomToBomSoField.Split(",");
            Dictionary<string, object> mfBomSo = new Dictionary<string, object>();
            foreach (var item in bomFields)
            {
                System.Diagnostics.Debug.WriteLine(item);
                mfBomSo.Add(item, MF[item]);
            }
            mfBomSo["CUS_NO"] = MF_Pos.CUS_NO;

            List<Dictionary<string, object>> tfBomSos = new List<Dictionary<string, object>>();
            foreach (DataRow item in TF_BOMS.Rows)
            {
                Dictionary<string, object> tfBomSo = new Dictionary<string, object>();
                foreach (var item2 in BomTFToBomSoTF.Split(","))
                {
                    if (item2 == "PZ_ID" || item2 == "EC_NO"
                        || item2 == "OUTPUT_RTO" || item2 == "WHEREUSE_REM")
                        continue;

                    tfBomSo.Add(item2, item[item2]);
                }

                tfBomSos.Add(tfBomSo);
            }

            DB.Insertable(mfBomSo).AS("MF_BOM_SO").IgnoreColumns(true).ExecuteCommand();
            foreach (var item in tfBomSos)
            {
                DB.Insertable(item).AS("TF_BOM_SO").IgnoreColumns(true).ExecuteCommand();
            }
            ///DB.Updateable(mfBomSo).WhereColumns("BOM_NO");
            return true;
        }

        public bool SoIsCheck(string OS_NO)
        {
            var so = DB.Queryable<MF_Pos>().Where(o => o.OS_NO == OS_NO ).First();
            if (so == null)
                throw new InValidException("销售订单[{0}]不存在".FormatOrg(OS_NO));

            return so.CHK_MAN.IsNotEmpty() ? true : false;
        }


        public bool SoHasSAOperate(string OS_NO, int ITM)
        {
            return false;
            //var DB = Entity.GetDb(DbName);

            //var so = DB.Queryable<MF_Pos>().Where(o => o.OS_NO == OS_NO).First();
            //if (so == null)
            //    throw new InValidException("销售订单[{0}]不存在".FormatOrg(OS_NO));

            //return so.CHK_MAN.IsNotEmpty() ? true : false;
        }

        public string GetNewOSNo(DateTime Day, string SOFormat) {
            //SOYMDDNNNN ->SO 0222 0001
            int yIndex = SOFormat.IndexOf("Y");
            int nIndex = SOFormat.IndexOf("N");

            string fixHeader = SOFormat.Substring(0, yIndex );// "SO";
            string dateFormat = SOFormat.Substring(yIndex, nIndex - yIndex);// "YMDD";
            int numLen = SOFormat.Length - nIndex;

            string str = fixHeader + getOS_NODate(dateFormat, Day);
            string maxSONo = DB.Queryable<MF_Pos>()
                            .Where(" OS_NO like '{0}%' and OS_NO <> '{0}' ".FormatOrg(str))
                            .OrderBy(o => o.OS_NO, type: OrderByType.Desc)
                            .Select(o => o.OS_NO)
                            .First();
            
            if (maxSONo.IsNullOrEmpty())
                return str + 1.ToString().PadLeft(numLen, '0');
            else
            {
                int currentIndex = 1;

                string numStr = maxSONo.Substring(str.Length).ToString();
                if (numStr.IsNumber() == false)
                    throw new InvalidCastException("单据号码异常[不能生成单据号码]");

                currentIndex = numStr.ObjToInt()+1;
                return str + currentIndex.ToString().PadLeft(numLen, '0');
            }

        }

        private string getOS_NODate(string DateFormat , DateTime Day)
        {
            if(DateFormat == "YMDD")
            {
                return Day.Year.ToString().Substring(3) + "" 
                    + getM(Day.Month) + "" 
                    + Day.Day.ToString().PadLeft(2, '0');
            }
            else if (DateFormat == "YMMDD")
            {
                return Day.Year.ToString().Substring(3) + ""
                    + Day.Month.ToString().PadLeft(2, '0') + ""
                    + Day.Day.ToString().PadLeft(2, '0');
            }
            else if (DateFormat == "YYMMDD")
            {
                return Day.Year.ToString().Substring(2) + "" 
                    + Day.Month.ToString().PadLeft(2, '0') + "" 
                    + Day.Day.ToString().PadLeft(2, '0');
            }
            else if (DateFormat == "YYYYMMDD")
            {
                return Day.Year.ToString() + ""
                    + Day.Month.ToString().PadLeft(2, '0') + ""
                    + Day.Day.ToString().PadLeft(2, '0');
            }

            throw new InvalidCastException("日期格式[{0}]未识别".FormatOrg(DateFormat));
        }

        private string getM(int Mounth)
        {
            if (Mounth < 10)
                return Mounth.ToString();
            else if (Mounth == 10)
                return "A";
            else if (Mounth == 11)
                return "B";
            else if (Mounth == 12)
                return "C";
            
            return "P";
        }
    }
}
