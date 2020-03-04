using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GAOWebAPI.Register;

namespace GAOWebAPI.Services
{
    public class SqlSugarBase
    {
        public SqlSugarBase()
        {
            if (_IsValidClient == false || (_IsValidClient == true && _IsValidClientDate.Value.Date < DateTime.Now.Date))// || (_IsValidClient == null && SqlSugarBase.IsValidClient() == false)
            {
                throw new Exception("未注册,请联系[广东冠奥互联网科技有限公司]");
            }
        }

        #region 序列号是否有权限
        BomRegisterService regHelper;
        protected static bool? _IsValidClient { get; set; } = null;
        protected static DateTime? _IsValidClientDate { get; set; } = null;

        /*private static List<string> _ValidErpNumbers = new List<string>() { "JMTW06","GDGA05", "GDGA01", "GDGA02", "GDGA03" };*/

        [Serializable]
        public class ClientInfo
        {
            public string SerialId { get; set; }
        }

        public static ClientInfo GetClientInfo()
        {
            ClientInfo erpCleint = null;
            if (LoginService.SYS_MODEL == SysModel.T8)
            {
                erpCleint = DB.SqlQueryable<ClientInfo>(@"
                            SELECT 
                                SerialId
                            FROM master.dbo.sysLicData ").ToList()
                            .FirstOrDefault();

                //if (_ValidErpNumbers.Contains(erpCleint.SerialId))
                //    result = true;
            }
            else
            {

            }
            return erpCleint;
        }

        public static bool IsValidClient()
        {
            bool result = false;
           
            _IsValidClient = null;
            return result;
        }

        #endregion

        public static Dictionary<string, LoginInfo> NowLogins = new Dictionary<string, LoginInfo>();
        public static string DB_ConnectionString { get; set; }
        public static string DB_ConnectionStringFormatOnDB { get; set; }
        public static Dictionary<string, SqlSugarClient> CacheDB = new Dictionary<string, SqlSugarClient>();
        
        public static SqlSugarClient DB2(string XLoginId)
        {

//#if DEUBG
//            string db_no ="DEMO";
//            return new SqlSugarClient(new ConnectionConfig()
//                            {
//                                ConnectionString = DB_ConnectionStringFormatOnDB.FormatOrg(db_no),
//                                DbType = SqlSugar.DbType.SqlServer,
//                                IsAutoCloseConnection = true,
//                                InitKeyType = InitKeyType.SystemTable,
//                                IsShardSameThread = true
//                            });
//#endif

 
            if (NowLogins.ContainsKey(XLoginId) == false)
                throw new Exception("请先登录!");
            string db_no = NowLogins[XLoginId].db_no;
            lock (CacheDB)
            {
                if (CacheDB.ContainsKey(db_no) == false)
                {
                    CacheDB.Add(db_no, new SqlSugarClient(new ConnectionConfig()
                    {
                        ConnectionString = DB_ConnectionStringFormatOnDB.FormatOrg(db_no),
                        DbType = SqlSugar.DbType.SqlServer,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.SystemTable,
                        IsShardSameThread = true
                    }));
                }
            }

            return CacheDB[db_no];
        }

        public static SqlSugarClient DB {
           
            get => new SqlSugarClient(new ConnectionConfig() {
                ConnectionString = DB_ConnectionString,
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.SystemTable,
                IsShardSameThread = true
            });
        }


        //public static SqlSugarClient DB
        //{
        //    get => new SqlSugarClient(new ConnectionConfig()
        //    {
        //        ConnectionString = DB_ConnectionStringFormatxxxxxx,
        //        DbType = SqlSugar.DbType.SqlServer,
        //        IsAutoCloseConnection = true,
        //        InitKeyType = InitKeyType.SystemTable,
        //        IsShardSameThread = true
        //    }
        //    );
        //}


        public bool ExeCmds(List<SqlCommand> Cmds, SqlSugarClient P_DB = null)
        {
            SqlSugarClient tempDB = P_DB != null ? P_DB : DB;

            bool result = false;
            try
            {
                tempDB.BeginTran();
                foreach (SqlCommand item in Cmds)
                {
                    tempDB.Ado.ExecuteCommand(item.CommandText, 
                            new SugarParameter("@prd_noxxxxxxxxxxxxxx", ""));
                }
                tempDB.CommitTran();
                result = true;
            }
            catch (Exception ex)
            {
                tempDB.RollbackTran();
                throw ex;
            }

            return result;
        }

    }

   
}
