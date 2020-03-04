
Ext.define('ExtGAO.view.user.commands.ListDeleteCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            sels = view.getSelectionModel().getSelection();

        if (sels.length == 0) {
            Ext.Msg.alert('提示', '没有选择记录');
            return false;
        }
        var selRec = sels[0];

        Ext.Msg.confirm('确认', '是否确认删除记录', function (btn) {
            if (btn == "yes") {
                console.log(arguments);

                me.Post(Ext.encode(selRec.getData()), function (data) {
                    view.getStore().remove(selRec);
                    //form.record.commit();
                    //win.close();
                    AppTool.ShowOKMsg('删除成功');
                });
            }
        });
    }
});
