using GAOSelectBom.Models;
using GAOSelectBom.Services.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs
{
    public class PartPrdtDetailViewConfig : GaoCore.EntityViewConfig<PartPrdtDetail>
    {
        public readonly string ViewGroup_SelectPrdt = "SelectPrdt";
        public override void OnOtherView(string P_ViewGroup)
        {
            SetController("base_controller");
            Layout = new GaoCore.ViewConfigs.LayoutFit();
            UseCommands(typeof(SelectPrdtAddCommand), typeof(SelectPrdtDeleteCommand));
            if (P_ViewGroup == ViewGroup_SelectPrdt)
            {
                Propery(o => o.PrdId).SetComboboxEditor("selectprdteditor")
                                  .SetEditorConfig("valueField", "Id")
                                 .SetComboboxEditorSetter("Prd_No", "Prd_No")
                                 .SetComboboxEditorSetter("Name", "PrdName")
                                 .SetComboboxEditorSetter("Spc", "PrdSpc"); 
                //Propery(o => o.Prd_No).SetEditable(false);
                Propery(o => o.PrdName).SetEditable(false);
                Propery(o => o.PrdSpc).SetEditable(false);
            }
        }
    }
}
