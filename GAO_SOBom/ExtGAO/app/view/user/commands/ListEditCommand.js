
Ext.define('ExtGAO.view.user.commands.ListEditCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this;

        var form = AutoUI.GenerateDetailView('Gao.Models.User', 'EDIT_INFO');
        var sels = view.getSelectionModel().getSelection();
        if (sels.length ==0) {
            Ext.Msg.alert('提示', '没有选择记录');
            return false;
        }

        form.record = view.getSelectionModel().getSelection()[0];
        form.record.commit();
        form.loadRecord(form.record);

        Ext.create('Ext.window.Window', {
            title: '编辑用户',
            height: 200,
            //width: 400,
            layout: 'fit',
            items: form,
            onClick: function (BtnPosition, win) {
                if (BtnPosition == 1) {
                    this.close();
                    return;
                }

                console.log(form);
                if (form.isValid()) {
                    form.updateRecord(form.record);
                    if (form.record.crudState == 'R') {
                        Ext.Msg.alert('提示', '资料无修改过');
                        return;
                    }

                    me.Post(Ext.encode(form.record.getData()), function (data) {
                        form.record.commit();
                        win.close();
                        AppTool.ShowOKMsg('修改成功');
                    });

                    return;
                }
            },
            buttons: [
                { text: '创建', handler: function () { this.up('window').onClick(0, this.up('window')); } },
                { text: '取消', handler: function () { this.up('window').onClick(1, this.up('window')); } }
            ]
        }).show();

    }
});
