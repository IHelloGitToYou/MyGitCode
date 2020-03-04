
Ext.define('FirstApp.view.page_so.FormPage', {
    extend: 'Ext.panel.Panel',
// Ext.define('FirstApp.view.page_so.FormPage', {
//     extend: 'Ext.form.Panel',
    xtype: 'FormPage',
    requires: [
        'FirstApp.store.Personnel'
    ],
    controller:'SO',
    bbar:[{
        text:'提交',
        handler :'SubmitForm'
    }],
    items:[
        {
            xtype:'formpanel',
            items: [{
                xtype: 'textfield',
                name: 'name',
                label: 'Name'
            }, {
                xtype: 'emailfield',
                name: 'email',
                label: 'Email'
            }, {
                xtype: 'passwordfield',
                name: 'password',
                label: 'Password'
            }]            
        },{
            xtype:'mainlist'
        }
    ]
    
});
