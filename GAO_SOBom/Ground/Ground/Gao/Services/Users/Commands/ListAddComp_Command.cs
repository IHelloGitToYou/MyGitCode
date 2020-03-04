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
    public class ListAddComp_Command : ExtjsViewCommand
    {
        public ListAddComp_Command()
        {
            this.Text = "添加";
            this.CommandPath = "ExtGAO.view.user.commands.ListAddComp_Command";
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
