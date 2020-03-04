using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using GAOSelectBom.Services.Parts;
using GAOSelectBom.Services.SO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs
{
    public class MF_PosViewConfig : GaoCore.EntityViewConfig<MF_Pos>
    {
        public override void OnDetailView()
        {
            SetController("so_controller");

            Layout = new LayoutVBox();
            ////上区域 Form 
            var mainForm = new MF_PosViewConfig();
            mainForm.Layout = new LayoutTable(4);
            mainForm.SetContainer(isFrom: true);
            mainForm.SetDetailDefaultConfig("labelWidth", 80);
            mainForm.SetDetailDefaultConfig("width", 220);
            mainForm.SetDetailDefaultConfig("margin", "0 2 2 2");

            //mainForm.ContainerLayoutConfig = new LayoutConfig(Flex: 1);
            mainForm.Propery(o => o.OS_DD);
            mainForm.Propery(o => o.OS_NO);
            mainForm.Propery(o => o.PO_DEP)
                    .SetComboboxEditor("selectdepteditor");
            mainForm.Propery(o => o.CUS_NO)
                    .SetComboboxEditor("selectcusteditor");


            mainForm.Propery(o => o.SAL_NO)
                 .SetComboboxEditor("selectsalmeditor");
            mainForm.Propery(o => o.EST_DD);
            mainForm.Propery(o => o.ZHANG_ID);
            mainForm.Propery(o => o.TAX_ID);


            mainForm.Propery(o => o.CUS_OS_NO).SetLayoutConfig(new LayoutConfig(Colspan: 4));


            mainForm.Propery(o => o.REM).SetLayoutConfig(new LayoutConfig(Colspan: 3)).SetEditorConfig("width", 600 );
            mainForm.Propery(o => o.SHOW_AMTN).SetLayoutConfig(new LayoutConfig(Colspan: 1));


            mainForm.Propery(o => o.ADR).SetLayoutConfig(new LayoutConfig(Colspan: 3))
                .SetEditorConfig("width", 600)
                .SetLabel("送货方式");
            mainForm.Propery(o => o.SHOW_TAX).SetLayoutConfig(new LayoutConfig(Colspan: 1));

            mainForm.Propery(o => o.PAY_REM).SetLayoutConfig(new LayoutConfig(Colspan: 3))
                .SetEditorConfig("width", 600)
                .SetLabel("交易方式");

            mainForm.Propery(o => o.SHOW_AMT_TATOL).SetLayoutConfig(new LayoutConfig(Colspan: 1));

            AddItem(mainForm);

            ////下区域 Grid 
            var tfView = new TF_PosViewConfig();
            //tfView.Layout = new LayoutColumn();
            tfView.ContainerLayoutConfig = new LayoutConfig(Flex: 1);
            tfView.OnListView();
            AddItem(tfView);
        }

        public override void OnListView()
        {
            SetController("base_controller");

            SetTabTitle("销售管理");
            SetTitle("");
            UseCommands(typeof(SOAddCommand), typeof(SOEditCommand), typeof(ToolbarSeparatorCommand));

            QueryView = new MF_PosQueryViewConfig();
            QueryView.OnDetailView();
            QueryView.SetPageSize(20);

            Propery(o => o.OS_DD);
            Propery(o => o.OS_NO);
            Propery(o => o.PO_DEP)
                    .SetComboboxEditor("selectdepteditor");
            Propery(o => o.CUS_NO)
                    .SetComboboxEditor("selectcusteditor");


            Propery(o => o.SAL_NO)
                 .SetComboboxEditor("selectsalmeditor");
            Propery(o => o.EST_DD);
            Propery(o => o.ZHANG_ID);
            Propery(o => o.TAX_ID);


            Propery(o => o.CUS_OS_NO);

            Propery(o => o.REM);
            Propery(o => o.SHOW_AMTN);


            Propery(o => o.ADR);
            Propery(o => o.SHOW_TAX);

            Propery(o => o.PAY_REM);
            Propery(o => o.SHOW_AMT_TATOL);

            //订单日期:
            //订单号码:
            //部门:
            //客户
            //业务员
            //预交日期
            //立帐？
            //扣税类别
            //客户订单
            //备注
            //未税金额
            //送货地址
            //税金
            //交易方式
            //合计

           

        }
    }
}
