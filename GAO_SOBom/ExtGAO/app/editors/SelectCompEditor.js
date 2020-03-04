Ext.define('ExtGAO.editors.SelectCompEditor', {
    extend: 'Ext.form.ComboBox',
    fieldLabel: '',
    alias: ['widget.SelectCompEditor', 'widget.selectcompeditor'],
    store: {
        model: 'Gao.Models.Comp',
        proxy: {
            type: 'ajax',
            url: GroundApiUrl + '/Sys/GetComps',
            reader: {
                type: 'json',
                rootProperty: 'data'
            }
        },
        autoLoad: true
    },
    //store: {
    //    fields: [
    //        { name: 'id', type: 'int' },
    //        'name'
    //    ],
    //    data: [
    //        { "id": 1, "name": "Alabama1" },
    //        { "id": 3, "name": "Alaska3" },
    //        { "id": 8, "name": "Arizona8" }
    //    ]
    //},
    queryMode: 'local',
    displayField: 'CompNo',
    valueField: 'Id'
});