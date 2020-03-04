Ext.define('AutoUI', {
    singleton: true,
    requires: ['RequestX'],
    //生成视图器
    GenerateDetailView : function (EntityType, ViewGroup, fnChangeConfig) {
        //{
        //    "Items": null,
        //    "ShowFields": [{ "DataType": "string", "Label": "登录编码", "Name": "UserNo", "TotalWidth": null, "LabelWidth": null, "Editable": true, "Editor": "textfield", "EditorConfig": null }, { "DataType": "string", "Label": "名称", "Name": "Name", "TotalWidth": null, "LabelWidth": null, "Editable": true, "Editor": "textfield", "EditorConfig": null }, { "DataType": "string", "Label": "MD5密码", "Name": "PassWork", "TotalWidth": null, "LabelWidth": null, "Editable": true, "Editor": "textfield", "EditorConfig": null }],
        //     "Layout": null,
        //     "EntityType": "Gao.Models.User",
        //      "ViewGroup": "DETAIL"
        //}
        ViewGroup = ViewGroup || 'DETAIL';
        var layouts;
        RequestX.RequestSync('Sys/GetView', { EntityType: EntityType, ViewGroup: ViewGroup },
            function (data) {
                layouts = data;

            }, this);


        //如果Items有值,代表是复合的UI
        if (layouts.Items) {
            //保证第一个视力一定是Detail
            if (layouts.Items[0].Container != "DETAIL") {
                Ext.toast({
                    html: '第一个视图不是[明细]布局',
                    title: '配置异常',
                    width: 300,
                    align: 'br'
                });
                return null;
            }

            var bigPanelConfig = {
                title: layouts.Title,
                xtype: 'panel',
                items: [],
                controller: layouts.Controller || ''
                //width: 300,
                //height: 400
            };

            bigPanelConfig.layout = layouts.Layout || 'vbox';

            for (let i = 0; len = layouts.Items.length, i < len; i++) {
                let subView = layouts.Items[i];

                if (subView.Container == 'LIST') {
                    var subListConfig = AutoUI.GenerateListViewConfig(subView);

                    if (subView.ContainerLayoutConfig) {
                        subListConfig = Ext.Object.merge(subListConfig, subView.ContainerLayoutConfig);
                    }
                    bigPanelConfig.items.push(subListConfig);
                }
                else if (subView.Container == 'DETAIL') {
                    var subFormConfig = AutoUI.GenerateDetailConfig(subView);

                    if (subView.ContainerLayoutConfig) {
                        subFormConfig = Ext.Object.merge(subFormConfig, subView.ContainerLayoutConfig);
                    }
                    bigPanelConfig.items.push(subFormConfig);
                }
            }

            //查询面板
            if (layouts.QueryView) {
                var queryViewConfig = AutoUI.GenerateDetailConfig(layouts.QueryView);
                //console.log(queryViewConfig);
                bigPanelConfig.lbar = queryViewConfig;

                //增加分页 第一个视图是主视图
                bigPanelConfig.items[0].bbar = {
                    xtype: 'pagingtoolbar',
                    displayInfo: true,
                    pageSize: 25
                };
                if (layouts.QueryView.PageSize) {
                    bigPanelConfig.items[0].bbar.pageSize = layouts.QueryView.PageSize;
                    bigPanelConfig.items[0].store.pageSize = layouts.QueryView.PageSize;
                }
            }


            console.log(bigPanelConfig);

            if (fnChangeConfig) {
                Ext.callback(fnChangeConfig, this, [bigPanelConfig]);
            }
            var formPanel = Ext.create(bigPanelConfig);


            ///所有区域的按钮 增加当前主控件
            for (let i = 0; len = layouts.Items.length, i < len; i++) {

                if (layouts.Items[i].Commands) {
                    var subPanel = formPanel.getComponent(i);
                    var btns = subPanel.getDockedItems('toolbar[dock="top"]')[0].items;//.getDockedItems('toolbar[dock="top"]')[0].items;

                    for (let j = 0; len = btns.getCount(), j < len; j++) {
                        btns.getAt(j).View = subPanel;
                        btns.getAt(j).getView = function () { return this.View; }
                    }
                }
            }

            ///查询面板按钮 增加当前主控件
            // 查询变量 bigPanel2.searchForm
            if (layouts.QueryView) {
                if (layouts.QueryView.Commands) {
                    let searchForm = formPanel.getDockedItems('form')[0];
                    let searchBtns = searchForm.getDockedItems('toolbar[dock="top"]')[0].items;

                    for (let i = 0; len = searchBtns.getCount(), i < len; i++) {
                        searchBtns.getAt(i).View = searchForm;
                        searchBtns.getAt(i).getView = function () { return this.View; }
                    }

                    formPanel.searchForm = searchForm;
                    formPanel.searchForm.EntityType = layouts.QueryView.EntityType;
                }
            }
            ///设置View 视图列表 mainView
            formPanel.views = [];
            for (let i = 0; len = layouts.Items.length, i < len; i++) {
                if (i == 0) {
                    formPanel.mainView = formPanel.getComponent(i);
                }
                formPanel.views.push(formPanel.getComponent(i));
            }

            return formPanel;
        }
        else {
            var formConfig2 = AutoUI.GenerateDetailConfig(layouts);

            formConfig2.controller = layouts.Controller || '';
            //if (layouts.TabTitle)
            //    formConfig2.tab = { title: layouts.TabTitle };
            console.log(formConfig2);
            var formPanel2 = Ext.create(formConfig2);
            formPanel2.views = [];
            formPanel2.mainView = formPanel2.getComponent(0);
            return formPanel2;
        }
    },


    GenerateDetailConfig : function (layouts) {
        var formConfig = {
            title: layouts.Title,
            //tab: { title: 'XXX2' },
            defaults: {
                labelAlign: 'right'
            },
            xtype: 'form',
            items: [],
            tbar: [],
            //width: 300,
            //height: 400,
            plugins: [
                { ptype: 'gaoformediting' }
            ]
            //listeners: {
            //    boxready: function (vthis, width, height, eOpts) {

            //    }
            //}
        };

        ///初始化控件默认值
        var defaultValues = Ext.create(layouts.EntityType, {});

        if (layouts.DetailDefaultConfig)
            formConfig.defaults = Ext.Object.merge(formConfig.defaults, layouts.DetailDefaultConfig);

        formConfig.layout = layouts.Layout || 'form';

        for (var i = 0; len = layouts.ShowFields.length, i < len; i++) {
            let field = layouts.ShowFields[i];
            let fieldConfig = {
                fieldLabel: field.Label,
                name: field.Name,
                xtype: field.Editor
                //allowBlank : false
            }

            if (defaultValues.get(field.Name)) {
                fieldConfig.value = defaultValues.get(field.Name);
            }

            if (field.LabelWidth)
                fieldConfig.labelWidth = field.LabelWidth;
            if (field.TotalWidth)
                fieldConfig.width = field.TotalWidth;
            if (field.Hidden == true)
                fieldConfig.hidden = true;


            if (field.LayoutConfig) {
                fieldConfig = Ext.Object.merge(fieldConfig, field.LayoutConfig);
            }

            if (field.EditorConfig) {
                //console.log(field.EditorConfig);
                fieldConfig = Ext.Object.merge(fieldConfig, field.EditorConfig);

                //枚举的数据
                if (field.EditorConfig.EditorEunmData) {
                    fieldConfig.xtype = "SelectEunmEditor";
                    fieldConfig.EditorEunmData = field.EditorConfig.EditorEunmData;
                    fieldConfig.store = {
                        model: 'Gao.Models.EmumData',
                        data: Gao.Datas[field.EditorConfig.EditorEunmData]
                    };
                    //colConfig.editor.store.data = Gao.Datas[field.EditorConfig.EditorEunmData];
                }
            }


            if (field.IsCombobox) {
                fieldConfig.IsCombobox = true;
            }

            formConfig.items.push(fieldConfig);
        }

        ///显示按钮
        if (layouts.Commands) {
            for (let i = 0; len = layouts.Commands.length, i < len; i++) {
                var btnConfig = {
                    xclass: layouts.Commands[i].CommandPath,
                    text: layouts.Commands[i].Text,
                    iconCls: layouts.Commands[i].IconCls
                };

                formConfig.tbar.push(btnConfig);
            }
        }

        //var form = Ext.create(formConfig);
        return formConfig;
    },


    GenerateListView : function (EntityType, ViewGroup, fnChangeConfig) {
        ViewGroup = ViewGroup || 'LIST';
        var layouts;
        RequestX.RequestSync('Sys/GetView', { EntityType: EntityType, ViewGroup: ViewGroup },
            function (data) {
                layouts = data;
            }, this);

        //如果Items有值,代表是复合的UI
        if (layouts.Items) {
            ////保证第一个视力一定是list
            if (layouts.Items[0].Container != "LIST") {
                Ext.toast({
                    html: '第一个视图不是[列表]布局',
                    title: '配置异常',
                    width: 300,
                    align: 'br'
                });
                return null;
            }

            var bigPanelConfig = {
                title: layouts.TabTitle,
                xtype: 'panel',
                items: [],
                tbar: [],
                controller: layouts.Controller || ''
            };

            ///////显示按钮
            ////if (layouts.Commands) {
            ////    for (let i = 0; len = layouts.Commands.length, i < len; i++) {
            ////        var btnConfig = {
            ////            xclass: layouts.Commands[i].CommandPath,
            ////            text: layouts.Commands[i].Text,
            ////            iconCls: layouts.Commands[i].IconCls
            ////        };

            ////        bigPanelConfig.tbar.push(btnConfig);
            ////    }
            ////}

            bigPanelConfig.layout = layouts.Layout || 'hbox';

            for (let i = 0; len = layouts.Items.length, i < len; i++) {
                let subView = layouts.Items[i];

                if (subView.Container == 'LIST') {
                    var subListConfig = AutoUI.GenerateListViewConfig(subView);

                    if (subView.ContainerLayoutConfig) {
                        subListConfig = Ext.Object.merge(subListConfig, subView.ContainerLayoutConfig);
                    }
                    bigPanelConfig.items.push(subListConfig);
                }
                else if (subView.Container == 'DETAIL') {
                    var subFormConfig = AutoUI.GenerateDetailConfig(subView);

                    if (subView.ContainerLayoutConfig) {
                        subFormConfig = Ext.Object.merge(subFormConfig, subView.ContainerLayoutConfig);
                    }
                    bigPanelConfig.items.push(subFormConfig);
                }
            }

            //查询面板
            if (layouts.QueryView) {
                var queryViewConfig = AutoUI.GenerateDetailConfig(layouts.QueryView);
                //console.log(queryViewConfig);
                bigPanelConfig.lbar = queryViewConfig;

                //增加分页 第一个视图是主视图
                bigPanelConfig.items[0].bbar = {
                    xtype: 'pagingtoolbar',
                    displayInfo: true,
                    pageSize: 25
                };
                if (layouts.QueryView.PageSize) {
                    bigPanelConfig.items[0].bbar.pageSize = layouts.QueryView.PageSize;
                    bigPanelConfig.items[0].store.pageSize = layouts.QueryView.PageSize;
                }
            }

            console.log(bigPanelConfig);
            if (fnChangeConfig) {
                Ext.callback(fnChangeConfig, this, [bigPanelConfig]);
            }
            var bigPanel = Ext.create(bigPanelConfig);

            ///所有区域的按钮 增加当前主控件
            for (let i = 0; len = layouts.Items.length, i < len; i++) {

                if (layouts.Items[i].Commands) {
                    var subPanel = bigPanel.getComponent(i);
                    var btns = subPanel.getDockedItems('toolbar[dock="top"]')[0].items;//.getDockedItems('toolbar[dock="top"]')[0].items;

                    for (let j = 0; len = btns.getCount(), j < len; j++) {
                        btns.getAt(j).View = subPanel;
                        btns.getAt(j).getView = function () { return this.View; }
                    }
                }
            }

            ///查询面板按钮 增加当前主控件
            // 查询变量 bigPanel2.searchForm
            if (layouts.QueryView) {
                if (layouts.QueryView.Commands) {
                    let searchForm = bigPanel.getDockedItems('form')[0];
                    let searchBtns = searchForm.getDockedItems('toolbar[dock="top"]')[0].items;

                    for (let i = 0; len = searchBtns.getCount(), i < len; i++) {
                        searchBtns.getAt(i).View = searchForm;
                        searchBtns.getAt(i).getView = function () { return this.View; }
                    }

                    bigPanel.searchForm = searchForm;
                    bigPanel.searchForm.EntityType = layouts.QueryView.EntityType;
                }
            }
            ///设置View 视图列表 mainView
            bigPanel.views = [];
            for (let i = 0; len = layouts.Items.length, i < len; i++) {
                if (i == 0) {
                    bigPanel.mainView = bigPanel.getComponent(i);
                }
                bigPanel.views.push(bigPanel.getComponent(i));
            }

            ///增加主视图事件
            var selModel = bigPanel.mainView.getSelectionModel();
            selModel.mon(selModel, 'selectionchange', 'onMainViewItemSelected');
            return bigPanel;
        }
        else {
            var bigPanelConfig2 = {
                title: layouts.TabTitle,
                xtype: 'panel',
                items: [],
                tbar: [],
                controller: layouts.Controller || '',
                layout: 'fit'
            };

            var listConfig2 = AutoUI.GenerateListViewConfig(layouts);

            //查询面板
            if (layouts.QueryView) {
                var queryViewConfig2 = AutoUI.GenerateDetailConfig(layouts.QueryView);
                //console.log(queryViewConfig);
                bigPanelConfig2.lbar = queryViewConfig2;

                //增加分页
                listConfig2.bbar = {
                    xtype: 'pagingtoolbar',
                    displayInfo: true,
                    pageSize: 25
                };
                if (layouts.QueryView.PageSize) {
                    listConfig2.bbar.pageSize = layouts.QueryView.PageSize;
                    listConfig2.store.pageSize = layouts.QueryView.PageSize;
                }
            }

            bigPanelConfig2.items = listConfig2;

            if (fnChangeConfig) {
                Ext.callback(fnChangeConfig, this, [bigPanelConfig2]);
            }
            console.log(bigPanelConfig2);
            var bigPanel2 = Ext.create(bigPanelConfig2);

            //var list2 = Ext.create(listConfig2);
            ///按钮 增加当前主控件
            if (layouts.Commands) {
                var list2 = bigPanel2.down('grid');
                var btn2s = list2.getDockedItems('toolbar[dock="top"]')[0].items;
                //console.log(btn2s);
                for (let i = 0; len = btn2s.getCount(), i < len; i++) {
                    btn2s.getAt(i).View = list2;
                    btn2s.getAt(i).getView = function () { return list2; }
                }
            }

            ///查询面板按钮 增加当前主控件
            // 查询变量 bigPanel2.searchForm
            if (layouts.QueryView) {
                if (layouts.QueryView.Commands) {
                    let searchForm = bigPanel2.getDockedItems('form')[0];
                    let searchBtns = searchForm.getDockedItems('toolbar[dock="top"]')[0].items;

                    for (let i = 0; len = searchBtns.getCount(), i < len; i++) {
                        searchBtns.getAt(i).View = searchForm;
                        searchBtns.getAt(i).getView = function () { return searchForm; }
                    }

                    bigPanel2.searchForm = searchForm;
                    bigPanel2.searchForm.EntityType = layouts.QueryView.EntityType;
                }
            }

            // console.log('aa');

            ///设置View 视图列表 mainView
            bigPanel2.views = [bigPanel2.down('grid')];
            bigPanel2.mainView = bigPanel2.down('grid');
            ///增加主视图事件
            var selModel2 = bigPanel2.mainView.getSelectionModel();
            selModel2.mon(selModel2, 'selectionchange', 'onMainViewItemSelected');

            return bigPanel2;
        }
    },


    GenerateListViewConfig : function (layouts) {
        //Ext.create('Ext.grid.Panel', {
        //    title: 'Simpsons',
        //    store: Ext.data.StoreManager.lookup('simpsonsStore'),
        //    columns: [
        //        { text: 'Name', dataIndex: 'name' },
        //        { text: 'Email', dataIndex: 'email', flex: 1 },
        //        { text: 'Phone', dataIndex: 'phone' }
        //    ],
        //    height: 200,
        //    width: 400,
        //    renderTo: Ext.getBody()
        //});
        var gridConfig = {
            xtype: 'grid',
            EntityType: layouts.EntityType,
            title: layouts.Title,
            margin: 5,
            store: {
                autoLoad: false,
                model: layouts.EntityType,
                method: 'POST',
                proxy: {
                    type: 'ajax',
                    url: GroundApiUrl + '/Sys/GetQueryData', // url that will load data with respect to start and limit params
                    reader: {
                        type: 'json',
                        rootProperty: 'data.items',
                        totalProperty: 'data.total'
                    },
                    actionMethods: {
                        create: 'POST',
                        read: 'POST', // by default GET
                        update: 'POST',
                        destroy: 'POST'
                    },
                    headers: RequestX.DefaultHeaders
                },
                pageSize: 25
            },//Ext.data.StoreManager.lookup('simpsonsStore'),
            columns: [
                //{ text: 'Name', dataIndex: 'name' },
                //{ text: 'Email', dataIndex: 'email', flex: 1 },
                //{ text: 'Phone', dataIndex: 'phone' }
            ],
            tbar: [],
            plugins: []
        }

        gridConfig.layout = layouts.Layout || 'fit';
        var hasColumnEditor = false;
        for (var i = 0; len = layouts.ShowFields.length, i < len; i++) {
            let field = layouts.ShowFields[i];
            let colConfig = {
                text: field.Label,
                dataIndex: field.Name,
                width: field.TotalWidth || undefined
                //xtype: field.Editor
            };

            if (field.LabelWidth)
                colConfig.labelWidth = field.LabelWidth;
            if (field.TotalWidth)
                colConfig.width = field.TotalWidth;
            if (field.Hidden == true)
                colConfig.hidden = true;

            ///显示的格式  
            if (field.Format)
                colConfig.format = field.Format;

            //有可能有控件,但不允许编辑 下拉只读
            if (field.Editor) {
                colConfig.editor = {
                    xtype: field.Editor,
                    editable: field.Editable || true
                };

                if (field.EditorConfig) {
                    colConfig.editor = Ext.Object.merge(colConfig.editor, field.EditorConfig);
                    //枚举的数据
                    if (field.EditorConfig.EditorEunmData) {
                        colConfig.EditorEunmData = field.EditorConfig.EditorEunmData;
                        colConfig.editor.store = {
                            model: 'Gao.Models.EmumData',
                            data: Gao.Datas[field.EditorConfig.EditorEunmData]
                        };
                        //colConfig.editor.store.data = Gao.Datas[field.EditorConfig.EditorEunmData];

                        colConfig.renderer = function (c_value, c_metaData, c_record, c_rowIndex, c_colIndex, c_store, c_view) {
                            var headerCt = this.getHeaderContainer(),
                                column = headerCt.getHeaderAtIndex(c_colIndex);

                            var enumDatas = Gao.Datas[column.EditorEunmData];//c_record.get(column.EditorEunmData);
                            for (var i = 0; i < enumDatas.length; i++) {
                                if (enumDatas[i].Id == c_value)
                                    return enumDatas[i].NAME;
                            }
                            return '';
                        }
                    }
                }


                //显示 在Grid显示下拉框值
                if (field.IsCombobox) {
                    colConfig.editor.IsCombobox = true;

                    if (!colConfig.editor.DisplayItSelf) {
                        colConfig.renderer = function (c_value, c_metaData, c_record, c_rowIndex, c_colIndex, c_store, c_view) {
                            var headerCt = this.getHeaderContainer(),
                                column = headerCt.getHeaderAtIndex(c_colIndex);

                            return c_record.get(column.dataIndex + '_display');
                        }
                    }
                }



                ////日期显示格式 
                //if (field.DataType == "date") {
                //    colConfig.editor.dateFormat = 'Y-m-d'; //todo: 奇怪了,无效果
                //}

                hasColumnEditor = true;
            }

            //日期显示格式 
            if (field.DataType == "date") {
                colConfig.formatter = 'date("Y-m-d")';
            }

            gridConfig.columns.push(colConfig);
        }

        if (hasColumnEditor) {
            gridConfig.plugins.push({
                ptype: 'gaocellediting',
                clicksToEdit: 1

            });
        }

        ///显示按钮
        if (layouts.Commands) {
            for (let i = 0; len = layouts.Commands.length, i < len; i++) {
                var btnConfig = {
                    xclass: layouts.Commands[i].CommandPath,
                    text: layouts.Commands[i].Text,
                    iconCls: layouts.Commands[i].IconCls
                };

                gridConfig.tbar.push(btnConfig);
            }
        }

        return gridConfig;
    }
});

