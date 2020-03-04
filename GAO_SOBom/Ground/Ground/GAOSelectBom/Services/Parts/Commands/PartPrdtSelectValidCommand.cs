using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;

namespace GAOSelectBom.Services.Parts
{
    public class PartPrdtSelectValidCommand : ExtjsViewCommand
    {
        public PartPrdtSelectValidCommand()
        {
            this.Text = "可选机型";
            this.CommandPath = "ExtGAO.view.part.commands.PartPrdtSelectValidCommand";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
