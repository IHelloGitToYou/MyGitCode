using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Gao.Models;
using Gao.Models.Users;
using Gao.Services.Users;
using GaoCore;
using GaoCore.Extjs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ground.Controllers
{
    [Route("api/[controller]/[action]/{id?}")]
    public class SysController : BaseController
    {
        // GET api/sys
        [HttpGet]
        public EntityViewConfig GetView(string EntityType, string ViewGroup)
        {
            var entityViewConfig = ExtjsBuilder.GenerateExtjsView(EntityType, ViewGroup);
            return entityViewConfig;
           // string configs = JsonConvert.SerializeObject(viewConfig);
        }

        public class ApiRequest_GetCommandClick{
            public string Command { get; set; }
            public string PostData { get; set; }
        }


        [HttpPost]
        public object GetCommandClick(ApiRequest_GetCommandClick Request)
        {
            var command = ExtjsBuilder.FindViewCommand(Request.Command);
            if (command == null)
                throw new InValidException("命令[{0}]不存在".FormatOrg(Request.Command));
            command.CurrentXLoginDB = GetDB_ON_X_LOGIN_ID();
            command.LoginUser = GetLoginUser();
            var result = command.Execute(Request.PostData);

            return result;
        }


        #region Grid 查询数据统一Api

        [HttpPost]
        public ApiQueryDataResponse GetQueryData(ApiQueryDataRequest Request)
        {
            var qe = ExtjsBuilder.FindQueryEntity(Request.QueryEntity);
            var qe2 = (QueryEntity)JsonConvert.DeserializeObject(Request.Data, qe.GetType());
            qe2.page = Request.page;
            qe2.limit = Request.limit;
            qe2.CurrentXLoginDB = GetDB_ON_X_LOGIN_ID();

            var data = qe.Fetch(qe2);

            return data;
        }

        #endregion

        [HttpGet]
        public List<Comp> GetComps()
        {
            var db = Entity.GetDb(typeof(Comp));
            return db.Queryable<Comp>().ToList();

            ////LoginInfo Info
            //db.Queryable<Comp>().Where(o => o.UserNo == Info.username
            //    && o.PassWork == UserService.ToPassWordMD5(Info.password)
            //).Count();

            //List<Comp> list;

            //string strWhere = "";
            //if (Info.username.IsNotEmpty())
            //    strWhere += "";
            //SqlSugarBase.GetDB()
            ////if (UserService.ErpType == ErpTypes.T8)
            ////{
            ////    list = SqlSugarBase.DB.SqlQueryable<CompInfo>("select  COMPNO, NAME from TbrSystem.dbo.Comp").ToList();
            ////}
            ////else
            ////{
            ////    list = SqlSugarBase.DB.SqlQueryable<CompInfo>("select  COMPNO, NAME from SunSystem.dbo.Comp").ToList();
            ////}
            ////return list;
        }

        [HttpPost]
        public List<Comp> MatchCompOnLogin([FromBody]LoginInfo loginInfo)
        {
            var db = Entity.GetDb(typeof(Comp));
            var user = db.Queryable<User>().Where(o => o.UserNo == loginInfo.UserNo).First();
            if (user == null)
                return new List<Comp>();

            var list = db.Queryable<UserComp>()
                        .Where(o => o.UserId == user.Id && o.PassWork == UserService.ToPassWordMD5(loginInfo.Password))
                        .ToList();
            if (list.Count == 0)
                return new List<Comp>();

            var list2 = db.Queryable<Comp>().In(list.Select(o => o.CompId)).ToList();
            return list2;
        }

        #region 登录
        
        [HttpPost]
        public string Login([FromBody]LoginInfo loginInfo)
        {
            var comps = MatchCompOnLogin(loginInfo);
            if (comps.Count == 0)
                throw new InvalidCastException("登录失败!");

            string ticks = DateTime.Now.Ticks.ToString();
            lock (NowLogins)
            {
                NowLogins.Add(ticks, loginInfo);
                //注册 帐套的数据库
                SqlSugarBase.Register(loginInfo.DB, UserService.DB_ConnectionStringFormatOnDB.FormatOrg(loginInfo.DB));
            }

            return ticks;
        }


        #endregion

        [HttpGet]
        public object GetUserComps(int UserId)
        {
            var q = RF<UserComp>.GetSqlQueryWithViewProperty();

            var dt = q.Where(o=>o.UserId == UserId).ToDataTable();
            
            return RF.ToDictionary(dt);
        }
        
        [HttpPost]
        public bool SaveUserComps([FromBody]List<UserComp> UserComps)
        {
            if (UserComps.Count == 0)
                throw new InValidException("无需要修改的记录！");

            //List<UserComp> UserComps
            RF<UserComp>.SaveFixDllDB(UserComps, GetLoginUser());

            return true;
        }
        
    }
}
