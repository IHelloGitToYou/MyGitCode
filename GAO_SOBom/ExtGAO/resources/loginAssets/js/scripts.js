var ShowErrorMsg = function (msg, title) {
    Ext.toast({
        html: msg,
        title: title || '提示',
        width: 300,
        align: 'tr',
        //autoClose : false
        autoCloseDelay: 10000
    });
}

var findComp=function(){
    var usr = $('#form-username')[0].value,
            pw = $('#form-password')[0].value,
            comp_nos = [];
        
        Ext.Ajax.request({
            url: GroundApiUrl + '/' + 'Sys/MatchCompOnLogin',
            async: false,
            cors: true,
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'X-LOGIN-ID': '637161936203680483'  },//'application/x-www-form-urlencoded; charset=UTF-8',
            params: {},
            jsonData: {
                UserNo: usr,
                Password: pw
            },
            success: function (response, opts) {
                var json = Ext.decode(response.responseText);
                //console.log(json);
                if (json.code == 200) {
                    comp_nos = json.data;
                }
                else {
                    ShowErrorMsg(json.message, '异常');
                }
                
            },
            failure: function (response, opts) {
                // console.log('failure');
                ShowErrorMsg('远程服务端异常', '服务中断');
            }
        });

        //RequestX.RequestGetSync('Sys/MatchCompOnLogin', 
        //    {
        //        UserNo: usr,
        //        Password: pw 
        //    },
        //    function (data2) {
        //        comp_nos = data2;
        //        success = true;
        //    }, this);

        var cb = $(".selectpicker");
        if (comp_nos.length > 0) {
            var cnt = cb[0].options.length;

            var list = [];//"<option value='null'>请选择帐套</option>"
            for (let i = 0; i < comp_nos.length; ++i) {
                var s = "<option value='" + comp_nos[i].CompNo + "'>" + comp_nos[i].Name + "</option>";
                list.push(s);
                //cb[1].append(s);
            }

            $("#parkID").html(list.join(' '));

            //使用refresh方法更新UI以匹配新状态。
            cb.selectpicker('refresh');
            //render方法强制重新渲染引导程序 - 选择ui。
            cb.selectpicker('render');
        }
        else {
            $("#parkID").html('');
            //使用refresh方法更新UI以匹配新状态。
            cb.selectpicker('refresh');
            //render方法强制重新渲染引导程序 - 选择ui。
            cb.selectpicker('render');
        }
}

jQuery(document).ready(function() {
        var task = new Ext.util.DelayedTask(function(){
            var userTxt =$('#form-username')[0].value;
            if(userTxt){
                findComp();
            }
        });
        task.delay(800);

        
	    $(".selectpicker").selectpicker({
            noneSelectedText: '请选择帐套'//默认显示内容
        });
    /*
        Fullscreen background1223456 
    */
    $.backstretch("resources/loginAssets/img/backgrounds/1.jpg");
    
    /*
        Form validation
    */
    $('.login-form input[type="text"], .login-form input[type="password"], .login-form textarea').on('focus', function() {
    	$(this).removeClass('input-error');
    });

    $('.login-form input[type="text"], .login-form input[type="password"], .login-form textarea').on("input", function (event) {
        findComp();
    });
    
    $('#SubmitBtn').on('click', function(e) {
        var usr = $('#form-username')[0].value,
            pw = $('#form-password')[0].value,
            db = $('#parkID')[0].value;

        if (!db) {
            return;
        }


        Ext.Ajax.request({
            url: GroundApiUrl + '/' + 'Sys/Login',
            async: false,
            cors: true,
            method: 'POST',
            headers: { 'Content-Type': 'application/json', 'X-LOGIN-ID': '637161936203680483' },//'application/x-www-form-urlencoded; charset=UTF-8',
            params: {},
            jsonData: {
                UserNo: usr,
                Password: pw,
                DB: db
            },
            success: function (response, opts) {
                var json = Ext.decode(response.responseText);
                //console.log(json);
                if (json.code == 200) {
                    var url = 'Index.html?X-LOGIN-ID=' + json.data;
                    window.open(url, '_self');
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

     //  // console.log();
    	//$(this).find('input[type="text"], input[type="password"], textarea').each(function(){
    	//	if( $(this).val() == "" ) {
    	//		e.preventDefault();
    	//		$(this).addClass('input-error');
    	//	}
    	//	else {
    	//		$(this).removeClass('input-error');
    	//	}
    	//});


    });
    
    
});
