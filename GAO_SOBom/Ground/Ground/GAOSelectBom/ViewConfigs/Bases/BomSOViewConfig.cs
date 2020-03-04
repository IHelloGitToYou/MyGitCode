using GAOSelectBom.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs.Bases
{
    public class BomSOViewConfig : GaoCore.EntityViewConfig<BomSO>
    {
        public override void OnListView()
        {
            QueryView = new BomSOQueryViewConfig();
            QueryView.OnDetailView();

            Propery(o => o.BOM_NO).SetEditable(false);
            Propery(o => o.PRD_NO).SetEditable(false);
            Propery(o => o.NAME).SetEditable(false);
        }
    }
}
