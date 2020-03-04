using Gao.Models;
using GaoCore.ViewConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.ViewConfigs
{
    public class UserQueryViewConfig : GaoCore.EntityViewConfig<UserQuery>
    {
        public UserQueryViewConfig()
        {
            UseCommands(typeof(QueryViewSearchCommand));
        }

        public override void OnDetailView()
        {
            Layout = new LayoutForm();
            Propery(o => o.UserNo);
            Propery(o => o.Name);
        }
    }
}
