using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;

namespace GAOSelectBom.Services.Parts
{
    public class SelectPrdtAddCommand : ExtjsViewCommand
    {
        public SelectPrdtAddCommand()
        {
            this.IconCls = "x-fa fa-plus-square-o";
            this.Text = "创建一行";
            this.CommandPath = "ExtGAO.view.part.commands.SelectPrdtAddCommand";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
