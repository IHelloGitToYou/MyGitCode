
Ext.define('FirstApp.IFrame', {
    extend: 'Ext.Container',
    xtype: 'app-IFrame',
    //html:'www.baidu.com'
    //fullscreen: true,
    layout: 'fit',
    onRender: function(ct, position){
       
        this.el = this.element.createChild({
            title:  this.title,
            tag: 'iframe', 
            //id: 'iframe-'+ this.id, 
            frameBorder: 0, 
            src: this.url
        });
    }
});