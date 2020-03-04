Ext.define('FirstApp.view.page_so.PageSa', {
    extend: 'Ext.grid.Grid',
    xtype: 'PageSalist',
    requires: [
        'FirstApp.store.Personnel'
        //'Ext.grid.Grid'
    ],

    //title: 'SAAAAAAAA单1',
   
    store: {
        type: 'personnel'
    },

    columns: [{ 
        text: 'Name4',
        dataIndex: 'name',
        width: 100,
        cell: {
            userCls: 'bold'
        }
    }, {
        text: 'Email4',
        dataIndex: 'email',
        width: 230 
    }, { 
        text: 'Phone4',
        dataIndex: 'phone',
        width: 150 
    }]
});
