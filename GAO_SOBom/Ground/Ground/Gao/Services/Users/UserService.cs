using Gao.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using GaoCore;

namespace Gao.Services.Users
{
    public class UserService
    {
        public static ErpTypes ErpType { get; set; }
        public static string DB_ConnectionStringFormatOnDB { get; set; }
        
        

        public static string ToPassWordMD5(string strs)
        {
            if (strs.IsNullOrEmpty()) return "";

            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(strs);//将要加密的字符串转换为字节数组
            byte[] encryptdata = md5.ComputeHash(bytes);//将字符串加密后也转换为字符数组
            return Convert.ToBase64String(encryptdata);//将加密后的字节数组转换为加密字符串
        }
         
    }
}
