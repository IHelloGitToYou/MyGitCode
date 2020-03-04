
Ext.define('ExtGAO.view.so.commands.SOEditCommand', {
    extend: 'ExtGAO.view.so.commands.SOAddCommand',
    CacheAllParts: undefined,  //缓存选配项
    Execute: function (view, viewModel) {
        var me = this,
            sels = view.getSelectionModel().getSelection(),
            controller = view.up('panel').getController();      //.getNewId()
        
        if (sels.length == 0) {
            AppTool.ShowOKMsg('订单未选择!');
            return;
        }

        var formPanel = me.LayoutUI(view, viewModel);
        var form = formPanel.mainView,
            grid = formPanel.views[1],
            hasParts = me.CacheAllParts.length > 0 ? true : false,
            sel = sels[0];

        me.MFRecord = sel;

        me.LoadTableSO(formPanel, sel);
        formPanel.getController().SetChangeListeners(formPanel);

        Ext.create('Ext.window.Window', {
            title: '[修改]销售订单',
            height: 650,
            width: 1050,
            layout: 'fit',
            modal :true,
            items: formPanel,
            onClick: function (BtnPosition) {
                if (BtnPosition == 1) {
                    this.close();
                    return;
                }
                if (form.isValid()) {
                    me.OnSave(formPanel );
                }
            },
            buttons: [
                { text: '保存', handler: function () { this.up('window').onClick(0); } },
                { text: '取消', handler: function () { this.up('window').onClick(1); } }
            ]
        }).show();
    },

    MFRecord: null, 

    LoadTableSO: function (formPanel, mf_record) {
        var me = this,
            form = formPanel.mainView,
            grid = formPanel.views[1],
            hasParts = me.CacheAllParts.length > 0 ? true : false;

        RequestX.RequestGetSync('SO/GetTableSO', {
                OS_ID: mf_record.get('OS_ID'),//'SO',
                OS_NO: mf_record.get('OS_NO') //'SO1816'
            },
            function (data2) {
                var record = Ext.create('GAOSelectBom.Models.MF_Pos', data2.Header);
                ///变量为了下拉框 联动
                form.record = record;

                form.loadRecord(record);


                //填充Z字段到TF_POS上
                for (var i = 0; i < data2.DetailList.length; i++) {
                    var dtl = Ext.create('GAOSelectBom.Models.TF_Pos', data2.DetailList[i]);
                    var dtlZData = Ext.Array.findBy(data2.DetailZList, function (v_item, v_index) {
                        if (v_item['ITM'] == dtl.get('ITM'))
                            return true;
                    });

                    var dtlZ = Ext.create('GAOSelectBom.Models.TF_Pos_Z', dtlZData);
                    console.log(dtlZ);
                    if (!dtlZ) {
                        alert("Z行记录找不到!");
                    }
                    else {
                        dtl.set('Z_T_PRD_NO', dtlZ.get('Z_T_PRD_NO'));
                        dtl.set('Z_T_PRD_NAME', dtlZ.get('Z_T_PRD_NAME'));
                        dtl.set('Z_T_HIS_ID_NO', dtlZ.get('Z_T_HIS_ID_NO'));
                        dtl.set('Z_T_JSON', dtlZ.get('Z_T_JSON'));

                        if (hasParts && dtlZ.get('Z_T_JSON')) {
                            var partJson = Ext.decode(dtlZ.get('Z_T_JSON'));
                            for (var j = 0; j < me.CacheAllParts.length; j++) {
                                var part = me.CacheAllParts[j];
                                dtl.set(part.PartNo, partJson[part.PartNo]);
                                dtl.set(part.PartNo + '_display', partJson[part.PartNo + '_display']);
                            }
                        }
                    }
                }
                grid.getStore().loadData(data2.DetailList);

                ///计算汇总金额(是虚拟字段)
                formPanel.getController().SetTotalAmtToHead(grid.getStore(), form);
                form.updateRecord(form.record);
                form.record.commit();

                grid.getStore().commitChanges();
            }, this);
    },

    OnSave:function(p_formPanel){
        var me = this,
            form = p_formPanel.mainView,
            grid = p_formPanel.views[1],
            store = grid.getStore(),
            changeRecords = AppTool.GetChangeRecords(store),
            soData = {};

        if (store.getCount() == 0) {
            AppTool.ShowErrorMsg('[订单行]不存在！');
            return;
        }

        form.updateRecord(form.record);
        if(form.record.crudState != 'U' && changeRecords.length == 0){
            AppTool.ShowErrorMsg('单据无修改！');
            return;
        }

        soData.Header = form.record.data;
        soData.DetailList = [];

        for (var i = 0; i < changeRecords.length; i++) {
            var tPartJson = {},
                tPartJsonIsEmpty = true,
                rec = changeRecords[i];

            for (var j = 0; j < me.CacheAllParts.length; j++) {
                var partNo = me.CacheAllParts[j].PartNo;
                if (rec.get(partNo)) {
                    tPartJsonIsEmpty = false;
                    tPartJson[partNo] = rec.get(partNo);
                    tPartJson[partNo + '_display']
                                = rec.get(partNo + '_display');
                }
            }
            if (tPartJsonIsEmpty)
                rec.set('Z_T_JSON', "");
            else 
                rec.set('Z_T_JSON', Ext.encode(tPartJson));

            soData.DetailList.push(rec.data);
        }
        
        //console.log(Ext.encode(soData));
        //form.record.set('OS_DD', "2020-02-19");

        me.Post(Ext.encode(soData), function (data) {
            AppTool.ShowOKMsg('修改成功');

            me.LoadTableSO(p_formPanel, me.MFRecord);
            ///this.close();
            ///var searchBtn = AppTool.FindSearchCommand(view);
            ///searchBtn.Execute(searchBtn.View, null);
            ///view.getStore().load();
        }, this);
        
    }
});