
Ext.define('ExtGAO.view.part.commands.PartEditCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            sels = view.getSelectionModel().getSelection();


        if (sels.length == 0) {
            Ext.Msg.alert('提示', '没有选择记录');
            return false;
        }

        var form = AutoUI.GenerateDetailView('GAOSelectBom.Models.Part', 'DETAIL');

        ////var ttttest = Ext.create('GAOSelectBom.Models.Part', {
        ////    Id:2,
        ////    PartNo: "T1",
        ////    PartName: "颜色",
        ////    ReplacePrdId: "4",
        ////    ReplacePrdNo: "A0002",
        ////    Disabled: "False",
        ////    Sort: "1",
        ////    CreateId: "1",
        ////    CreateDate: "2020/2/4 16:15:28",
        ////    UpdateId: "1",
        ////    UpdateDate: "2020/2/4 16:15:28",
        ////    ReplacePrdId_display: "A0002",
        ////    RowIndex: "1"
        ////});
        //////变量为了下拉框 联动

        form.record = sels[0];
        
        form.loadRecord(form.record); //form.record


        Ext.create('Ext.window.Window', {
            title: '编辑模块',
            height: 350,
            //width: 400,
            layout: 'fit',
            items: form,
            onClick: function (BtnPosition) {
                if (BtnPosition == 1) {
                    this.close();
                    return;
                }
                if (form.isValid()) {
                    form.updateRecord(form.record);

                    me.Post(Ext.encode(form.record.getData()), function (data) {
                        AppTool.ShowOKMsg('修改成功');
                        this.close();

                        var searchBtn = AppTool.FindSearchCommand(view);
                        searchBtn.Execute(searchBtn.View, null);
                        //ExtGAO.view.main.QueryViewSearchCommand
                        //view.getStore().load();
                    }, this);
                }
            },
            buttons: [
                { text: '创建', handler: function () { this.up('window').onClick(0); } },
                { text: '取消', handler: function () { this.up('window').onClick(1); } }
            ]
        }).show();
 
    }
});
