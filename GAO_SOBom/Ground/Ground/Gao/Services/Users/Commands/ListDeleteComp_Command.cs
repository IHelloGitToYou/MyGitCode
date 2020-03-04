using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Gao.Models;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;

namespace Gao.Services.Users.Commands
{
    public class ListDeleteComp_Command : ExtjsViewCommand
    {
        public ListDeleteComp_Command()
        {
            this.Text = "删除";
            this.CommandPath = "ExtGAO.view.user.commands.ListDeleteComp_Command";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
