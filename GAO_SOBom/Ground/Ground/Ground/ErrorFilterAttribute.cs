using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Ground
{
    /// <summary>
    /// 全局错误处理
    /// </summary>
    public class ErrorFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            //这里后面加一个日志记录,保存错误信息
            context.Result = new ObjectResult(new BaseResponseModel<object>(500, null, context.Exception.Message));
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = new ObjectResult(new BaseResponseModel<object>(500, null, context.Exception.Message));
            return base.OnExceptionAsync(context);
        }
    }
}
