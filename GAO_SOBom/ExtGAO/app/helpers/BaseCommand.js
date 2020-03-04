Ext.define('ExtGAO.helpers.BaseCommand', {
    extend: 'Ext.button.Button',
    Execute: function (view, viewModel) {
        console.log('Base Execute');
        this.Post('Base Execute');
    },
    //提交到后台API
    Post: function (JsonString, successCallBack, fnScore) {
        var data,
            xclass = this.xclass,
            success = false;

        RequestX.RequestPostSync('Sys/GetCommandClick', { Command: xclass, PostData: JsonString },
            function (data2) {
                data = data2;
                success = true;
            }, this); 

        //console.log(data);
        if (success) {
            Ext.callback(successCallBack, fnScore || this, [data]);
        }

        return success;
    },
    handler: function () {
        var view = this.getView();
        //var viewModel = this.getViewModel();
        this.Execute(view, null);
    }
});
