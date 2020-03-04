Ext.define('ExtGAO.store.Personnel', {
    extend: 'Ext.data.Store',

    alias: 'store.personnel',

    model: 'ExtGAO.model.Personnel',

    data: {
        items: [
            { name: 'Jean Luc', email: "jeanluc.pic  发ard@enterprise.com", phone: "555-111-1111" },
            { name: 'Worf', email: "worf.moghsson@enf大规模terprise.com", phone: "555-222-2222" },
            { name: 'Deanna', email: "deanna.troi@enterprise.com", phone: "555-333-3333" },
            { name: 'Data', email: "mr.data@enterprise.com", phone: "555-444-4444" }
        ]
    },

    proxy: {
        type: 'memory',
        reader: {
            type: 'json',
            rootProperty: 'items'
        }
    }
});
