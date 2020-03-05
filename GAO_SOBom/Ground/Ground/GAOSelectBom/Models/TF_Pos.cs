using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    /// <summary>
    /// 销售订单 表身
    /// </summary>
    [RootEntity]
    [SqlSugar.SugarTable("TF_POS")]
    public class TF_Pos : Entity
    {
        [Label("单据类别")]
        [Require]
        public string OS_ID { get; set; } = "SO";

        [Label("订单号码")]
        [Require]
        public string OS_NO { get; set; } = "";

        [Label("订单日期")]
        [Require]
        public DateTime OS_DD { get; set; } = DateTime.Now.Date;

        [Label("行次")]
        [Require]
        public int ITM { get; set; } = -1;


        [Label("品号")]
        [Require]
        [IsCombobox("Prd_No", "Prdt", "a", "Prd_No")]
        public string PRD_NO { get; set; } = "";

        [Label("品名")]
        [Require]
        public string PRD_NAME { get; set; } = "";

        [Label("货品规格")]
        //[ViewAsProperty("prd_no", "Prdt", "b", "Spc", "prd_no")]
        public string SPC { get; set; } = "";

        [Label("特征")]
        [Require]
        public string PRD_MARK { get; set; } = "";

        [Label("仓库")]
        [Require]
        [IsCombobox("WH", "MY_WH", "W", "WH")]
        public string WH { get; set; } = "0000";

        [Label("储位")]
        [Require]
        public string PRD_LOC { get; set; } = "";

        [Label("单位")]
        [Require]
        public string UNIT { get; set; } = "1";

        [Label("数量")]
        [Require]
        public decimal QTY { get; set; } = 0.00m;

        [Label("副数量")]
        [Require]
        public decimal QTY1 { get; set; } = 0.00m;
         
        [Label("单价")]
        [Require]
        public decimal UP { get; set; } = 0.00m;

        //[Label("单价副")]
        //[Require]
        //public decimal UP1 { get; set; } = 0.00m;

        //DIS_CNT AMT AMTN TAX QTY1
        [Label("折扣")]
        [Require]
        public int DIS_CNT { get; set; } = 0;

        [Label("金额")]
        [Require]
        public decimal AMT { get; set; } = 0.00m;

        [Label("本币别金额")]
        [Require]
        public decimal AMTN { get; set; } = 0.00m;

        [Label("税率")]
        [Require]
        public decimal TAX_RTO { get; set; } = 16m;

        [Label("税额")]
        [Require]
        public decimal TAX { get; set; } = 0.00m;
         
        [Label("预交日")]
        [Require]
        public DateTime EST_DD { get; set; }

        [Label("赠品")]
        public string FREE_ID { get; set; } = "";

        [Label("订单配方")]
        //[IsCombobox("BOM_NO", "MF_BOM_SO", "d", "id_no")]
        public string ID_NO { get; set; } = "";

        #region 不明字段

        [Label("XXX行次")]
        public int EST_ITM { get; set; } = 1;

        [Label("XXX2行次")]
        public int PRE_ITM { get; set; } = 1;

        [Label("XXXX单价")]
        [Require]
        public decimal UP_END { get; set; } = 0.00m;

        #endregion

        #region 追加字段

        [Label("虚拟货号*")]
        ///[IsCombobox("Prd_No", "Prdt", "c", "z_t_prd_no")] 不能使用,这是不存在栏位,LeftJoin会出错
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_PRD_NO { get; set; } = "";

        [Label("虚拟货名*")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_PRD_NAME { get; set; } = "";

        [Label("历史配方*")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_HIS_ID_NO { get; set; } = "";

        [Label("选配Json*")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_JSON { get; set; } = "";

        #endregion
    }
}
