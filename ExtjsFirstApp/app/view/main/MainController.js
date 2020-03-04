/**
 * This class is the controller for the main view for the application. It is specified as
 * the "controller" of the Main view class.
 */
Ext.define('FirstApp.view.main.MainController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.main',

    onItemSelected: function (sender, record) {
        var formModel = this.getViewModel();
        var form = this.getView();

        var p = Ext.create('Ext.form.Panel', {
            title:'Tssf',
            defaults: {
                xtype: 'textfield',
                labelAlign: 'left',

                // margin: '10 0',
                // value: 'invalid email@foo.com',
                // validators: 'email',
                // errorTip: {
                //     anchor: true,
                //     align: 'l-r?'
                // }
            },
            layout:'column',
            items: [{
                fieldLabel: 'Column 1',
                columnWidth: 0.25
            },{
                fieldLabel: 'Column 2',
                columnWidth: 0.55
            },{
                fieldLabel: 'Column 3',
                columnWidth: 0.20
            },{
                fieldLabel: 'Column 3',
                columnWidth: 0.5
            }]
                    // layout: {
            //     type:'form',
            //     //columns :5
            // },
            // items: [
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'First Name'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'Bank Account NumberBank Account Number'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'First Name'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'First Name2'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'First Name2'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'First Name'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'First Name'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'Last Name'
            //     },
            //     {
            //         xtype: 'textfield',
            //         fieldLabel: 'Bank Account Number',
            //         labelWidth :'60'
            //     },
            //     {
            //         xtype: 'checkboxfield',
            //         fieldLabel: 'Approved'
            //     }]
            });        

        form.add(p);return;
       
        var newForm = Ext.create('FirstApp.view.page_so.FormPage',{
            tab:{ title:'打开单据' }
        });
        
        // newForm.record = record[0];
        // newForm.record.commit();
        // //console.log();
        // newForm.setValues(record[0].data);

        form.add(newForm);

        Ext.Msg.confirm('Confirm', 'Are you sure?', 'onConfirm', this);
    },
    indext:0 ,
    onConfirm: function (choice) {
        if (choice === 'yes') {

            var formModel = this.getViewModel();
            formModel.set('name','name 11_23456789AA');
            console.log(formModel.get('name'));

            //formModel.set('expiry', Ext.Date.add(new Date(), Ext.Date.DAY, 7));
            var form = this.getView();
            
            var AA = Ext.create(
                {
                    title:'SOO单2',
                    tab:{title:'SOO单2'},
                    xtype:'PageSOlist'
                }
            );
         
            var BB = Ext.create(
                {
                    title:'SA单3',
                    tab:{title:'SA单3'},
                    xtype:'PageSalist'
                }
            );
             
            form.add(AA);
            form.add(BB);

            return;
        }
    }
});
