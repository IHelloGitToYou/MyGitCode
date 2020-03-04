
Ext.define('ExtGAO.view.so.commands.SOAddCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    CacheAllParts: undefined,  //缓存选配项
    LayoutUI: function (view, viewModel) {
        var me = this,
            controller = view.up('panel').getController();//.getNewId()
        if (me.CacheAllParts == undefined) {
            RequestX.RequestGetSync('SelectBom/GetAllPart', {},
                function (data2) {
                    me.CacheAllParts = data2;
                }, this);
        }

        var formPanel = AutoUI.GenerateDetailView('GAOSelectBom.Models.MF_Pos', 'DETAIL', function (panelConfig) {
            //panelConfig.title = "xtypextypextype";
            var gridConfig = panelConfig.items[1],
                columns = gridConfig.columns,
                hisIdNoIndex = -1;

            Ext.Array.forEach(columns, function (item, index, columns2) {
                if (item.dataIndex == 'Z_T_HIS_ID_NO')
                    hisIdNoIndex = index;
            }, me);

            if (hisIdNoIndex == -1) {
                alert('异常[未配置Z_T_HIS_ID_NO]列');
            }

            var partColumns = [];
            for (var i = 0; i < me.CacheAllParts.length; i++) {
                partColumns.push({
                    editor: {
                        xtype: 'selecttparteditor',
                        PartNo: me.CacheAllParts[i].PartNo,
                        RendererDisplayField: "PrdName",
                        iscombobox: true,
                        IsCombobox: true
                        //valueField: "Prd_No",
                        //SetterOtherField: { PrdNo: "prd_name" }
                    },
                    PartNo: me.CacheAllParts[i].PartNo,
                    text: me.CacheAllParts[i].PartName,
                    dataIndex: me.CacheAllParts[i].PartNo,
                    width: undefined,
                    renderer: function (c_value, c_metaData, c_record, c_rowIndex, c_colIndex, c_store, c_view) {

                        var headerCt = this.getHeaderContainer(),
                            column = headerCt.getHeaderAtIndex(c_colIndex);

                        return c_record.get(column.dataIndex + '_display');
                    }
                });
            }

            Ext.Array.insert(columns, hisIdNoIndex + 1, partColumns);
        });

        //formPanel.setController(controller);

        return formPanel;
    },
    Execute: function (view, viewModel) {
        var me = this,
            controller = view.up('panel').getController();

        var formPanel = me.LayoutUI(view, viewModel);
        var form = formPanel.mainView;

        formPanel.getController().SetChangeListeners(formPanel);
        //console.log('aaa');
        var newRecord = Ext.create('GAOSelectBom.Models.MF_Pos', {
            Id: controller.getNewId(),
            OS_DD: AppTool.GetDateString(),// new Date().getDate(),
            PO_DEP: '0000',
            PO_DEP_display: '0000'
        });
        ///变量为了下拉框 联动
        form.record = newRecord;

        form.loadRecord(newRecord);

        Ext.create('Ext.window.Window', {
            title: '[添加]新销售订单',
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
                    var soData = {},
                        gridStore = formPanel.down('grid').getStore();

                    soData.Header = form.getValues();
                    soData.DetailList = [];

                    for (var i = 0; i < gridStore.getCount(); i++) {
                        var tPartJson = {},
                            tPartJsonIsEmpty = true,
                            rec = gridStore.getAt(i);

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

                    soData.SOTableFormat = Gao.SOTableFormat;
                    me.Post(Ext.encode(soData), function (data) {
                        AppTool.ShowOKMsg('添加成功');


                        this.close();
                        var searchBtn = AppTool.FindSearchCommand(view);
                        searchBtn.Execute(searchBtn.View, null);
                        view.getStore().load();
                    }, this);
                }
            },
            buttons: [
                { text: '创建', handler: function () { this.up('window').onClick(0); } },
                { text: '取消', handler: function () { this.up('window').onClick(1); } }
            ]
        }).show();

        this.SetNewOSNO(form);
    },
    //加载新的单号
    SetNewOSNO: function (form) {
            RequestX.RequestGet('SO/GetNewOSNo', {
                Day: new Date(),
                SOFormat: Gao.SOTableFormat
            },
            function (data2) {
                var boxOSNo = Ext.ComponentQuery.query('field[name=OS_NO]', form)[0];
                boxOSNo.setValue(data2);
                newPartNo = data2;
            }, form);
    }
});
