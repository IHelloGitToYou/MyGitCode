using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public class PartEditCommand : ExtjsViewCommand
    {
        public PartEditCommand()
        {
            this.IconCls = "x-fa fa-pencil-square-o";
            this.Text = "编辑";
            this.CommandPath = "ExtGAO.view.part.commands.PartEditCommand";
        }

        public override object Execute(string PostData)
        {
            var newPart = JsonConvert.DeserializeObject<Part>(PostData);
            newPart.PersistStatus = GaoCore.PersistStatus.MODIFY;
            RF<Part>.Save(newPart, LoginUser);
            return newPart;
        }
    }
}
