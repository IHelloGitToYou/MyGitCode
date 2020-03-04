Ext.define('ExtGAO.controllers.BaseController', {
    extend: 'Ext.app.ViewController',
    alias: 'controller.base_controller',

    //当前主页选择行
    MainViewCurrent: null,
    //取最新Id 因为 Model Id不允许重复,所以要做一个不重复的Id生成器
    newId : 0,
    getNewId: function () {
        this.newId = this.newId - 1;
        return this.newId;
    },
    //alias: 'controller.base',
    onMainViewItemSelected: function (sender, records) {
        var view = this.getView(),
            rec = records.length > 0 ? records[0] : null;

        this.MainViewCurrent = rec;
        console.log(' onMainViewItemSelected BASE ');
    }
});