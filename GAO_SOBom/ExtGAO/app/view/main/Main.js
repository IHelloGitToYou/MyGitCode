/**
 * This class is the main view for the application. It is specified in app.js as the
 * "mainView" property. That setting automatically applies the "viewport"
 * plugin causing this view to become the body element (i.e., the viewport).
 *
 * TODO - Replace this content of this view to suite the needs of your application.
 */
Ext.define('ExtGAO.view.main.Main', {
    extend: 'Ext.tab.Panel',
    xtype: 'app-main',

    requires: [
        'Ext.plugin.Viewport',
        'Ext.window.MessageBox',

        'ExtGAO.view.main.MainController',
        'ExtGAO.view.main.MainModel',
        
        'ExtGAO.view.main.List',
        'ExtGAO.editors.*',
        'ExtGAO.controllers.*',
        'Gao.Models.*',
        'ExtGAO.helpers.*',
        'ExtGAO.helpers.BaseCommand',
        'AppTool',
        'AutoUI',
        'RequestX',
        'Ext.layout.container.*',
        'Ext.window.Toast'
    ],

    controller: 'main',
    viewModel: 'main',

    ui: 'navigation',

    tabBarHeaderPosition: 1,
    titleRotation: 0,
    tabRotation: 0,
    //closable :true,
    header: {
        layout: {
            type: 'hbox',
            align: 'stretchmax'
            //align: 'stretchmax'
        },
        title: {
            bind: {
                text: '{name}'
            },
            flex: 0
        },
        iconCls: 'fa-th-list'
    },

    tabBar: {
        flex: 1,
        layout: {
            align: 'stretch',
            overflowHandler: 'none'
        }
    },

    responsiveConfig: {
        tall: {
            headerPosition: 'top'
        },
        wide: {
            headerPosition: 'top'
        }
    },

    defaults: {
        bodyPadding: 5,
        tabConfig: {
            responsiveConfig: {
                wide: {
                    iconAlign: 'top',
                    textAlign: 'center'
                },
                tall: {
                    iconAlign: 'left',
                    textAlign: 'left',
                    //iconAlign: 'top',
                    //textAlign: 'center',
                    width: 120
                }
            }
        }
    },

    items: [
        {
            
            title: '菜单',
            items: [{
                    xtype: 'fieldset',
                    title: '系统维护',
                    //width: 400,
                    bodyPadding: 10,
                    layout: {
                        type: 'hbox',
                        align: 'middle'
                    },
                    defaults: {
                        width: 120,
                        xtype: 'button'
                    },
                    items: [{
                        text: '用户管理',
                        margin: '0 0 0 10',
                        iconCls: 'x-fa fa-graduation-cap',
                        PageInfo: {
                            EntityType: 'Gao.Models.User',
                            ViewGroup: 'LIST',
                            IsList: true
                        },
                        listeners: {
                            click: 'onMenuClick'
                        }
                    }]
                },
                {
                    xtype: 'fieldset',
                    title: '智选BOM',
                    bodyPadding: 10,
                    layout: {
                        type: 'hbox',
                        align: 'middle'
                    },
                    defaults: {
                        width: 150,
                        xtype: 'button'
                    },
                    items: [{
                            text: '模块管理',
                            margin: '0 0 0 10',
                            iconCls: 'x-fa fa-graduation-cap',
                            PageInfo: {
                                EntityType: 'GAOSelectBom.Models.Part',
                                ViewGroup: 'LIST',
                                IsList: true
                            },
                            listeners: {
                                click: 'onMenuClick'
                            }
                        },{
                            text: '销售订单',
                            margin: '0 0 0 10',
                            iconCls: 'x-fa fa-th-list',
                            PageInfo: {
                                EntityType: 'GAOSelectBom.Models.MF_Pos',
                                ViewGroup: 'LIST',
                                IsList: true
                            },
                            listeners: {
                                click: 'onMenuClick'
                            }
                        }
                    ]
                }]
        }
        //,
       // {
       // title: 'Home',
       // //iconCls: 'fa-home',
       // // The following grid shares a store with the classic version's grid as well!
       // items: [{
       //     xtype: 'mainlist'
       // }]
    //}
    ],
    listeners: {
        boxready: function (o) {
            //ExtGAO.configs.DiyLoad1.Init();
            function getUrlParam(name) {
                var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)');
                var r = window.location.search.substr(1).match(reg);
                if (r != null) return decodeURIComponent(r[2]); return null;
            }
            RequestX.DefaultHeaders['X-LOGIN-ID'] = getUrlParam('X-LOGIN-ID');

            //console.log(Ext.USE_NATIVE_JSON);
            Ext.USE_NATIVE_JSON = false;
            Ext.JSON.encodeDate = function (o) {
               // console.log(o);
                var pad = function (n) {
                    return n < 10 ? "0" + n : n;
                };

                return '"' + o.getFullYear() + "-" + pad(o.getMonth() + 1) + "-" + pad(o.getDate()) + " " + pad(o.getHours()) + ":" + pad(o.getMinutes()) + ":" + pad(o.getSeconds()) + '"';
            };

            Number.prototype.gaoRoundIndexs = {
                0 :1,
                1: 10,
                2: 100,
                3: 1000,
                4: 10000,
                5: 100000,
                6: 1000000,
                7: 10000000,
                8: 100000000,
                9: 1000000000
            };

            /*给Number类型增加一个方法，调用起来更加方便。*/
            Number.prototype.gaoRound = function (v_precision) {
                var v_number = this;
                v_precision = v_precision || 0;
                var fixNumber = Number.prototype.gaoRoundIndexs[v_precision];

                return Math.round(v_number * fixNumber) / fixNumber;
            }
            
        }
    }
});
