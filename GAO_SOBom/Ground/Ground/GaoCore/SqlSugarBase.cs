using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GaoCore
{
    public class GaoSqlSugarClient : SqlSugarClient
    {
        public LoginInfo LoginInfo { get; private set; }
        public GaoSqlSugarClient(ConnectionConfig config) : base(config){}

        public void SetLoginInfo(LoginInfo v_LoginInfo)
        {
            LoginInfo = v_LoginInfo;
        }
    }

    public class SqlSugarBase
    {
        private static Dictionary<string, string> DB_ConnectionStrings = new Dictionary<string, string>();
        public static void Register(string DbName, string DbConnString)
        {
            lock (DB_ConnectionStrings)
            {
                if (DB_ConnectionStrings.ContainsKey(DbName) == false)
                    DB_ConnectionStrings.Add(DbName, DbConnString);
            }
        }

        //public static GaoSqlSugarClient DB
        //{
        //    get => new GaoSqlSugarClient(new ConnectionConfig()
        //    {
        //        ConnectionString = DB_ConnectionStrings.First().Value,
        //        DbType = SqlSugar.DbType.SqlServer,
        //        IsAutoCloseConnection = true,
        //        InitKeyType = InitKeyType.Attribute,
        //        IsShardSameThread = true,
        //    });
        //}

        public static GaoSqlSugarClient GetDB(string DbName)
        {
            var db = GetDB(DbName, null);
            return db;
        }

        public static GaoSqlSugarClient GetDB(LoginInfo LoginUser)
        {
            var db = GetDB(LoginUser.DB, LoginUser);
            return db;
        }



        public static GaoSqlSugarClient GetDB(string DbName, LoginInfo LoginUser)
        {
            if (DB_ConnectionStrings.ContainsKey(DbName) == false)
                throw new InValidException("数据库[{0}]不存在".FormatOrg(DbName));

            var db = new GaoSqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = DB_ConnectionStrings[DbName],
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                IsShardSameThread = true
            });

            db.SetLoginInfo(LoginUser);
            return db;
        }


    }
}
