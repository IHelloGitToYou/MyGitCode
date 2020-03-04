
Ext.define('ExtGAO.view.part.commands.PartAddCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            newPartNo = '';
        
        RequestX.RequestGetSync('SelectBom/GetNewPartName', {},
            function (data2) {
                newPartNo = data2;
            }, this);

        var form = AutoUI.GenerateDetailView('GAOSelectBom.Models.Part', 'DETAIL');
        var newRecord = Ext.create('GAOSelectBom.Models.Part', { Id: -1, PartNo: newPartNo });
        //变量为了下拉框 联动
        form.record = newRecord;

        form.loadRecord(newRecord);

        Ext.create('Ext.window.Window', {
            title: '添加新模块',
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
                        AppTool.ShowOKMsg('添加成功');
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
