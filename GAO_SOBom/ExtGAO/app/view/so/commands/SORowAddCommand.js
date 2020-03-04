
Ext.define('ExtGAO.view.so.commands.SORowAddCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            store = view.getStore(),
            controller = view.up('panel').getController(),
            form = view.up('panel').down('form'),
            boxEstDD = Ext.ComponentQuery.query('field[name=EST_DD]', form)[0];

        var rec = Ext.create('GAOSelectBom.Models.TF_Pos', {
            Id: controller.getNewId(),
            WH_display: '0000',
            EST_DD:boxEstDD.getValue()
            //T1: 'A0006',
            //T1_display: 'A0006名'
        });

        store.add(rec);
    }
});
