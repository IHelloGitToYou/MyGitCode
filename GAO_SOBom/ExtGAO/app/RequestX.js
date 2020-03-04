Ext.define('RequestX', {
    singleton: true,
    DefaultHeaders : {},
    BaseGet: function (ApiUrl, Params, JsonData, Method, Async, FnThen, FnScope) {
        return Ext.Ajax.request({
            url: GroundApiUrl + '/' + ApiUrl,
            async: Async,
            cors: true,
            method: Method,
            params: Params,
            jsonData: JsonData,
            headers: RequestX.DefaultHeaders,
            success: function (response, opts) {
                // console.log('success');
                var json = Ext.decode(response.responseText);
                if (json.code == 200) {
                    Ext.callback(FnThen, (FnScope || this), [json.data]);
                }
                else {
                    AppTool.ShowErrorMsg(json.message, '异常');
                }
            },
            failure: function (response, opts) {
                // console.log('failure');
                AppTool.ShowErrorMsg('远程服务端异常', '服务中断');
            }
        });
    },

    ///Get 同步
    RequestSync: function (ApiUrl, Params, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, Params, null, 'GET', false, FnThen, FnScope);
    },
    RequestGetSync: function (ApiUrl, Params, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, Params, null, 'GET', false, FnThen, FnScope);
    },

    //Get 异步
    Request: function (ApiUrl, Params, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, Params, null, 'GET', true, FnThen, FnScope);
    },
    RequestGet: function (ApiUrl, Params, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, Params, null, 'GET', true, FnThen, FnScope);
    },

    RequestPostSync: function (ApiUrl, Params, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, Params, null, 'POST', false, FnThen, FnScope);
    },

    RequestPost: function (ApiUrl, Params, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, Params, null, 'POST', true, FnThen, FnScope);
    },

    RequestPostSyncJsonData : function (ApiUrl, JsonData, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, null, JsonData, 'POST', true, FnThen, FnScope);
    },

    RequestPostJsonData : function (ApiUrl, JsonData, FnThen, FnScope) {
        return RequestX.BaseGet(ApiUrl, null, JsonData, 'POST', true, FnThen, FnScope);
    }
});
