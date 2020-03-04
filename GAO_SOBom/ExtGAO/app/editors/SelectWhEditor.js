Ext.define('ExtGAO.editors.SelectWhEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectWhEditor', 'widget.selectwheditor'],
    store: {
        model: 'GAOSelectBom.Models.MY_WH',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchWH',
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
    queryMode: 'local',
    displayField: 'NAME',
    valueField: 'WH'
});