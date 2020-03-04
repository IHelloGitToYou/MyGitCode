Ext.define('ExtGAO.view.part.commands.PartPrdtDeleteCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            store = view.getStore(),
            sels = selRec = view.getSelectionModel().getSelection();

        if (sels.length == 0) {
            AppTool.ShowOKMsg('记录未选择!');
            return;
        }

        if (sels[0].get('Id') > 0) {
            Ext.Msg.confirm('确认', '是否确认删除记录', function (btn) {
                if (btn == "yes") {
                    store.remove(sels[0]);
                }
            });
        }
        else {
            store.remove(sels[0]);
        }
    }
});