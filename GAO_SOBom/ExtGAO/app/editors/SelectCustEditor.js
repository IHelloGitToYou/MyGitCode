Ext.define('ExtGAO.editors.SelectCustEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectCustEditor', 'widget.selectcusteditor'],
    store: {
        model: 'GAOSelectBom.Models.Cust',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchCust',
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
    valueField: 'CUS_NO'
});