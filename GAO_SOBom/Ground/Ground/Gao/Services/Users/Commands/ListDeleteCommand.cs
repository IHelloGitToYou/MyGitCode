using Gao.Models;
using GaoCore;
using GaoCore.ViewConfigs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.Services.Users.Commands
{
    public class ListDeleteCommand : ExtjsViewCommand
    {
        public ListDeleteCommand()
        {
            this.Text = "删除";
            this.CommandPath = "ExtGAO.view.user.commands.ListDeleteCommand";
        }

        public override object Execute(string PostData)
        {
            User user = JsonConvert.DeserializeObject<User>(PostData);
            user.PersistStatus = GaoCore.PersistStatus.DELETED;
            user = RF<User>.SaveFixDllDB(user, LoginUser);

            return user;
        }
    }
}
