Ext.define('ExtGAO.editors.SelectDeptEditor', {
    extend: 'ExtGAO.editors.BaseRemoteSelectEditor',
    fieldLabel: '',
    alias: ['widget.SelectDeptEditor', 'widget.selectdepteditor'],
    store: {
        model: 'GAOSelectBom.Models.Dept',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/SelectBom/SearchDept',
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
    //queryMode: 'remote',
    displayField: 'NAME',
    valueField: 'DEP'
});