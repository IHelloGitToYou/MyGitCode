using GaoCore.ViewConfigs;
using GAOSelectBom.Models.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs.Bases
{

    public class BomSOQueryViewConfig : GaoCore.EntityViewConfig<BomSOQuery>
    {
        public BomSOQueryViewConfig()
        {
            UseCommands(typeof(QueryViewSearchCommand));
        }

        public override void OnDetailView()
        {
            Propery(o => o.SearchBom).SetEditable(true); 
            Propery(o => o.BOM_NO).SetEditable(true);
            Propery(o => o.PRD_NO).SetEditable(true);
            Propery(o => o.NAME).SetEditable(true);
            //{
            //    xtype: 'radiofield',
            //    boxLabel: '订单BOM',
            //    name: 'size',
            //    inputValue: 'BOM_SO',
            //    height: 30
            //}
            //Propery(o => o.SearchBom).SetEditable(true)
            //    .SetEditorConfig("boxLabel", "")//标准BOM
            //    .SetEditorConfig("name", "SearchBom")
            //    .SetEditorConfig("inputValue", true)
            //    .Editor = "radiofield";

            //Propery(o => o.SearchBomSO).SetEditable(true)
            // .SetEditorConfig("boxLabel", "")//订单BOM
            // .SetEditorConfig("name", "SearchBomSO")
            // .SetEditorConfig("inputValue", false)
            // //.SetEditorConfig("inputValue", "SearchBom")
            // .Editor = "radiofield";

        }
    }

}
