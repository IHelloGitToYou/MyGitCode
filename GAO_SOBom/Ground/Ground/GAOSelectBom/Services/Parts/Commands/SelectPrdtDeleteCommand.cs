using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;

namespace GAOSelectBom.Services.Parts
{
    public class SelectPrdtDeleteCommand : ExtjsViewCommand
    {
        public SelectPrdtDeleteCommand()
        {
            IconCls = "x-fa fa-times";
            this.Text = "删除一行";
            this.CommandPath = "ExtGAO.view.part.commands.SelectPrdtDeleteCommand";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
