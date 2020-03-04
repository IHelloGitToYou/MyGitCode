using GaoCore;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System;
using System.Collections.Generic;

namespace Ground.Controllers
{
    public class BaseController: Controller
    {
        public static Dictionary<string, LoginInfo> NowLogins = new Dictionary<string, LoginInfo>();


        public string Get_X_LOGIN_ID(bool NoFindToError = true)
        {
            if (Request.Headers.ContainsKey("X-LOGIN-ID"))
                return Request.Headers["X-LOGIN-ID"].ToString();
            else if (Request.Query.ContainsKey("X-LOGIN-ID"))
                return Request.Query["X-LOGIN-ID"].ToString();

            if (NoFindToError == false)
                return "";

            throw new System.Exception("请先登录");
        }


        /// <summary>
        /// 登录帐套的数据库
        /// </summary>
        /// <returns></returns>
        public string GetDB_ON_X_LOGIN_ID()
        {
            var session = Request.HttpContext.Session;//.SetString("X-LOGIN-ID", "Yao");

            var logId = Get_X_LOGIN_ID(true);
            if (logId == "yaochao") //后台
                return "DEMO";

            if (NowLogins.ContainsKey(logId) == false)
                throw new InvalidCastException("请先登录");

            return NowLogins[logId].DB;
        }


        public LoginInfo GetLoginUser() {
            var logId = Get_X_LOGIN_ID(true);
            if (logId == "yaochao")
            {//后台
                return new LoginInfo()
                {
                    UserNo = "ADMIN",
                    DB = "DEMO"
                };
            }

            if (NowLogins.ContainsKey(logId) == false)
                throw new InvalidCastException("请先登录");

            return NowLogins[logId];
        }

        
    }
}
