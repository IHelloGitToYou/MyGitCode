Ext.define('ExtGAO.editors.BaseRemoteSelectEditor', {
    extend: 'Ext.form.ComboBox',
    fieldLabel: '',
    alias: ['widget.BaseRemoteSelectEditor', 'widget.baseremoteselecteditor'],
    store:null,
    pageSize: 25,
    queryMode: 'remote',
    //displayField: 'PrdNo',
    //valueField: 'Id',
    queryCaching: false,
    matchFieldWidth: false,
    minChars: 1,
    forceSelection: true
    //listeners: {
    //    beforequery: function (queryPlan, eOpts) {
    //        //queryPlan.combo.lastQuery = queryPlan.combo.getRawValue();
    //        ////console.log(queryPlan.combo.lastQuery);
    //    }
    //}
});