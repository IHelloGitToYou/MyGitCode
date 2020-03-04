/**
 * This class is the controller for the main view for the application. It is specified as
 * the "controller" of the Main view class.
 */
Ext.define('ExtGAO.view.main.MainController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.main',

    onMenuClick: function (btn) {
        var view = this.getView();
        //btn.PageInfo: {
        //    EntityType: 'Gao.Models.User',
        //        ViewGroup: 'LIST',
        //            IsList: true
        //},
        
 
        //已打开了？
        for(var i = 0;i < view.items.length; ++i){
            if(view.items.getAt(i).PageInfo == btn.PageInfo){
                view.setActiveTab(i);
                return ;
            }
        }



        if (btn.PageInfo.IsList == true) {
            var listGrid = AutoUI.GenerateListView(btn.PageInfo.EntityType, btn.PageInfo.ViewGroup);
            listGrid.PageInfo = btn.PageInfo;
            
            listGrid.closeable = true;
            listGrid.closable = true;
            view.add(listGrid);

            var task = new Ext.util.DelayedTask(function(){
                view.setActiveTab(i);
            });
            task.delay(120);
        }
        else {
            alert('菜单未实现 Form');
        }
    },
    onItemSelected: function (sender, record) {

       
        //RequestX.Request('Sys/GetView', { EntityType: 'XX', ViewGroup: '' },
        //    function (data) {
        //        console.log(data);
        //    }, this);

       // Ext.Msg.confirm('Confirm', '你确认吗？ you sure?', 'onConfirm', this);
    },

    onConfirm: function (choice) {
        if (choice === 'yes') {
            //
        }
    }
});
