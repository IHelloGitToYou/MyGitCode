using GAOSelectBom.Models;
using GAOSelectBom.Services.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs
{
    public class PrdtViewConfig : GaoCore.EntityViewConfig<Prdt>
    {
        //public readonly string ViewGroup_SelectPrdt = "SelectPrdt";
        //public override void OnOtherView(string P_ViewGroup)
        //{
        //    SetController("base_controller");
        //    Layout = new GaoCore.ViewConfigs.LayoutFit();
        //    UseCommands(typeof(SelectPrdtAddCommand), typeof(SelectPrdtDeleteCommand));
        //    if (P_ViewGroup == ViewGroup_SelectPrdt)
        //    {
        //        Propery(o => o.Id).SetComboboxEditor("selectprdteditor")
        //                         .SetComboboxEditorSetter("Name", "Name")
        //                         .SetComboboxEditorSetter("Spc", "Spc"); 
        //        //Propery(o => o.Prd_No).SetEditable(false);
        //        Propery(o => o.Name).SetEditable(false);
        //        Propery(o => o.Spc).SetEditable(false);
        //    }
        //}
    }
}
