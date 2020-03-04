using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Gao.Models;
using GaoCore;
using SqlSugar;
using GaoCore.ViewConfigs;
using Gao.Models.Users;

namespace Gao.Services.Users.Commands
{
    public class ListChangePassWorkComp_Command : ExtjsViewCommand
    {
        public ListChangePassWorkComp_Command()
        {
            this.Text = "修改密码";
            this.CommandPath = "ExtGAO.view.user.commands.ListChangePassWorkComp_Command";
        }

        public override object Execute(string PostData)
        {
            var vmodel = JsonConvert.DeserializeObject<ChangePassWordViewModel>(PostData);
            if (vmodel.NewPassWork != vmodel.NewPassWork2)
                throw new InValidException("二个新密码不一致");

            var db = Entity.GetDb(typeof(UserComp));
            var uesrComp = db.Queryable<UserComp>().Where(o => o.UserId == vmodel.UserId && o.CompId == vmodel.CompId).First();
            if (uesrComp == null)
                throw new InvalidCastException("原记录意外被删除!");

            if (uesrComp.PassWork != UserService.ToPassWordMD5(vmodel.PassWork))
                throw new InvalidCastException("原密码不相同,请联系管理员");

            uesrComp.PassWork = UserService.ToPassWordMD5(vmodel.NewPassWork2);
            uesrComp.PersistStatus = PersistStatus.MODIFY;
            //db.Updateable<UserComp>(uesrComp) 
            //            .Where(o => o.Id == uesrComp.Id)
            //            .ExecuteCommand();
            RF<UserComp>.SaveFixDllDB(uesrComp, LoginUser);
            return true;
        }
    }
}
