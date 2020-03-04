using GAOSelectBom.Models;
using System;
using System.Collections.Generic;
using System.Text;
using GAOSelectBom.Services.Parts;
using GaoCore.ViewConfigs;

namespace GAOSelectBom.ViewConfigs
{
    public class PartPrdtViewConfig : GaoCore.EntityViewConfig<PartPrdt>
    {
        public override void OnListView()
        {
            UseCommands(typeof(PartPrdtAddCommand), 
                typeof(PartPrdtDeleteCommand),
                typeof(PartPrdtSaveCommand),

                typeof(ToolbarFillCommand),
                typeof(PartPrdtSelectValidCommand),
                typeof(PartPrdtSelectExceptCommand));

            SetTabTitle("模块货号列表");
            SetTitle("模块货号列表");
            Propery(o => o.PartNo).SetEditable(false);
            Propery(o => o.PrdId).SetComboboxEditor("selectprdteditor")
                                 .SetEditorConfig("valueField", "Id")
                                 .SetComboboxEditorSetter("Prd_No", "PrdNo")
                                 .SetComboboxEditorSetter("Name", "PrdName")
                                 .SetComboboxEditorSetter("Spc", "PrdSpc");
            
            Propery(o => o.PrdName);
            Propery(o => o.PrdSpc);

            Propery(o => o.ValidBomPrdtString).SetEditable(false).UserGrid(TotalWidth: 200);
            Propery(o => o.ExceptBomPrdtString).SetEditable(false).UserGrid(TotalWidth: 150);
        }
    }
}
