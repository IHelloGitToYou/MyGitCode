Ext.define('ExtGAO.editors.GaoFormEditing', {
    alias: 'plugin.gaoformediting',
    extend: 'Ext.plugin.Abstract',
    init: function (form) {
        var me = this;
        me.form = form;
        //console.log(form);
        me.fieldListeners = [];
        me.comboboxs = [];

        me.findCombobox();

        //console.log('form!!!! gaoformediting');
        me.formListeners = form.mon(form, 'boxready', function () {           
            for (var i = 0; i < me.comboboxs.length; i++) {
                var editor = me.comboboxs[i];
                var lner = editor.mon(editor, 'change', me.doComBoxSetter, editor.up('form'));
                me.fieldListeners.push(lner);
            }
        });

        //console.log('me.form.loadRecord');

        /////编辑单据时,设置下拉框的显示值
        me.form.loadRecord = function (record) {

            var result = me.form.form.loadRecord(record);

            for (var i = 0; i < me.comboboxs.length; i++) {
                var editor = me.comboboxs[i],
                    editorName = editor.name,
                    editorDataModel = editor.store.model;

                ///附加Picker Record 目的在未加载记录前渲染 display值
                //console.log(editorName);
                var editorPickerRecord = Ext.create(editorDataModel, {
                    //Id: record.get(editorName)
                });

                editorPickerRecord.set(editor.valueField, record.get(editorName));
                editorPickerRecord.set(editor.displayField, record.get(editorName + '_display'));

                //var prdtSelRecord = Ext.create('GAOSelectBom.Models.Prdt', { Id: 4, Prd_No: 'A0002', Name: 'A0002' });
                //var editor = form.getForm().findField('ReplacePrdId');
                editor.setValue(editorPickerRecord);

                var value1 = editor.getValue();
                var value2 = editor.getRawValue();
                console.log({ value1: value1, rawValue: value2 });
            }


            return result;
        };
            
        me.callParent(arguments);
    },

    findCombobox: function () {
        var me = this;
        var comboboxs = Ext.ComponentQuery.query('BaseRemoteSelectEditor', me.form);
        for (var i = 0; i < comboboxs.length; i++) {
            me.comboboxs.push(comboboxs[i]);
        }

        //子form
        var subForms = Ext.ComponentQuery.query('form', me.form);
        for (var i = 0; i < subForms.length; i++) {
            var comboboxs2 = Ext.ComponentQuery.query('BaseRemoteSelectEditor', subForms[i]);
            for (var j = 0; j < comboboxs2.length; j++) {
                me.comboboxs.push(comboboxs2[j]);
            }
        }
    },

    //下拉框Setter与_display 设值
    doComBoxSetter: function (editor, value, oldValue, eOpts ) {
        var me = this,
            editorStore = editor.store,
            editorRecord = null,
            formRecord = me.record;

        if (!editorStore || !formRecord)
            return;

        if (value !== null && value !== undefined) {
            editorRecord = editorStore.findRecord(editor.valueField, value);
        }

        if (editor.SetterOtherField) {
            var setterOtherField = editor.SetterOtherField;//{ "CompNo": "CompNo", "Name": "CompName" };

            for (var key in setterOtherField){ //遍历对象的所有属性，包括原型链上的所有属性
                //if (setOtherField.hasOwnProperty(key)){ //判断是否是对象自身的属性，而不包含继承自原型链上的属性
                var dataIndex = setterOtherField[key],
                    selector = 'field[name=' + dataIndex + ']',
                    components = Ext.ComponentQuery.query(selector, me);
                //console.log(selector);
                //console.log(components);
                
                //console.log(key);                     //键名
                //console.log(setterOtherField[key]);   //键值
                if (editorRecord !== null) {
                    formRecord.set(dataIndex, editorRecord.get(key));
                    if (components.length > 0)
                        components[0].setValue(editorRecord.get(key));
                }
                else {
                    formRecord.set(dataIndex, '');
                    if (components.length > 0)
                        components[0].setValue(''); ///要联动的栏位,都是String类型,所以设''
                }
                // }
            }
        }
    },
    destroy: function () {
        var me = this;
        Ext.destroy(me.formListeners);
        for (var i = 0; i < me.fieldListeners.length; i++) {
            Ext.destroy(me.fieldListeners[i]);
        }
        me.callParent();
    }
});