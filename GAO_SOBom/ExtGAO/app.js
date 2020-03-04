/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'ExtGAO.Application',

    name: 'ExtGAO',

    requires: [
        // This will automatically load all classes in the ExtGAO namespace
        // so that application classes do not need to require each other.
        'ExtGAO.*'
    ],

    // The name of the initial view to create.
    mainView: 'ExtGAO.view.main.Main'
});
