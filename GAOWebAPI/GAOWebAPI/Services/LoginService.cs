using GAOWebAPI.Controllers;
using GAOWebAPI.Models;
using GAOWebAPI.Register;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Services
{
    public class LoginService : SqlSugarBase
    {
        private BaseController _BaseController;
        public LoginService(BaseController baseController)
        {
            _BaseController = baseController;
            
            /// 如果之前已经有注册过，直接判断注册信息是否有效？
            if (_IsValidClient == null && DB.Queryable<RegisterInfoModel>().First() != null)
            {
                new Register.BomRegisterService(baseController).TryGet(DateTime.Now);
            }

        }

        //"ERP系统序列号:": "GAD12012",
        //"ERP系统": "T8", //或 "SUNLIKE"
        //public static string SYS_NUMBER;
        public static SysModel SYS_MODEL;
        
        public List<EmployeeModel> GetUsers(string usr, string name, string compno)
        {
            var q = DB.Queryable<EmployeeModel>();//.ToList();
            if (usr.IsNotEmpty())
            {
                q.Where(o => o.usr.Contains(usr));
            }
            if (name.IsNotEmpty())
            {
                q.Where(o => o.name.Contains(name));
            }
            if (compno.IsNotEmpty())
            {
                q.Where(o => o.compno == compno);
            }
            
            return q.Distinct().ToList();
        }



        public List<CompInfo> GetComps()
        {
           
            List<CompInfo> list;
            if (SYS_MODEL == SysModel.T8)
            {
                list = DB.SqlQueryable<CompInfo>("select  COMPNO, NAME from TbrSystem.dbo.Comp").ToList();
            }
            else
            {
                list = new List<CompInfo>();
            }
            return list;
        }

        public bool Login(LoginInfo LoginInfo)
        {
            bool result = false;
            if (SYS_MODEL == SysModel.T8)
            {
                string f1 = @"SELECT TOP 1 
	                                compno,
	                                usr,
	                                name
                                FROM TbrSystem.dbo.PSWD 
                                Where compno='{0}' and UPPER(usr)=UPPER('{1}') and PWD_GAO='{2}' ";

                var list = DB.SqlQueryable<EmployeeModel>(f1.FormatOrg(LoginInfo.db_no, 
                    LoginInfo.username, ToPassWordMD5(LoginInfo.password))).ToList();
                result = list.Count > 0;
            }
            else
            {
                result = false;
            }

            return result;
        }

        private string ToPassWordMD5(string strs)
        {
            if (strs.IsNullOrEmpty()) return "";

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strs);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }


        public bool ChangePassWork(ChangePassWork_PostInfo passwork)
        {
            if(Login(new LoginInfo() { db_no = 
                                    passwork.db_no, 
                                    username = passwork.username,
                                    password = ToPassWordMD5(passwork.oldPassWork) }) == false)
            {
                throw new Exception("原密码不对!");
            }

            string f1 = @"Update TbrSystem.dbo.PSWD set PWD_GAO='{2}'
                                Where compno='{0}' and UPPER(usr)=UPPER('{1}')";
            int count = DB.Ado.ExecuteCommand(
                    f1.FormatOrg(passwork.db_no,
                                passwork.username, 
                                ToPassWordMD5(passwork.newPassWork)));
            
            if(count == 0)
                throw new Exception("用户或帐套不存在!");

            return true;
        }
    }

    public enum SysModel
    {
        T8,
        SUNLIKE
    }


    [Serializable]
    public class CompInfo
    {
        public string compno { get; set; }
        public string name { get; set; }
    }

    [Serializable]
    public class ChangePassWork_PostInfo
    {
        public string db_no { get; set; }
        public string username { get; set; }
        public string oldPassWork { get; set; }
        public string newPassWork { get; set; }
    }

    //[Serializable]
    //public class UserInfo
    //{
    //    public string usr { get; set; }
    //    public string name { get; set; }
    //}


    [Serializable]
    public class LoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public string db_no { get; set; }
    }

    [Serializable]
    public class LoginResult
    {
        public string token { get; set; } = "admin-token";
        public string x_login_id { get; set; }
    }

    [Serializable]
    public class VueUserInfo
    {
        public List<string> roles;
        public string introduction { get; set; }
        public string avatar { get; set; }
        public string name { get; set; }
    }

    //[SugarTable("GAO_ExcelFormat_Header")]
    //public class ExcelFormatHeader
    //{
    //    [SugarColumn(ColumnDescription = "自增Id")]
    //    public int Id { get; set; }

    //    [SugarColumn(ColumnName = "Format_No", ColumnDescription = "模版编码")]
    //    public string FormatNo { get; set; } //"标准样式"
    //}
}