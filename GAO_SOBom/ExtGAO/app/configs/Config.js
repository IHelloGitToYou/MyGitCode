
//后台API网址
GroundApiUrl = "http://localhost:50465/api";

Gao.AllTable = {
    DepNoForm: 'SALM'  // LOGINER 单据部门取值 业务员,或 当前登录人
}

//销售订单号生成格式
Gao.SOTable = {
    TableFormat: "SOYMDDNNNN"
}


//金额,单价的小数点
GAOPrecisionNumber = {
    AMTN: 2
};




///////////////////////////////////////////////////////////////
Ext.define('ExtGAO.configs.Config', {
    singleton: true,
    Init: function () {

    }
});