using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs
{
    public class PartQueryViewConfig : GaoCore.EntityViewConfig<PartQuery>
    {
        public PartQueryViewConfig()
        {
            UseCommands(typeof(QueryViewSearchCommand));
        }

        public override void OnDetailView()
        {
            Layout = new LayoutForm();
            Propery(o => o.PartNo);
            Propery(o => o.PartName);
        }
    }
}
