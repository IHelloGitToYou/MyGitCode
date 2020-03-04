using Gao.Services.Users.Commands;
using Gao.ViewConfigs;
using GaoCore.ViewConfigs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao.Models
{
    public class UserViewConfig: GaoCore.EntityViewConfig<User>
    {
        public override void OnDetailView()
        {
            //SetTitle("用户管理");
            //UseCommands(typeof(ListAddCommand));

            //Layout = new LayoutColumn();

            //var form1 = new FormContainer();
            //form1.SetTitle("");
            //form1.ContainerLayoutConfig = new LayoutConfig(ColumnWidth : 0.8m);
            //form1.UseLayout(new LayoutColumn());
            //form1.Add(Propery(o => o.UserNo).SetLayoutConfig(new LayoutConfig(ColumnWidth:0.5m)));
            //form1.Add(Propery(o => o.Name).SetLayoutConfig(new LayoutConfig(ColumnWidth: 0.5m)));

            //AddItem(form1);
            
            //var form2 = new FormContainer();
            //form2.SetTitle("");
            //form2.ContainerLayoutConfig = new LayoutConfig(ColumnWidth: 0.2m);
            //form2.UseLayout(new LayoutTable(2));
            //form2.Add(Propery(o => o.PassWork));
            //AddItem(form2);
            //return;

            Layout = new LayoutForm();
            Propery(o => o.UserNo);
            Propery(o => o.Name);
           //Propery(o => o.PassWork).SetLabel("初始化密码");
        }

        public override void OnListView()
        {
            SetController("user_controller");

            SetTabTitle("用户管理");
            SetTitle("");

            Layout = new LayoutVBox();

            //var gridLeft = new ListContainer();
            //gridLeft.UseCommands(typeof(ListAddCommand), typeof(ListEditCommand), typeof(ListDeleteCommand));
            //gridLeft.ContainerLayoutConfig = new LayoutConfig(Flex: 2);
            //gridLeft.Add(Propery(o => o.UserNo));
            //gridLeft.Add(Propery(o => o.Name));
            //AddItem(gridLeft);

            //var gridRight = new ListContainer();
            //gridRight.ContainerLayoutConfig = new LayoutConfig(Flex: 2);
            //gridRight.Add(Propery(o => o.PassWork));
            //gridRight.Add(Propery(o => o.Name));
            //AddItem(gridRight);

            QueryView = new UserQueryViewConfig();
            QueryView.OnDetailView();
            QueryView.SetPageSize(20);

            //左边区域
            var mainGrid = new UserViewConfig();
            mainGrid.UseCommands(typeof(ListAddCommand), typeof(ListEditCommand), typeof(ListDeleteCommand));
            mainGrid.ContainerLayoutConfig = new LayoutConfig(Flex: 2);
            mainGrid.Propery(o => o.UserNo).SetEditable(false);
            mainGrid.Propery(o => o.Name).SetEditable(false);

            AddItem(mainGrid);
            //右边区域
            var compView = new UserCompViewConfig();
            compView.ContainerLayoutConfig = new LayoutConfig(Flex: 2);
            compView.OnListView();
            AddItem(compView);
        }

        public override void OnOtherView(string P_ViewGroup)
        {
            if(P_ViewGroup == "EDIT_INFO")
            {
                //SetTitle("编辑用户资料");
                Propery(o => o.UserNo).SetEditable(false);
                Propery(o => o.Name);
            }
        }
    }
}
