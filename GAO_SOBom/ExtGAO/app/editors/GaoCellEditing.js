Ext.define('ExtGAO.editors.GaoCellEditing', {
    alias: 'plugin.gaocellediting',
    extend: 'Ext.grid.plugin.CellEditing',//'Ext.grid.plugin.Editing',
   // requires: ['Ext.grid.CellEditor', 'Ext.util.DelayedTask'],
    onEditComplete: function (ed, value, startValue) {
        ///值无变不更新Display与联动,否则Store无加载记录,会清空数据
        if (value == startValue) {
            this.callParent([ed, value, startValue]);
            return;
        }

        var activeEd = this.getActiveEditor(),
            editor = activeEd.getComponent(0);
        //console.log(arguments);
        //下拉框Setter与_display 设值
        if (editor.IsCombobox) {
           var editorStore = activeEd.getComponent(0).store,
                gridRecord = activeEd.context.record,
                columnDataIndex = activeEd.column.dataIndex,
                editorRecord = null;

            if (value !== null && value !== undefined) {
                editorRecord = editorStore.findRecord(editor.valueField, value);
                if (editorRecord) {
                    gridRecord.set(columnDataIndex + '_display', editorRecord.get(editor.RendererDisplayField));
                }
                else {
                    var gridRecDispaly = gridRecord.get(columnDataIndex + '_display');
                    if (gridRecDispaly)
                        gridRecord.set(columnDataIndex + '_display', gridRecDispaly);
                    else
                        gridRecord.set(columnDataIndex + '_display', value);
                }
            }
            else {
                gridRecord.set(columnDataIndex + '_display', '');
            }
            //console.log('editor.IsCombobox');

            if (editor.SetterOtherField) {
                var setterOtherField = editor.SetterOtherField;//{ "CompNo": "CompNo", "Name": "CompName" };

                for (var key in setterOtherField){//遍历对象的所有属性，包括原型链上的所有属性
                    //if (setOtherField.hasOwnProperty(key)){ //判断是否是对象自身的属性，而不包含继承自原型链上的属性
                    var dataIndex = setterOtherField[key];
                    //console.log(key);        //键名
                    //console.log(setterOtherField[key]);   //键值

                    if (editorRecord !== null)
                        gridRecord.set(dataIndex, editorRecord.get(key));
                    else
                        gridRecord.set(dataIndex, '');
                    // }
                }
            }
        }

        this.callParent([ed, value, startValue]);
        //console.log('after onEditComplete');
    }
    //activateCell: function (position, skipBeforeCheck, doFocus){
    //    //console.log('before activateCell');
    //    //this.callParent([position, skipBeforeCheck, doFocus]);
    //},
    //deactivate: function () {
    //    console.log('before deactivate');

     
    //    this.callParent([]);
    //},
    //resume: function (position) {
    //    console.log('before resume');
    //    this.callParent([position]);
    //}
});