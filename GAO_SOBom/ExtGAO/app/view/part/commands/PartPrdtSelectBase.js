
Ext.define('ExtGAO.view.part.commands.PartPrdtSelectBase', {
    extend: 'ExtGAO.helpers.BaseCommand',
    IsExcept :false,
    Execute: function (view, viewModel) {
        var me = this,
            bigPanel = view.up('panel'),
            controller = bigPanel.getController(),
            store = view.getStore(),
            sels = view.getSelectionModel().getSelection(),
            prdNos = [];

        if (sels.length == 0) {
            Ext.Msg.alert('提示', '没有选择记录');
            return false;
        }

        var rec = sels[0],
            f1 = me.IsExcept ? 'ExceptBomPrdtString' : 'ValidBomPrdtString',//本设置栏位
            f2 = me.IsExcept ? 'ValidBomPrdtString' : 'ExceptBomPrdtString',
            f3 = me.IsExcept ? '排除机型' : '可选机型';


        if (rec.get(f2)) {
            AppTool.ShowErrorMsg('可选与排除二选一!', '异常');
            return;
        }

        if (rec.get(f1)) {
            prdNos = rec.get(f1).split(',');
        }

        var prdts = [];
        RequestX.RequestGetSync('SelectBom/GetPrdts', { PrdNos: prdNos },
            function (data2) {

                for (var i = 0; i < data2.length; i++) {
                    var newID = controller.getNewId();
                    prdts.push({
                        Id: i + 100,// 这个Id不需要是真实Id,因为这个保存是先删除后插入,
                            //  并且弹出窗体的【添加一行】的Controller不一样, 
                            //  所以用getNewId会取到相同的Id值, 如(- 1), 导致覆盖记录
                        Prd_No: data2[i].Prd_No,
                        PrdName: data2[i].Name,
                        PrdSpc: data2[i].Spc,
                        PrdId: data2[i].Id,
                        PrdId_display: data2[i].Prd_No
                    });
                }
                
            }, this);

        var listPanel = AutoUI.GenerateListView('GAOSelectBom.Models.PartPrdtDetail', 'SelectPrdt'),
            list = listPanel.mainView;
        list.getStore().loadData(prdts);

        Ext.create('Ext.window.Window', {
            title: f3,
            height: 500,
            width: 400,
            layout: 'fit',
            items: listPanel,
            onClick: function (BtnPosition) {
                if (BtnPosition == 1) {
                    this.close();
                    return;
                }

                var cnt = list.getStore().getCount(),
                    prdtsBack = [];
                for (var i = 0; i < cnt; i++) {
                    prdtsBack.push(list.getStore().getAt(i).get('Prd_No'));
                }

                rec.set(f1, prdtsBack.toString());

                this.close();
            },
            buttons: [
                { text: '创建', handler: function () { this.up('window').onClick(0); } },
                { text: '取消', handler: function () { this.up('window').onClick(1); } }
            ]
        }).show();
    }
});