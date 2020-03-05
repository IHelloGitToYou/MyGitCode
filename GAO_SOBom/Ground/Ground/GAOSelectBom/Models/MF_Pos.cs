using GaoCore;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GAOSelectBom.Models
{
    /// <summary>
    /// 销售订单
    /// </summary>
    [RootEntity]
    [SqlSugar.SugarTable("MF_POS")]
    public class MF_Pos : Entity
    {
        [Label("单据类别")]
        [Require]
        public string OS_ID { get; set; } = "SO";

        [Label("订单号码")]
        [Require]
        public string OS_NO { get; set; } = "";

        [Label("批号")]
        public string BAT_NO { get; set; } = "";

        [Label("订单日期")]
        [Require]
        public DateTime OS_DD { get; set; } = DateTime.Now.Date;



        [Label("预交日")]
        [Require]
        public DateTime EST_DD { get; set; }

        [IsCombobox("NAME", "Cust", "a", "CUS_NO")]
        [Label("客户")]
        [Require]
        public string CUS_NO { get; set; } = "";

        [IsCombobox("NAME", "SALM", "b", "SAL_NO")]
        [Label("业务员")]
        [Require]
        public string SAL_NO { get; set; } = "";

        [IsCombobox("NAME", "DEPT", "c", "DEP")]
        [Label("部门")]
        [Require]
        public string PO_DEP { get; set; } = "";

        [Label("客户订单")]
        public string CUS_OS_NO { get; set; } = "";

        [Label("汇率")]
        [Require]
        public double? EXC_RTO { get; set; } = 1;

        [Label("折扣")]
        [Require]
        public int DIS_CNT { get; set; } = 0;

        [Label("是否已结案")]
        public string CLS_ID { get; set; } = "F";

        [Label("是否历史")] //HIS_PRICE
        public string HIS_PRICE { get; set; } = "F";

        //3. 即收现金  5 其它..
        [Label("交易方式")]
        public string PAY_MTH { get; set; } = "5";

        [Label("交易方式-描述")]
        public string PAY_REM { get; set; } = "";

        [Label("送货方式-备注")]
        public string ADR { get; set; } = "";

        [Label("扣税方式")]// 1.不计税 2.含税价 3.不含税价
        [Require]
        public EnumTax TAX_ID { get; set; } = EnumTax.inTax;

        [Label("立帐方式")]// 1. 记应收帐  2.不立帐  3.开票记账
        [Require]
        public EnumZhangType ZHANG_ID { get; set; } =  EnumZhangType.No;

        [Label("备注")]
        public string REM { get; set; } = "";


        [Label("创建用户")]
        public string USR { get; set; } = "";

        [Label("审核者")]
        public string CHK_MAN { get; set; } = "";


        [Label("创建时间")]
        public DateTime RECORD_DD { get; set; } = DateTime.Now;

        [Label("修改时间")]
        public DateTime EFF_DD { get; set; } = DateTime.Now;


        #region 显示栏位

        [Label("本币别金额")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public decimal SHOW_AMTN { get; set; } = 0.00m; 

        [Label("税额")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public decimal SHOW_TAX { get; set; } = 0.00m;


        [Label("合计")]
        [SqlSugar.SugarColumn(IsIgnore = true)]
        public decimal SHOW_AMT_TATOL { get; set; } = 0.00m;
        #endregion
    }

}
