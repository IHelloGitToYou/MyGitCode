using GaoCore.ViewConfigs;
using GAOSelectBom.Models;
using GAOSelectBom.Services.Parts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAOSelectBom.ViewConfigs
{
    public class PartViewConfig : GaoCore.EntityViewConfig<Part>
    {

        public override void OnDetailView()
        {
            Layout = new LayoutForm();
            Propery(o => o.PartNo).SetEditable(false);
            Propery(o => o.PartName);
            Propery(o => o.ReplacePrdId).SetComboboxEditor("selectprdteditor")
                                    .SetEditorConfig("valueField", "Id")
                                    .SetComboboxEditorSetter("Prd_No", "ReplacePrdNo")
                                    .SetLabel("对应替代编码");

           // Propery(o => o.ReplacePrdNo);

            Propery(o => o.Disabled).SetLabel("隐藏");
        }

        public override void OnListView()
        {
            SetController("part_controller");
            SetTabTitle("模块管理");
            SetTitle("");

            QueryView = new PartQueryViewConfig();
            QueryView.OnDetailView();
            QueryView.SetPageSize(20);

            Layout = new LayoutHBox();
            //上区域
            var mainGrid = new PartViewConfig();
            mainGrid.UseCommands(typeof(PartAddCommand), typeof(PartEditCommand), typeof(PartDeleteCommand), 
                                    typeof(ToolbarSeparatorCommand), typeof(ToolbarSpacerCommand), typeof(MoveUpCommand),typeof(MoveDownCommand));
            mainGrid.ContainerLayoutConfig = new LayoutConfig(Flex: 1);
            mainGrid.Propery(o => o.PartNo).SetEditable(false);
            mainGrid.Propery(o => o.PartName).SetEditable(false);
            mainGrid.Propery(o => o.ReplacePrdId)
                    .SetComboboxEditor("selectprdteditor")
                    .SetEditorConfig("valueField", "Id")
                    .SetLabel("对应替代编码").SetEditable(false);

            mainGrid.Propery(o => o.Disabled).SetLabel("隐藏").SetEditable(false);

            AddItem(mainGrid);
            //下区域
            var compView = new PartPrdtViewConfig();
            compView.ContainerLayoutConfig = new LayoutConfig(Flex: 2);
            compView.OnListView();
            AddItem(compView);
        }
    }
}
