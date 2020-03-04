Ext.define('ExtGAO.controllers.PartController', {
    extend: 'ExtGAO.controllers.BaseController',
    alias: 'controller.part_controller',

    onMainViewItemSelected: function (sender, records) {
        var bigPanel = this.getView(),
            //bigPanel = view.up('panel'),
            subView = bigPanel.views[1],
            rec = records.length > 0 ? records[0] : null;

        this.MainViewCurrent = rec;
        subView.getStore().removeAll();
        if (!rec)
            return;

        //console.log(' onMainViewItemSelected 2');
        //加载 帐套视图数据
        subView.setLoading(true);
        RequestX.RequestGetSync('SelectBom/GetPartPrdts', { PartId: this.MainViewCurrent.get('Id') },
            function (data2) {
                subView.getStore().loadData(data2);
                subView.getStore().commitChanges();
            }, this);

        new Ext.util.DelayedTask(function () {
            subView.setLoading(false);
        }).delay(200);
    },
    ///保存成功后,重加载子视图
    RefreshSubViewData(subView, mainRecord) {

        subView.setLoading(true);
        RequestX.RequestGetSync('SelectBom/GetPartPrdts', { PartId: mainRecord.get('Id') },
            function (data2) {
                subView.getStore().loadData(data2);
                subView.getStore().commitChanges();
            }, this);

        new Ext.util.DelayedTask(function () {
            subView.setLoading(false);
        }).delay(200);
    }


     
});
