Ext.define('FirstApp.view.page_so.PageSO', {
    extend: 'Ext.grid.Grid',
    xtype: 'PageSOlist',
    requires: [
        'FirstApp.store.Personnel'
        //'Ext.grid.Grid'
    ],
    title: 'SO单1',
    controller:'SO',
    selectable:{
        mode:'multi'
       // checkbox :true
    },
    store: {
        type: 'personnel'
    },

    columns: [{ 
        text: 'Name3',
        dataIndex: 'name',
        width: 100,
        cell: {
            userCls: 'bold'
        }
    }, {
        text: 'Email2',
        dataIndex: 'email',
        width: 230 
    }, { 
        text: 'Phone2',
        dataIndex: 'phone',
        width: 150 
    }],
    afterRender:function(){
        alert('afterRender');
    },
    listeners: {
        select: 'SayHello'
    }
});
