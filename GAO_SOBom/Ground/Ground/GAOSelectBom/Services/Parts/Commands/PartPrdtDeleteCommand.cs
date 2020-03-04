using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public class PartPrdtDeleteCommand : ExtjsViewCommand
    {
        public PartPrdtDeleteCommand()
        {
            IconCls = "x-fa fa-times";
            this.Text = "删除一行";
            this.CommandPath = "ExtGAO.view.part.commands.PartPrdtDeleteCommand";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
