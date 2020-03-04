using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI
{
    public class GlobalFilterAttribute : ActionFilterAttribute
    {
        //表示是否检查登录
        public bool IsCheck { get; set; }

        public bool IsIngroLoginUrl(string url)
        {
            string lowerUrl = url.ToLower();
            if (lowerUrl.Contains("user/getusers"))
                return true;
            if (lowerUrl.Contains("user/login"))
                return true;
            if (lowerUrl.Contains("user/info"))
                return true;
            if (lowerUrl.Contains("user/changepasswork"))
                return true;
            if (lowerUrl.Contains("user/getcomps"))
                return true;
            if (lowerUrl.Contains("user/logout"))
                return true;
            if (lowerUrl.Contains("bomregister/get_isvalidclient"))
                return true;
            if (lowerUrl.Contains("bomregister/trytoregister"))
                return true;
            

            return false;
        }
         
        //Action方法执行之前执行此方法
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var Request = filterContext.HttpContext.Request;
            string url = Request.Path.ToString();
            if (IsIngroLoginUrl(url) == false)
            {
                string loginId = "";
                if (Request.Headers.ContainsKey("X-LOGIN-ID"))
                    loginId = Request.Headers["X-LOGIN-ID"].ToString();
                else if (Request.Query.ContainsKey("X-LOGIN-ID"))
                    loginId = Request.Query["X-LOGIN-ID"].ToString();
                if (loginId.IsNullOrEmpty())
                    throw new System.Exception("请先登录");

                Request.HttpContext.Session.SetString("X-LOGIN-ID", loginId);
            }

            if (IsCheck)
            {
                if (true)
                {
                    base.OnActionExecuting(filterContext);
                }
                else
                {
                    //仅有这句话跳转时登陆界面会显示在iframe框架中
                    filterContext.HttpContext.Response.Redirect("/Login/Index");
                }
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }


        public override void OnResultExecuting(ResultExecutingContext context)
        {
            //根据实际需求进行具体实现
            if (context.Result is ObjectResult objectResult)
            {
                //if (objectResult.Value == null)
                //{
                //    context.Result = new ObjectResult(BaseResponseModel.NotFound404);
                //}
                //else
                //{
                context.Result = new ObjectResult(new BaseResponseModel<object>(200, objectResult.Value, ""));
                //}
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(BaseResponseModel.NotFound404);
            }
            else if (context.Result is ContentResult contentResult)
            {
                context.Result = new ObjectResult(new BaseResponseModel(contentResult.StatusCode.Value, null, contentResult.Content));
            }
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                context.Result = new ObjectResult(new BaseResponseModel(statusCodeResult.StatusCode, null, string.Empty));
            }
        }

    }

}
