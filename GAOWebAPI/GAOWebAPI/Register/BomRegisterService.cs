using GAOWebAPI.Controllers;
using GAOWebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAOWebAPI.Register
{
    /////// 24 yDix1U3x4B4iX79+4WvO0g==  25  "xKJsUHoqUIxBYK+5rYj53g=="  26 LDVaPzJk5Ew8tv7PsdDbcw==
    //BomRegisterHelper helper = new BomRegisterHelper();
    //bool result = helper.Register("GDDA05", new DateTime(2019, 12, 24), "yDix1U3x4B4iX79+4WvO0g==", DateTime.Now);
    //var info = helper.Get();
    //helper.Register("GDDA05", new DateTime(2019, 12, 26), "LDVaPzJk5Ew8tv7PsdDbcw==", DateTime.Now);
    //        var info2 = helper.Get();

    public class BomRegisterService : SqlSugarBase
    {
        private BaseController _BaseController;
        public BomRegisterService(BaseController baseController)
        {
            _BaseController = baseController;
        }

        RegisterInfoModel now;
        public bool Register(string SystemNo, DateTime ValidDate, string MD5Onlie, DateTime AskClientDate)
        {
            //担心修改了服务器的时间
            if (AskClientDate.Date != DateTime.Now.Date)
                return false;

            var oldReg = Get(AskClientDate);
            //相同的
            if (oldReg != null && MD5Onlie == oldReg.md5)
            {
                return true;
            }

            string md5New = CalcMD5(SystemNo, ValidDate);
            if (md5New != MD5Onlie)
                return false;
            if (oldReg == null)
            {
                Save(SystemNo, ValidDate, md5New);
                return true;
            }

            if (oldReg.valid_date != null && oldReg.valid_date.Date != ValidDate.Date)
            {
                Save(SystemNo, ValidDate, md5New);
                return true;
            }

            return false;
        }

        private string CalcMD5(string SystemNo, DateTime ValidDate)
        {
            //NO + Date1 + 4位Year + No[1,3,最后一位] = MD5_C 
            string str = SystemNo + ValidDate.ToString("yyyy-MM-dd") + ValidDate.ToString("yyyy")
                            + SystemNo[1].ToString() + SystemNo[3].ToString() + SystemNo[SystemNo.Length - 1].ToString();
            string md5 = ToMD5(str);

            return md5;
        }

        private string ToMD5(string strs)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = Encoding.Default.GetBytes(strs);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }


        public bool? Get_IsValidClient()
        {
            return SqlSugarBase._IsValidClient;
        }


        public RegisterInfoModel TryGet(DateTime AskClientDate)
        {
            if(SqlSugarBase._IsValidClient != null)
            {
                if (SqlSugarBase._IsValidClient == false)
                    return null;
            }

            var now = Get(AskClientDate);
            if (now != null)
            {
                SqlSugarBase._IsValidClient = true;
                SqlSugarBase._IsValidClientDate = now.valid_date;
            }
            return now;
        }

        public RegisterInfoModel Get(DateTime AskClientDate)
        {  
            //担心修改了服务器的时间
            if (AskClientDate.Date != DateTime.Now.Date)
                return null;

            var now = DB.Queryable<RegisterInfoModel>().First();
            if (now == null) return null;
            else
            {
                if (now.valid_date < DateTime.Now.Date)
                    return null;

                if (CalcMD5(now.system_no, now.valid_date) != now.md5)
                    return null;
                
                return now;
            }
        }

        public void Save(string SystemNo, DateTime ValidDate, string MD5Onlie)
        {
            now = new RegisterInfoModel();
            now.system_no = SystemNo;
            now.valid_date = ValidDate;
            now.md5 = MD5Onlie;

            DB.Deleteable<RegisterInfoModel>().ExecuteCommand();
            DB.Insertable<RegisterInfoModel>(now).ExecuteCommand();
        }

        public bool IsValidClient(DateTime AskClientDate)
        {
            bool result = false;
            ClientInfo cInfo = GetClientInfo();
            if (cInfo == null)
                return false;

            RegisterInfoModel rInfo = Get(AskClientDate);
            if (rInfo == null)
                result = false;


            _IsValidClient = result;
            _IsValidClientDate = rInfo.valid_date;
            return result;
        }

    }

    //public class RegisterInfo
    //{
    //    public string SystemNo { get; set; }
    //    public DateTime ValidDate { get; set; }

    //    public string MD5 { get; set; }
    //}
}
