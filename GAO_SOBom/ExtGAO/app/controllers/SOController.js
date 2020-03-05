Ext.define('ExtGAO.controllers.SOController', {
    extend: 'ExtGAO.controllers.BaseController',
    alias: 'controller.so_controller',
    onMainViewItemSelected: function (sender, records) {
        var view = this.getView();
        this.callParent([sender, records]);
    },
    SetChangeListeners: function (formPanel) {
        var me = this,
            form = formPanel.mainView,
            grid = formPanel.views[1],
            gaoCE = grid.findPlugin('gaocellediting');

        gaoCE.controller = me;

        //内部单价、金额公式、
        // 预交日
        var boxEstDD = Ext.ComponentQuery.query('field[name=EST_DD]', form)[0];
        boxEstDD.BodyGrid = grid;
        form.mon(boxEstDD, 'change', function (vthis, newValue, oldValue, eOpts) {
            var store = vthis.BodyGrid.getStore();
            for (var i = 0; i < store.getCount(); i++) {
                store.getAt(i).set('EST_DD', newValue);
            }
        });

        var boxTaxId = Ext.ComponentQuery.query('field[name=TAX_ID]', form)[0];
        boxTaxId.BodyGrid = grid;
        boxTaxId.BodyGridCE = gaoCE;
        
        form.mon(boxTaxId, 'change', function (vthis, newValue, oldValue, eOpts) {
            var store = vthis.BodyGrid.getStore();
            for (var i = 0; i < store.getCount(); i++) {
                vthis.BodyGridCE.fireEvent('edit', null, {
                    record: store.getAt(i),
                    field : 'TAX_RTO'
                });
            }
        });

        //切换客户带立帐\扣税方式
        var boxCusNo = Ext.ComponentQuery.query('field[name=CUS_NO]', form)[0];
        boxCusNo.boxTaxId = Ext.ComponentQuery.query('field[name=TAX_ID]', form)[0];
        boxCusNo.boxZhangId = Ext.ComponentQuery.query('field[name=ZHANG_ID]', form)[0];
        
        form.mon(boxCusNo, 'change', function (vthis, newValue, oldValue, eOpts) {
            if (!newValue) return;

            var custBoxStore = vthis.store,
                editorRecord = custBoxStore.findRecord(vthis.valueField, newValue);
            if (!editorRecord) return;

            vthis.boxTaxId.setValue(editorRecord.get('ID1_TAX'));
            vthis.boxZhangId.setValue(editorRecord.get('CLS2'));
        });


        var boxSalNo = Ext.ComponentQuery.query('field[name=SAL_NO]', form)[0];
        var boxDEP = Ext.ComponentQuery.query('field[name=PO_DEP]', form)[0];
        //业务员变了,更新所属部门
        if (Gao.AllTable.DepNoForm == 'SALM') {
            boxSalNo.boxDEP = boxDEP;
            form.mon(boxSalNo, 'change', function (vthis, newValue, oldValue, eOpts) {
                if (!newValue) return;

                RequestX.RequestGet('SO/GetUserDept', { SalNo: newValue },
                    function (data2) {
                        if (!data2) return;
                        data2.DEP = data2.DEP || '0000';
                        boxDEP.setValue(data2.DEP);
                    }, this);
            });
        }
        else {
            boxDEP.setValue('0000');
            //RequestX.RequestGet('SO/GetUserDept', { SalNo: 'xx' },
            //    function (data2) {
            //        if (!data2) return;
            //        data2.DEP = data2.DEP || '0000';
            //        boxDEP.setValue(data2.DEP);
            //    }, this);
        }


        gaoCE.form2 = form;
        gaoCE.mon(gaoCE, 'edit', function (v_editor, v_context, v_eOpt) {
            //console.log('ediitttt');
            var rec = v_context.record,
                form2 = this.form2,
                taxId = Ext.ComponentQuery.query('field[name=TAX_ID]', form2)[0].getValue(),
                precision = GAOPrecisionNumber.AMTN || 0,
                amt = Number(rec.get('UP') * rec.get('QTY').gaoRound(precision)),
                tax = 0,
                taxRTO = rec.get('TAX_RTO') || 0;

            if (v_context.field == 'FREE_ID' || v_context.field == 'QTY'
                    || v_context.field == 'UP' || v_context.field == 'TAX_RTO') {
                rec.set('AMTN', amt);
                rec.set('AMT', amt);
                rec.set('TAX', 0);

                if (taxId == 2 && taxRTO > 0) {
                    //内含税
                    var net = amt / ((100 + taxRTO) / 100);
                    net = Number(net.gaoRound(precision));
                    tax = amt - net;
                    rec.set('AMTN', net.gaoRound(precision));
                    rec.set('TAX', tax.gaoRound(precision));
                }
                if (taxId == 3 && taxRTO > 0) {
                    //税外加
                    tax = amt * (taxRTO / 100);
                    tax = Number(tax.gaoRound(precision));
                    var net = Number((amt + tax).gaoRound(precision));
                    //rec.set('AMTN', net);
                    rec.set('TAX', tax);
                }
            }

            //是Free赠品
            ///console.log(rec.get('FREE_ID'));
            if (rec.get('FREE_ID') == true || rec.get('FREE_ID') == 'T') {
                rec.set('AMTN', 0);
                rec.set('AMT', 0);
                rec.set('TAX', 0);
            }

            this.controller.SetTotalAmtToHead(rec.store, form2);
        });        
    },
    SetTotalAmtToHead: function (gridStore, form2) {
        var formTax = Ext.ComponentQuery.query('field[name=SHOW_TAX]', form2)[0],
            formShowAmtn = Ext.ComponentQuery.query('field[name=SHOW_AMTN]', form2)[0],
            formShowAmtTotal = Ext.ComponentQuery.query('field[name=SHOW_AMT_TATOL]', form2)[0],
            tax = 0, amtn = 0, amt = 0;

        for (var i = 0; i < gridStore.getCount(); i++) {
            tax += gridStore.getAt(i).get('TAX');
            amtn += gridStore.getAt(i).get('AMTN');
           // amt += gridStore.getAt(i).get('AMT');
        }

        formTax.setValue(tax);
        formShowAmtn.setValue(amtn);
        formShowAmtTotal.setValue(amtn + tax);
    }

});