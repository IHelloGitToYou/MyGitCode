Ext.define('ExtGAO.editors.GaoDateEditor', {
    extend: 'Ext.form.field.Date',
    fieldLabel: '',
    alias: ['widget.GaoDateEditor', 'widget.gaodateeditor'],
    submitFormat:'Y-m-d H:i:s',
    dateFormat: 'Y-m-d',
    format: 'Y-m-d',
    getValue: function() {
        return this.getSubmitValue();// || null;
    }
});