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
    queryMode: 'local',
    displayField: 'NAME',
    valueField: 'PRD_LOC'
});