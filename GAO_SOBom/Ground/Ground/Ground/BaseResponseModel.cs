using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ground
{
    /// <summary>
    /// 接口响应数据的基本模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponseModel<T>
    {
        /**
         {
             "code":200, //成功编号统一为200,其它错误码404, 500参照http
             "data": null, //可以是object, 也可以是数组
             "message": "SUCCESS" //错误信息
         } 
         */
        public bool success { get; set; } = true;

        public int code { get; set; }

        public T data { get; set; }

        public string message { get; set; }

        public BaseResponseModel() { }

        public BaseResponseModel(int code, T data, string msg)
        {
            if (code != 200)
                success = false;

            this.code = code;
            this.data = data;
            this.message = msg;
        }
    }

    public class BaseResponseModel : BaseResponseModel<object>
    {
        public BaseResponseModel(int code, object data, string msg) : base(code, data, msg)
        {
        }

        public readonly static BaseResponseModel NotFound404 = new BaseResponseModel(404, null, "");
    }
}
