Ext.define('ExtGAO.editors.SelectEunmEditor', {
    extend: 'Ext.form.ComboBox',
    fieldLabel: '',
    alias: ['widget.SelectEunmEditor', 'widget.selecteunmeditor'],
    store: {
        model: 'Gao.Models.EmumData',
        data: null//Gao.Datas.EunmTax
    },
    queryMode: 'local',
    displayField: 'NAME',
    valueField: 'Id'
});