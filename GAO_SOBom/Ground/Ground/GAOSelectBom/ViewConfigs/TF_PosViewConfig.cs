using GAOSelectBom.Models;
using GAOSelectBom.Services.SO;

namespace GAOSelectBom.ViewConfigs
{
    public class TF_PosViewConfig : GaoCore.EntityViewConfig<TF_Pos>
    {
        public override void OnListView()
        {
            SetTabTitle("");
            SetTitle("");
            UseCommands(typeof(SORowAddCommand), typeof(SORowDeleteCommand));

            Propery(o => o.PRD_NO).SetComboboxEditor("selectprdteditor")
                 .SetEditorConfig("valueField", "Prd_No")
                 .SetComboboxEditorSetter("Name", "PRD_NAME")
                 .SetComboboxEditorSetter("Spc", "SPC");
            Propery(o => o.PRD_NAME);
            Propery(o => o.SPC);

            Propery(o => o.Z_T_PRD_NO).SetComboboxEditor("selectprdteditor")
                                        .SetEditorConfig("valueField", "Prd_No")
                                        .SetComboboxEditorSetter("Name", "Z_T_PRD_NAME")
                                        .SetComboboxDisplayItSelf(true);
            Propery(o => o.Z_T_PRD_NAME);

            //特殊 可能是标准BOM,或 订单BOM ,
            //  所以要屏蔽查询时LeftJoin显示Display,当它是个普通栏位
            Propery(o => o.ID_NO).Editor = "selectbomsoeditor";//SetComboboxEditor("selectbomsoeditor");

            Propery(o => o.Z_T_HIS_ID_NO);

            ///要加载出 选配的控件

            Propery(o => o.FREE_ID).Editor = "checkboxoftfvalueeditor";//  todo T,F 结合 Extjs CheckBox

            Propery(o => o.QTY);
            Propery(o => o.QTY1).UserGrid(Hidden:true);//默认隐藏
            Propery(o => o.WH).SetComboboxEditor("selectwheditor"); //要做仓库控件
            Propery(o => o.PRD_LOC);// todo 储位 控件

            Propery(o => o.UP);
            //Propery(o => o.UP1).UserGrid(Hidden: true);

            Propery(o => o.TAX_RTO);
            Propery(o => o.TAX);
            
            Propery(o => o.AMT);
            Propery(o => o.AMTN);
            Propery(o => o.EST_DD);
        }
    }
}
