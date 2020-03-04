
Ext.define('ExtGAO.view.user.commands.ListSaveComp_Command', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            bigPanel = view.up('panel'),
            controller = bigPanel.getController(),
            store = view.getStore(),
            changeRecords = AppTool.GetChangeRecords(store),
            data = [];
        
        for (var i = 0; i < changeRecords.length; i++) {
            data.push(changeRecords[i].data);
        }

        //console.log(data);
        RequestX.RequestPostSyncJsonData('Sys/SaveUserComps', data,
            function (data2) {
                //重载子视图数据
                controller.RefreshCompViewData(view, controller.MainViewCurrent);
                ////subView.getStore().loadRecords(data2);
                ////var comp_nos = data2;
                /////var success = true;
                ////subView.getStore().commit();
            }, this);


    }
});