
Ext.define('ExtGAO.view.part.commands.MoveUpCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            store = view.getStore(),
            sels = view.getSelectionModel().getSelection();

        if (sels.length == 0) {
            Ext.Msg.alert('提示', '没有选择记录');
            return false;
        }

        var currentIndex = store.indexOf(sels[0]);
        if (currentIndex == 0) {
            AppTool.ShowErrorMsg('已经到顶', '提示');
            return;
        }

        me.Post(Ext.encode({
                IsMoveUp: true,
                CurrentEntityId: sels[0].get('Id')
            }),
            function (data) {
            //AppTool.ShowOKMsg('成功');
            //this.close();

            var searchBtn = AppTool.FindSearchCommand(view);
            searchBtn.Execute(searchBtn.View, null);
        }, this);
        
    }
});
