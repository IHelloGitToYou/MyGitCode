/*
 * This file launches the application by asking Ext JS to create
 * and launch() the Application class.
 */
Ext.application({
    extend: 'FirstApp.Application',

    name: 'FirstApp',

    requires: [
        // This will automatically load all classes in the FirstApp namespace
        // so that application classes do not need to require each other.
        'FirstApp.*'
    ],

    // The name of the initial view to create.
    mainView: 'FirstApp.view.main.Main'
});
