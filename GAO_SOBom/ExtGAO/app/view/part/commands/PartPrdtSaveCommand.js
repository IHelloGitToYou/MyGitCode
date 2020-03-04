
Ext.define('ExtGAO.view.part.commands.PartPrdtSaveCommand', {
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

        RequestX.RequestPostSyncJsonData('SelectBom/SavePartPrdt', data,
            function (data2) {
                console.log(data2);
                //重载子视图数据
                controller.RefreshSubViewData(view, controller.MainViewCurrent);
                
            }, this);
    }
});