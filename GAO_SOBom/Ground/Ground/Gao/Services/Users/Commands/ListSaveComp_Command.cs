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
    public class ListSaveComp_Command : ExtjsViewCommand
    {
        public ListSaveComp_Command()
        {
            this.Text = "保存";
            this.CommandPath = "ExtGAO.view.user.commands.ListSaveComp_Command";
        }

        public override object Execute(string PostData)
        {
            return "";
            //var newUser = JsonConvert.DeserializeObject<User>(PostData);
            //newUser.PersistStatus = GaoCore.PersistStatus.NEW;
            //RF<User>.Save(newUser);
            //return newUser;
        }
    }
}
