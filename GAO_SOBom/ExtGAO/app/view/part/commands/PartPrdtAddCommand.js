
Ext.define('ExtGAO.view.part.commands.PartPrdtAddCommand', {
    extend: 'ExtGAO.helpers.BaseCommand',
    Execute: function (view, viewModel) {
        var me = this,
            bigPanel = view.up('panel'),
            controller = bigPanel.getController();
         

        if (!controller.MainViewCurrent) {
            AppTool.ShowOKMsg('用户记录未选择!');
            return;
        }

        var newRec = Ext.create(view.EntityType);
        newRec.set('PartId', controller.MainViewCurrent.get('Id'));
        newRec.set('PartNo', controller.MainViewCurrent.get('PartNo'));
        newRec.set('Id', controller.getNewId());
        view.getStore().add(newRec);
         
    }
});