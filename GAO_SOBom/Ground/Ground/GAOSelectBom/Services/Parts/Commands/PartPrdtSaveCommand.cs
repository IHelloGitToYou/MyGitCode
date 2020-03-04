using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public class PartPrdtSaveCommand : ExtjsViewCommand
    {
        public PartPrdtSaveCommand()
        {
            IconCls = "x-fa fa-floppy-o";
            this.Text = "保存";
            this.CommandPath = "ExtGAO.view.part.commands.PartPrdtSaveCommand";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
