using GAOWebAPI.Models;
using GAOWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GAOWebAPI.Controllers
{
    public class BaseController : Controller
    {
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
    }
   

    [Route("api/[controller]/[action]/{id?}")]
    public class UserController : BaseController
    {
        LoginService LService;
        public UserController()
        {
            LService = new LoginService(this);
        }

        [HttpGet]
        public List<EmployeeModel> GetUsers(string usr, string name, string compno)
        {
            return LService.GetUsers(usr, name, compno);
        }

        [HttpGet]
        public List<CompInfo> GetComps()
        {
            return LService.GetComps();
        }
        
        [HttpPost]
        public LoginResult Login([FromBody]LoginInfo LoginInfo)
        {
            LoginResult res = new LoginResult();
            var result = LService.Login(LoginInfo);

            if (result == true)
            {
                if (LoginInfo.password.IsNullOrEmpty())
                {
                    return new LoginResult() { x_login_id = "PASSWORK_EMPTY" };
                }
                lock (SqlSugarBase.NowLogins)
                {
                    string ticks = DateTime.Now.Ticks.ToString();
                    SqlSugarBase.NowLogins.Add(ticks, LoginInfo);
                    res.x_login_id = ticks;
                }
                return res;
            }
            else
            {
                throw new Exception("用户或密码错误");
            }

            res.token = "";
            return res;
        }

        [HttpPost]
        public bool ChangePassWork([FromBody]ChangePassWork_PostInfo passwork)
        {
            return LService.ChangePassWork(passwork);
        }
        

        [HttpGet]
        public VueUserInfo info(string token)
        {
            var userInfo = new VueUserInfo
            {
                roles = new List<string>() { "admin" },  // "['admin']",
                introduction = "I am a super administrator",
                avatar = "https://wpimg.wallstcn.com/f778738c-e4f8-4870-b634-56703b4acafe.gif",
                name = "Super Admin"
            };

            return userInfo;
        }
        
        [HttpPost]
        public bool logout()
        {
            var a = Request.Cookies;
            lock (SqlSugarBase.NowLogins)
            {
                var logId = Get_X_LOGIN_ID(false);
                if (logId != null && SqlSugarBase.NowLogins.ContainsKey(logId))
                {
                    SqlSugarBase.NowLogins.Remove(logId);
                }
            }

            return true;
        }

    }

}

