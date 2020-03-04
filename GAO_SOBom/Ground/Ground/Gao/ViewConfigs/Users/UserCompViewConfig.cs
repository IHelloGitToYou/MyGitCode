using Gao.Models.Users;
using Gao.Services.Users.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.ViewConfigs
{
    public class UserCompViewConfig : GaoCore.EntityViewConfig<UserComp>
    {
        public override void OnListView()
        {
            SetTabTitle("");
            SetTitle("");//用户帐套管理

            UseCommands(typeof(ListAddComp_Command), typeof(ListDeleteComp_Command), typeof(ListSaveComp_Command), typeof(ListChangePassWorkComp_Command));
            Propery(o => o.CompId).SetComboboxEditor("SelectCompEditor")
                                    .SetComboboxEditorSetter("CompNo", "CompNo")
                                    .SetComboboxEditorSetter("Name", "CompName");

            Propery(o => o.CompNo);
            Propery(o => o.CompName);
            
        }

    }
}
