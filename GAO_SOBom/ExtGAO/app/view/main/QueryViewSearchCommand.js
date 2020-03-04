
Ext.define('ExtGAO.view.main.QueryViewSearchCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            grid = view.up('panel').down('grid'),
            store = grid.getStore(),
            formDataJson = Ext.encode(view.getValues()),
            lastParams = {
                QueryEntity: view.EntityType,// 'Gao.Models.UserQuery',
                data: formDataJson //"{\'id\' :\'123\',\'UserNo\':\'AAAAAA\', \'Name\':\'AAAABBBB\'}"
            };
         
        store.getProxy().setExtraParam('QueryEntity', lastParams.QueryEntity);
        store.getProxy().setExtraParam('data', lastParams.data);

        var pagetool = view.getDockedItems('toolbar[dock="bottom"]')[0];
        //console.log(pagetool);
        //store.getProxy().extraParames = lastParams;

        store.loadPage(1,{
            params: lastParams ,
            callback: function (records, operation, success) {
                grid.getSelectionModel().select(0, true);
            },
            scope: this
        });

        //me.Post("123457889");
    }
});
