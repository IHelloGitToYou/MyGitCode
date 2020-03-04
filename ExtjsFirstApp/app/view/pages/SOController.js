Ext.define('FirstApp.view.pages.SOController',{
    extend:'Ext.app.ViewController',
    alias:'controller.SO',
    SayHello:function(vthis, records){
        var so = this.getView();
        console.log('so');
        console.log(so);

        var store = so.getStore();
        //store.remove(records);
        //alert('Hello New Life!');
    },
    SubmitForm:function(){
        var formPage = this.getView();
        
        formPage.fillRecord(formPage.record);
        console.log(formPage.record);
    }
});
 