
using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.SO
{
    public class SORowDeleteCommand : ExtjsViewCommand
    {
        public SORowDeleteCommand()
        {
            this.IconCls = "x-fa fa-times";
            this.Text = "删除";
            this.CommandPath = "ExtGAO.view.so.commands.SORowDeleteCommand";
        }

        public override object Execute(string PostData)
        {
            return null;
        }
    }
}
