using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using GAOSelectBom.Services.Parts;
using System;
using System.Collections.Generic;
using System.Text;


namespace GAOSelectBom.ViewConfigs
{
    public class MF_PosQueryViewConfig : GaoCore.EntityViewConfig<MF_PosQuery>
    {
        public override void OnDetailView()
        {
            UseCommands(typeof(QueryViewSearchCommand));

            Propery(o => o.os_dd_begin).SetEditorConfig("value",DateTime.Now.GetDateWeekFirstString());
            Propery(o => o.os_dd_end).SetEditorConfig("value", DateTime.Now.GetDateWeekLastString());

            Propery(o => o.os_no);
            Propery(o => o.cus_no);
            Propery(o => o.cus_os_no);
            Propery(o => o.bat_no);
        }


        public override void OnListView()
        {
           
        }
    }
}
