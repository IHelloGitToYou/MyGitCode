Ext.define('ExtGAO.editors.SelectWhLocEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectWhLocEditor', 'widget.selectwhloceditor'],
    store: {
        model: 'GAOSelectBom.Models.WHLocation',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchWHLoc',
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
    displayField: 'NAME',
    valueField: 'PRD_LOC',
    beforeQuery(queryPlan) {
        var me = this,
            proxy = me.store.getProxy(),
            wh = me.ownerCt.grid.getSelection()[0].get('WH');
        //console.log('whhhhh');
        if (!wh) {
            queryPlan.cancel = true;
            return queryPlan;
        }

        proxy.setExtraParam("WH", wh);
 
        return me.callParent([queryPlan]);//base.beforeQuery(queryPlan);
    }
});