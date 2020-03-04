using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Services
{
    public class BomLevelBase//: BomLevelInterface
    {
        public int Count { get; set; }
        public BomLevelBase(string BomLevelString) { }

        public virtual int GetCount()
        {
            return -1;
        }

        public virtual bool IsSame(BomLevelBase blBase2)
        {
            return false;
        }

        public virtual bool IsChild(BomLevelBase blBase2)
        {
            return false;
        }
    }

    /// <summary>
    /// 带. 分层
    /// </summary>
    public class BomLevelBaseSplit : BomLevelBase
    {
        List<int> Levels = new List<int>();
        public BomLevelBaseSplit(string BomLevelString) : base(BomLevelString)
        {
            if (BomLevelString == string.Empty)
            {
                Count = 0;
                return;
            }

            Levels = BomLevelString.Trim().Replace(" ", "").Split('.').Select(o => int.Parse(o)).ToList();
            Count = Levels.Count;
        }

        public int GetCount()
        {
            return Count;
        }

        public override bool IsSame(BomLevelBase Child2)
        {
            var child1 = this; //(BomLevelBaseSplit)Child1;
            var child2 = (BomLevelBaseSplit)Child2;

            if (child1.Count != child2.Count)
                return false;

            int cnt = child1.Count;
            string child1Str = string.Join(".", child1.Levels.Take(cnt - 1)).ToString();
            string child2Str = string.Join(".", child2.Levels.Take(cnt - 1)).ToString();
            bool result = child2Str.StartsWith(child1Str);

            return result;
        }

        public override bool IsChild(BomLevelBase Child)
        {
            var father = this;// (BomLevelBaseSplit)Father;
            var child = (BomLevelBaseSplit)Child;

            if (father.Count != child.Count - 1)
                return false;
            if (father.Count == 0)
                return true;

            string fatherStr = string.Join(".", father.Levels).ToString();
            string childStr = string.Join(".", child.Levels).ToString();
            bool result = childStr.StartsWith(fatherStr);

            return result;
        }

        public override string ToString()
        {
            return string.Join(".", Levels).ToString();
        }
    }

    /// <summary>
    /// 按数字分层的
    /// </summary>
    public class BomLevelNumber : BomLevelBase
    {
        public BomLevelNumber(string BomLevelString) : base(BomLevelString)
        {
            if (string.IsNullOrEmpty(BomLevelString.Trim()))
                Count = 0;
            else
                Count = int.Parse(BomLevelString.Trim());
        }

        public int GetCount()
        {
            return Count;
        }

        public override bool IsSame(BomLevelBase Child2)
        {
            var child1 = this; //(BomLevelBaseSplit)Child1;
            var child2 = (BomLevelNumber)Child2;


            if (child1.Count == Child2.Count)
                return true;

            return false;
        }

        public override bool IsChild(BomLevelBase Child)
        {
            var father = this;// (BomLevelBaseSplit)Father;
            var child = (BomLevelNumber)Child;

            if (father.Count == 0 && child.Count == 1)
                return true;

            if (father.Count == child.Count - 1)
                return true;

            return false;
        }

        public override string ToString()
        {
            return Count.ToString();
        }
    }

    public class BomLevelFactory
    {
        public static BomLevelBase Get(string BomLevelType, string BomLevelString)
        {
            if (BomLevelType == "Split")
            {
                return new BomLevelBaseSplit(BomLevelString);
            }
            else if (BomLevelType == "Number")
            {
                return new BomLevelNumber(BomLevelString);
            }

            throw new Exception("找不到阶级分层方法" + BomLevelType);
        }
    }

    public class BomInfo
    {
        public BomInfo()
        {
            BodyInfos = new List<BomInfo>();
            BodyRows = new List<DataRow>();
        }

        public bool NeedCreate { get; set; }

        public string bom_no { get; set; }
        public string prd_no { get; set; }
        public string prd_knd { get; set; }

        public DataRow HeadRow;
        /// <summary>
        /// 注意有可能 半成品 在Excel中无维护，但系统中实际已经有BodyRows
        /// </summary>
        public List<DataRow> BodyRows;

        public List<BomInfo> BodyInfos;
    }

    public class ImportBomFormExcel
    {
        public BomInfo DoCheckBomLevel(DataTable dt, DataRow StartCurrentRow, DataRow BomHeadRow, string PreFix = "", string BomLevelType = "Split")
        {
            BomInfo bomInfo = new BomInfo(); 
            if (BomHeadRow == null)
                throw new Exception("Bom头不能为空");

            bomInfo.HeadRow = BomHeadRow;
            bomInfo.prd_no = BomHeadRow["prd_no"].ToString();
            if (dt.Columns.IndexOf("bom_no") > -1)
            {
                bomInfo.bom_no = Common.ObjToString(BomHeadRow["bom_no"]); //配方号不是必须字段
            }

            int startIndex = 0;
            if (StartCurrentRow != null)
                startIndex = dt.Rows.IndexOf(StartCurrentRow);
            
            var headLevel = BomLevelFactory.Get(BomLevelType, PreFix);// GetIndexs(PreFix);
            List<DataRow> bomDetails = new List<DataRow>();
            for (int i = startIndex; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                
                if (row["Check_Result"].ToString() != string.Empty)
                    continue;

                var rowLevel = BomLevelFactory.Get(BomLevelType, row["bom_level"].ToString());  //GetIndexs(row["bom_level"].ToString());
                if (headLevel.Count > rowLevel.Count || (rowLevel.Count - headLevel.Count) > 1)
                {
                    row["Check_Result"] = "阶级异常";
                    throw new Exception("阶级异常");
                }

                //相同级别就跳走
                if (headLevel.IsSame(rowLevel))
                {
                    break;
                }

                if (headLevel.IsChild(rowLevel))
                {
                    BomHeadRow["bom_prdt_knd"] = "3";
                    if (bomDetails.Select(o => o.GetSting("prd_no")).ToList()
                        .Contains(row.GetSting("prd_no")))
                    {
                        row["Check_Result"] = "货号相同";
                        throw new Exception("货号相同{0}".FormatOrg(row.GetSting("prd_no")));
                    }

                    row["Check_Result"] = BomHeadRow["bom_level"].ToString() + "阶级成功";
                    bomDetails.Add(row);
                    bomInfo.BodyRows.Add(row);
                }

                //最后一行了
                if (i + 1 >= dt.Rows.Count)
                    break;

                var nextRow = dt.Rows[i + 1];
                var nextRowLevel = BomLevelFactory.Get(BomLevelType, nextRow["bom_level"].ToString()); //GetIndexs(nextRow["bom_level"].ToString());
                                                                                                       //不是自己的子级，跳出 例: 1.2 -> 2
                if (nextRowLevel.Count < headLevel.Count || nextRowLevel.Count < rowLevel.Count)
                    break;

                //下一级是否同级
                if (headLevel.IsChild(nextRowLevel))
                {
                    continue;
                }
                else if (rowLevel.IsChild(nextRowLevel))
                {
                    row["bom_prdt_knd"] = "3888";
                    string str = rowLevel.ToString();   //string.Join(".", rowLevel)
                    var subBomInfo = DoCheckBomLevel(dt, nextRow, row, PreFix = str, BomLevelType: BomLevelType);
                    bomInfo.BodyInfos.Add(subBomInfo);
                }
                else
                {
                    nextRow["Check_Result"] = "阶级异常";
                    throw new Exception("阶级异常");
                }
            }

            return bomInfo;
        }

        //public void DoCheckBomLevel(DataTable dt, DataRow StartCurrentRow, DataRow BomHeadRow, string PreFix = "")
        //{
        //    if (BomHeadRow == null)
        //        throw new Exception("Bom头不能为空");

        //    int startIndex = 0;
        //    if (StartCurrentRow != null)
        //        startIndex = dt.Rows.IndexOf(StartCurrentRow);

        //    var headLevel = GetIndexs(PreFix);
        //    List<DataRow> bomDetails = new List<DataRow>();
        //    for (int i = startIndex; i < dt.Rows.Count; i++)
        //    {
        //        var row = dt.Rows[i];
        //        //if (row["bom_level"].ToString() == "6.1")
        //        //{
        //        //    int a = 123;
        //        //}
        //        if (row["check_result"].ToString() != string.Empty)
        //            continue;

        //        var rowLevel = GetIndexs(row["bom_level"].ToString());
        //        if (headLevel.Count > rowLevel.Count || (rowLevel.Count - headLevel.Count) > 1)
        //        {
        //            row["check_result"] = "阶级异常";
        //            throw new Exception("阶级异常");
        //        }

        //        //相同级别就跳走
        //        if (IsSame(headLevel, rowLevel))
        //        {
        //            break;
        //        }

        //        if (IsChild(headLevel, rowLevel))
        //        {
        //            row["check_result"] = BomHeadRow["bom_level"].ToString() + "阶级成功";
        //            bomDetails.Add(row);
        //        }

        //        //最后一行了
        //        if (i + 1 >= dt.Rows.Count)
        //            break;

        //        var nextRow = dt.Rows[i + 1];
        //        var nextRowLevel = GetIndexs(nextRow["bom_level"].ToString());
        //        //不是自己的子级，跳出 例: 1.2 -> 2
        //        if (nextRowLevel.Count < headLevel.Count || nextRowLevel.Count < rowLevel.Count)
        //            break;

        //        //下一级是否同级
        //        if (IsChild(headLevel, nextRowLevel))
        //        {
        //            continue;
        //        }
        //        else if (IsChild(rowLevel, nextRowLevel))
        //        {
        //            string str = string.Join(".", rowLevel).ToString();
        //            DoCheckBomLevel(dt, nextRow, row, PreFix = str);
        //        }
        //        else
        //        {
        //            nextRow["check_result"] = "阶级异常";
        //            throw new Exception("阶级异常");
        //        }
        //    }
        //}

        //public List<int> GetIndexs(string BomLevel)
        //{
        //    if (BomLevel == string.Empty)
        //        return new List<int>();

        //    return BomLevel.Trim().Replace(" ", "").Split('.').Select(o => int.Parse(o)).ToList();
        //}

        //public static bool IsChild(List<int> Father, List<int> Child)
        //{
        //    //"" ->  "1"  = "" -> "1"
        //    //"1" -> "1.1"= 1 -> 1.1
        //    //"1" -> "2.1"
        //    if (Father.Count != Child.Count - 1)
        //        return false;
        //    if (Father.Count == 0)
        //        return true;

        //    string fatherStr = string.Join(".", Father).ToString();
        //    string childStr = string.Join(".", Child).ToString();
        //    bool result = childStr.StartsWith(fatherStr);

        //    return result;
        //}

        ///// <summary>
        ///// 是否同级
        ///// </summary>
        ///// <param name="Child1">1.2.3</param>
        ///// <param name="Child2">1.2.4</param>
        ///// <returns></returns>
        //public static bool IsSame(List<int> Child1, List<int> Child2)
        //{

        //    if (Child1.Count != Child2.Count)
        //        return false;

        //    int cnt = Child1.Count;
        //    string child1Str = string.Join(".", Child1.Take(cnt - 1)).ToString();
        //    string child2Str = string.Join(".", Child2.Take(cnt - 1)).ToString();
        //    bool result = child2Str.StartsWith(child1Str);

        //    return result;
        //}
    }
   
}
