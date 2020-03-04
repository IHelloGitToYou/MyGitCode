Ext.define('ExtGAO.editors.CheckboxOfTFValueEditor', {
    extend: 'Ext.form.field.Checkbox',
    fieldLabel: '',
    alias: ['widget.CheckboxOfTFValueEditor', 'widget.checkboxoftfvalueeditor'],
    inputValue: 'T',
    getValue: function () {
        var inputEl = this.inputEl && this.inputEl.dom;

        var result = inputEl ? inputEl.checked : this.checked;
        return result ? this.inputValue : 'F';
    }
});
