Ext.define('ExtGAO.model.Base', {
    extend: 'Ext.data.Model',
    idProperty :'Id',
    schema: {
        namespace: 'ExtGAO.model'
    }
});

Ext.define('Gao.Models.EmumData', {
    extend: 'ExtGAO.model.Base',
    fields: [
        { "name": "Id", "type": "int" },
        { "name": "NAME", "type": "string"}
    ]
});
