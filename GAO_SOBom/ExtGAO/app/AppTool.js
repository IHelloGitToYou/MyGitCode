Ext.define('AppTool', {
    singleton: true,
    //显示成功类的 Toast
    ShowOKMsg: function (msg, title) {
        Ext.toast({
            html: msg,
            title: title || '提示',
            width: 300,
            align: 'tr'
        });
    },
    ShowErrorMsg: function (msg, title) {
        Ext.toast({
            html: msg,
            title: title || '提示',
            width: 300,
            align: 'tr',
            //autoClose : false
            autoCloseDelay: 10000
        });
    },
    //找到 查询 按钮 
    FindSearchCommand: function (view) {
        var searchForm = view.searchForm || view.up('panel').searchForm;
        if (searchForm) {
            let searchBtns = searchForm.getDockedItems('toolbar[dock="top"]')[0].items;

            for (let i = 0; len = searchBtns.getCount(), i < len; i++) {
                if (searchBtns.getAt(i).xclass == 'ExtGAO.view.main.QueryViewSearchCommand') {
                    return searchBtns.getAt(i);
                }
            }
        }

        return null;
    },
    ///取Store变化过的记录
    GetChangeRecords: function (store) {
        var recs = [],
            recIds = {};

        var listNews = store.getNewRecords();
        var listModify = store.getModifiedRecords();
        var listRemoved = store.getRemovedRecords();

        //console.log(listNews);
        ///console.log(listModify);
        //console.log(listRemoved);
        var id = -1;
        for (var i = 0; i < listNews.length; i++) {
            id = listNews[i].get('Id');
            listNews[i].set('PersistStatus', 0);

            recIds[id] = true;
            recs.push(listNews[i]);
        }
        for (var i = 0; i < listModify.length; i++) {
            id = listModify[i].get('Id');
            if (recIds[id])
                continue;
            if (id < 0)
                listModify[i].set('PersistStatus', 0);
            else
                listModify[i].set('PersistStatus', 1);
            recs.push(listModify[i]);
        }
        for (var i = 0; i < listRemoved.length; i++) {
            id = listRemoved[i].get('Id');
            if (id < 0)
                continue;

            listRemoved[i].set('PersistStatus', 3);
            recs.push(listRemoved[i]);
        }

        return recs;
    },

    GetDateString: function (date) {
        date = date || new Date();
        return Ext.util.Format.date(date, 'Y-m-d') + ' 00:00:00';
    }
});

//AppTool = {

//}

