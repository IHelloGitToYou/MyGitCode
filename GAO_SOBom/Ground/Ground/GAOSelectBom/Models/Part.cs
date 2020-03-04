using GaoCore;

namespace GAOSelectBom.Models
{
    [RootEntity]
    [SqlSugar.SugarTable("Gao_Part")]
    public class Part : Entity
    {
        [Label("模块代号")]
        [Require]
        public string PartNo { get; set; }

        [Label("选配模块名称")]
        [Require]
        public string PartName { get; set; }

        [IsCombobox("Prd_No", "Prdt", "a")]
        [Label("替代货号Id")]
        [Require]
        public int ReplacePrdId { get; set; }

        [Label("对应替代编码")]
        [Require]
        public string ReplacePrdNo { get; set; }

        [Label("是否隐藏")]
        [Require]
        public bool Disabled { get; set; } = false;

        [Label("排序")]
        [Require]
        public int Sort { get; set; } = 1;
    }
    /// <summary>
    /// 主表: 模块代号PartNo ,选配模块名称PartName 对应替代编码 ReplacePrdNo 是否隐藏 Disabled 排序 Sort [添加,编辑,上移,下移,删除,保存]
    /// </summary>
}
