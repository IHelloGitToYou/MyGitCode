
Ext.define('ExtGAO.view.user.commands.ListChangePassWorkComp_Command', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            store = view.getStore(),
            sels = view.getSelectionModel().getSelection();

        if (sels.length == 0) {
            AppTool.ShowOKMsg('记录未选择!');
            return;
        }

        //三个控件 原 2个新 
        // 确认提交 
        var form = AutoUI.GenerateDetailView('Gao.Models.ChangePassWordViewModel', 'DETAIL');
        Ext.create('Ext.window.Window', {
            title: '修改密码',
            height: 250,
            //width: 400,
            layout: 'fit',
            items: form,
            onClick: function (BtnPosition) {
                if (BtnPosition == 1) {
                    this.close();
                    return;
                }

                console.log(sels);
                var data = form.getValues();
                data.UserId = sels[0].get('UserId');
                data.CompId = sels[0].get('CompId');

                me.Post(Ext.encode(data), function (data) {
                    AppTool.ShowOKMsg('密码成功修改');
                    this.close();

                    //var searchBtn = AppTool.FindSearchCommand(view);
                    //searchBtn.Execute(searchBtn.View, null);
                    ////ExtGAO.view.main.QueryViewSearchCommand
                    ////view.getStore().load();
                }, this);
            },
            buttons: [
                { text: '确认', handler: function () { this.up('window').onClick(0); } },
                { text: '取消', handler: function () { this.up('window').onClick(1); } }
            ]
        }).show();
    }
});