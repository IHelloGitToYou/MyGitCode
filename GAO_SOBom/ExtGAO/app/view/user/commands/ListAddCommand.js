
Ext.define('ExtGAO.view.user.commands.ListAddCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this;

        var form = AutoUI.GenerateDetailView('Gao.Models.User', 'DETAIL');
        Ext.create('Ext.window.Window', {
            title: '添加用户',
            height: 250,
            //width: 400,
            layout: 'fit',
            items: form,
            onClick: function (BtnPosition) {
                if (BtnPosition == 1) {
                    this.close();
                    return;
                }

                //console.log(form.isValid);
                if (form.isValid()) {
                    me.Post(Ext.encode(form.getValues()), function (data) {
                        AppTool.ShowOKMsg('添加成功');
                        this.close();

                        var searchBtn = AppTool.FindSearchCommand(view);
                        searchBtn.Execute(searchBtn.View, null);
                        //ExtGAO.view.main.QueryViewSearchCommand
                        //view.getStore().load();
                    }, this);
                }
            },
            buttons: [
                { text: '创建', handler: function () { this.up('window').onClick(0); } },
                { text: '取消', handler: function () { this.up('window').onClick(1); } }
            ]
        }).show();

        //生成弹出Form Win 
        //  要不要使用Valid功能？
        //确认 创建 API成功后,
        // 重新Load 本视图Store 
        //   查询面板,
        //   Store 分页功能

        //this.Post('My add  Execute');
        //console.log('My add  Execute');
    }
});
