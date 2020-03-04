Ext.define('ExtGAO.editors.SelectPrdtEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectPrdtEditor', 'widget.selectprdteditor'],
    store: {
        model: 'GAOSelectBom.Models.Prdt',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchPrdt',
            reader: {
                type: 'json',
                rootProperty: 'data.items',
                totalProperty: 'data.total'
            },
            headers: RequestX.DefaultHeaders
        }
        //autoLoad: true
    },
    pageSize: 10,
    //queryMode: 'remote',
    displayField: 'Prd_No',
    valueField: 'Id'
});