
Ext.define('ExtGAO.view.part.commands.SelectPrdtAddCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            bigPanel = view.up('panel'),
            controller = bigPanel.getController();

        var nesIDDD = controller.getNewId();
        console.log(nesIDDD);
        var newRec = Ext.create(view.EntityType);
        newRec.set('Id', nesIDDD);
        view.getStore().add(newRec);
    }
});