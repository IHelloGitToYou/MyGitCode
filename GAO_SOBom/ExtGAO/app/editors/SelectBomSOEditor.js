Ext.define('ExtGAO.editors.SelectBomSOEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectBomSOEditor', 'widget.selectbomsoeditor'],
    store: {
        model: 'GAOSelectBom.Models.BomSO',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchBomSO',
            reader: {
                type: 'json',
                rootProperty: 'data.items',
                totalProperty: 'data.total'
            },
            headers: RequestX.DefaultHeaders
        },
        autoLoad: true
    },
    pageSize: 10,
    queryMode: 'remote',
    displayField: 'BOM_NO',
    valueField: 'BOM_NO',
    forceSelection: false,
    showGridPicker: function (xy) {
        var me = this;
        console.log(me);
        var gridPanel = AutoUI.GenerateListView('GAOSelectBom.Models.BomSO', 'LIST',
            function (panelConfig) {
                
                panelConfig.items.listeners = {
                    itemdblclick: function (gridThis, record, item, index, e, eOpts) {
                        //console.log(123123);
                        me.setValue(record);
                       // me.completeEdit();
                        var dataIndex = me.ownerCt.context.column.dataIndex;
                        me.ownerCt.context.record.set(dataIndex, me.getValue());
                        //me.ownerCt.context.record.set(dataIndex + '_display', record.get(me.displayField));
                        me.Win.close();
                        //me.updateValue();
                        //me.ownerCt.onEditComplete(me, me.getValue() );
                        //me.ownerCt.completeEdit();
                        
                    }
                    //boxready: function () {
                    //    console.log('bindStore');
                    //    me.bindStore( this.getStore());
                    //   // me.getStore = function () { return this.store; }
                    //}
                };
            });

        //gridPanel.margin = '-30 0 0 0';
        me.Win = Ext.create('Ext.window.Window', {
            closeAction: 'hide',
            title: '选择BOM',
            //header: false,
            height: 600,
            width: 800,
            layout: 'fit',
            items: gridPanel
        });

        me.Win.show();//me.getXY()
    },
    //回收自定义Win
    destroy: function () {
        if(this.Win)
            this.Win.destroy();
        this.callParent([]);
    },
    onTriggerClick: function (field, trigger, e) {
        var me = this,
            xy = me.getXY();
        if (!me.Win)
            me.showGridPicker(xy);
        else
            me.Win.show();
        //var menu = Ext.create('Ext.menu.Menu', {
        //    //width: 100,
        //    plain: true,
        //    floating: false,  // usually you want this set to True (default)
        //    items: [{
        //        text: '标准BOM',
        //        handler: function () {
        //            me.showGridPicker(xy);
        //            //baseonTriggerClick();
        //            this.up('window').close();
        //        }
        //    }, {
        //        text: '订单BOM',
        //        handler: function () {
        //            me.showGridPicker(xy);
        //            //baseonTriggerClick();
        //            this.up('window').close();
                    
        //        }
        //    }]
        //});//.show();

        //Ext.create('Ext.window.Window', {
        //    title: false,
        //    header: false,
        //    height: 80,
        //    width: 80,
        //    layout: 'fit',
        //    items: menu
        //}).showAt(me.getXY());
    }
    //createPicker: function () {
    //    var me = this;
    //    //me.callParent([]);
    //    var gridPanel = AutoUI.GenerateDetailView('GAOSelectBom.Models.BomSO', 'LIST', function (panelConfig) {
            
    //    });

    //    return gridPanel.mainView;
    //}
});