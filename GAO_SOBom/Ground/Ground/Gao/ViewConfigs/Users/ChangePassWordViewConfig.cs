using Gao.Models;
using GaoCore.ViewConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.ViewConfigs
{
    public class ChangePassWordViewConfig : GaoCore.EntityViewConfig<ChangePassWordViewModel>
    {
        public ChangePassWordViewConfig()
        {
             
        }

        public override void OnDetailView()
        {
            Layout = new LayoutForm();
            Propery(o => o.PassWork).SetEditorConfig("inputType", "password");
            Propery(o => o.NewPassWork).SetEditorConfig("inputType", "password");
            Propery(o => o.NewPassWork2).SetEditorConfig("inputType", "password");
        }
    }
}
