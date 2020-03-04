Ext.define('ExtGAO.editors.SelectSalmEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectSalmEditor', 'widget.selectsalmeditor'],
    store: {
        model: 'GAOSelectBom.Models.Salm',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchSalm',
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
    displayField: 'NAME',
    valueField: 'SAL_NO'
});