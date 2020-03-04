
Ext.define('ExtGAO.view.so.commands.SORowDeleteCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            store = view.getStore(),
            //controller = view.up('panel').getController(),
            sels = view.getSelectionModel().getSelection();

        if (sels.length == 0) {
            AppTool.ShowOKMsg('订单行[未选择]!');
            return;
        }

        Ext.Msg.confirm('确认', '是否确认删除记录', function (btn) {
            if (btn == "yes") {
                store.remove(sels[0]);
            }
        });
    }
});
