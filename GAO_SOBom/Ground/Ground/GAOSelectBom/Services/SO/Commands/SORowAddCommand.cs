
using GaoCore;
using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.Services.SO
{
    public class SORowAddCommand : ExtjsViewCommand
    {
        public SORowAddCommand()
        {
            this.IconCls = "x-fa fa-plus";
            this.Text = "添加";
            this.CommandPath = "ExtGAO.view.so.commands.SORowAddCommand";
        }

        public override object Execute(string PostData)
        {
            return null;
        }
    }
}
