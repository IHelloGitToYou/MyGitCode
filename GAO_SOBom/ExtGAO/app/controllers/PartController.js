Ext.define('ExtGAO.controllers.UserController', {
    extend: 'ExtGAO.controllers.BaseController',
    alias: 'controller.user_controller',

    onMainViewItemSelected: function (sender, records) {
        var bigPanel = this.getView(),
            //bigPanel = view.up('panel'),
            minView = bigPanel.views[0],
            subView = bigPanel.views[1],
            rec = records.length > 0 ? records[0] : null;
             
        
        this.MainViewCurrent = rec;
        subView.getStore().removeAll();
        if (!rec)
            return;

        //console.log(' onMainViewItemSelected 2');
        //加载 帐套视图数据
        subView.setLoading(true);
        RequestX.RequestGetSync('Sys/GetUserComps', { UserId: rec.get('Id') },
            function (data2) {
                //console.log(data2);
                subView.getStore().loadData(data2);
                subView.getStore().commitChanges();
                //var comp_nos = data2;
                ///var success = true;
            }, this);

        new Ext.util.DelayedTask(function () {
            subView.setLoading(false);
        }).delay(200);
    },
    ///保存成功后,重加载子视图
    RefreshCompViewData(subView, mainRecord) {
        //subView.getStore().removeAll();
        subView.setLoading(true);
        RequestX.RequestGetSync('Sys/GetUserComps', { UserId: mainRecord.get('Id') },
            function (data2) {
                subView.getStore().loadData(data2);
                subView.getStore().commitChanges();
            }, this);

        new Ext.util.DelayedTask(function () {
            subView.setLoading(false);
        }).delay(200);
    }


     
});
