using Gao.Models;
using GaoCore;
using GaoCore.ViewConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.Services.Users.Commands
{
    public class ListEditCommand : ExtjsViewCommand
    {
        public ListEditCommand()
        {
            this.Text = "编辑";
            this.CommandPath = "ExtGAO.view.user.commands.ListEditCommand";
        }

        public override object Execute(string PostData)
        {
            User user = JsonConvert.DeserializeObject<User>(PostData);
            user.PersistStatus = GaoCore.PersistStatus.MODIFY;
            user = RF<User>.SaveFixDllDB(user, LoginUser);

            return user;
        }
    }
}
