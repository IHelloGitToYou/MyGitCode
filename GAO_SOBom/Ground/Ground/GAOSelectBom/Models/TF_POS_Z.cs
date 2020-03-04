using GaoCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("TF_POS_Z")]
    public class TF_Pos_Z : Entity
    {
        [Label("单据类别")]
        [Require]
        public string OS_ID { get; set; } = "SO";

        [Label("订单号码")]
        [Require]
        public string OS_NO { get; set; } = "";

        [Label("行次")]
        [Require]
        public int ITM { get; set; } = -1;

        [Label("虚拟货号*")]
        //[IsCombobox("Prd_No", "Prdt", "c", "z_t_prd_no")]
        //[SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_PRD_NO { get; set; } = "";

        [Label("虚拟货名*")]
        //[SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_PRD_NAME { get; set; } = "";

        [Label("历史配方*")]
        //[SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_HIS_ID_NO { get; set; } = "";

        [Label("选配Json*")]
        //[SqlSugar.SugarColumn(IsIgnore = true)]
        public string Z_T_JSON { get; set; } = "";
    }
}
