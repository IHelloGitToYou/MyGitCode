using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.Parts
{
    public class PartDeleteCommand : ExtjsViewCommand
    {
        public PartDeleteCommand()
        {
            IconCls = "x-fa fa-times";
            this.Text = "删除";
            this.CommandPath = "ExtGAO.view.part.commands.PartDeleteCommand";
        }

        public override object Execute(string PostData)
        {
            //todo 检测是否已使用？
            Part part = JsonConvert.DeserializeObject<Part>(PostData);
            part.PersistStatus = GaoCore.PersistStatus.DELETED;
            part = RF<Part>.Save(part, LoginUser);

            return part;
        }
    }
}
