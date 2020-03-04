using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;

namespace GAOSelectBom.Services.Parts
{
    public class PartPrdtSelectExceptCommand : ExtjsViewCommand
    {
        public PartPrdtSelectExceptCommand()
        {
            this.Text = "排除机型";
            this.CommandPath = "ExtGAO.view.part.commands.PartPrdtSelectExceptCommand";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
