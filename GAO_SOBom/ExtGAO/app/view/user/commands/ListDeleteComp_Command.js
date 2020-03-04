
Ext.define('ExtGAO.view.user.commands.ListDeleteComp_Command', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            store = view.getStore(),
            sels = selRec = view.getSelectionModel().getSelection();

        if (sels.length == 0) {
            AppTool.ShowOKMsg('记录未选择!');
            return;
        }
        store.remove(sels[0]);
    }
});