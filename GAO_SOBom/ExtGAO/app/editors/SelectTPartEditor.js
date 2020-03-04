Ext.define('ExtGAO.editors.SelectTPartEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectTPartEditor', 'widget.selecttparteditor'],
    Part: '',       //选配项的编码
    store: {
        model: 'GAOSelectBom.Models.PartPrdt',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchTPartPrdt',
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
    displayField: 'PrdName',
    valueField: 'PrdNo',
    forceSelection: false,
    beforeQuery(queryPlan) {
        var me = this,
            proxy = me.store.getProxy(),
            t_prd_no = me.ownerCt.grid.getSelection()[0].get('Z_T_PRD_NO');

        if (!t_prd_no) {
            queryPlan.cancel = true;
            return queryPlan;
        }

        proxy.setExtraParam("m_prd_no", t_prd_no);
        proxy.setExtraParam("PartNo", me.PartNo);
         //alert('a');
        return me.callParent([queryPlan]);//base.beforeQuery(queryPlan);
    }
});