Ext.define('ExtGAO.editors.CheckboxOfTFValueEditor', {
    extend: 'Ext.form.field.Checkbox',
    fieldLabel: '',
    alias: ['widget.CheckboxOfTFValueEditor', 'widget.checkboxoftfvalueeditor'],
    inputValue: 'T',
    getValue: function () {

        console.log('getValue');
        var inputEl = this.inputEl && this.inputEl.dom;

        var result = inputEl ? inputEl.checked : this.checked;

        console.log(result ? this.inputValue : 'F');
        return result ? this.inputValue : 'F';
    },

    updateCheckedCls: function (checked) {
        var me = this;
        if (checked == null)
            checked = me.getValue();

        if (checked == true || checked == me.inputValue) 
            checked = true;
        else
            checked = false;

        me[checked ? 'addCls' : 'removeCls'](me.checkedCls);
    }
});
