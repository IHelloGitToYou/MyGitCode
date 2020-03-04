using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GaoCore
{
    public static class CommonExtend
    {
        public static string ObjToString(this Object obj)
        {
            if (obj == null) return "";

            return obj.ToString();
        }


        public static string TrimEx(this string s)
        {
            if (s.IsNullOrEmpty()) return "";

            return s.Trim().Replace(" ", "");
        }


        public static string FormatOrg(this string s, params object[] pss)
        {
            return string.Format(s, pss);
        }

        public static bool IsNotEmpty(this string s)
        {
            if (s == null) return false;
            if (string.IsNullOrEmpty(s) == true) return false;

            return s.Length > 0;
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNumberType(Type DataType)
        {
            if (DataType == typeof(int) || DataType == typeof(int?)
                || DataType == typeof(double) || DataType == typeof(double?)
                || DataType == typeof(decimal) || DataType == typeof(decimal?)
                || DataType == typeof(float) || DataType == typeof(float?))
                return true;

            return false;
        }

        public static bool IsDateType(Type DataType)
        {
            if (DataType == typeof(DateTime) || DataType == typeof(DateTime?))
                return true;

            return false;
        }

        public static bool IsNumber(this string s)
        {
            if (s.IsNotEmpty() == false) return false;
            bool result = true;
            try
            {
                double.Parse(s);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static int GetInt(this string s)
        {
            if (s.IsNumber())
            {
                decimal num = 0;
                decimal.TryParse(s, out num);
                return decimal.ToInt32(num);
            }

            throw new Exception("不是数字");
        }

        public static double GetDouble(this Object obj)
        {
            if (obj == null) return 0;
            return GetDouble(obj.ToString());
        }

        public static double GetDouble(this string s)
        {
            if (s.IsNumber())
            {
                decimal num = 0;
                decimal.TryParse(s, out num);
                return decimal.ToDouble(num);
            }

            throw new Exception("不是数字");
        }



        public static DateTime? GetDateOrNull(this Object obj)
        {
            if (obj == null) return null;
            if (obj.ToString().IsNullOrEmpty())
                return null;

            DateTime date = new DateTime();
            DateTime.TryParse(obj.ToString(), out date);

            return date;
        }

        #region Excel
        public static string GetSting(this DataRow Row, string FieldName)
        {
            if (Row.Table.Columns.IndexOf(FieldName) < 0)
                throw new Exception("表格不存在列[{0}]".FormatOrg(FieldName));

            return Row[FieldName].ObjToString();
        }

        /// <summary>
        /// 表字段的值，如表格中无该字段不报错返回“”
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static string GetStingWithTry(this DataRow Row, string FieldName)
        {
            if (Row.Table.Columns.IndexOf(FieldName) < 0)
                return "";

            return Row[FieldName].ObjToString();
        }

        #endregion

        public static string GetDateWeekFirstString(this DateTime Date)
        {
            return GetDateWeekFirst(Date).ToString("yyyy-MM-dd");
        }

        public static string GetDateWeekLastString(this DateTime Date)
        {
            return GetDateWeekLast(Date).ToString("yyyy-MM-dd");
        }

        public static DateTime GetDateWeekFirst(this DateTime Date)
        {
            return Date.AddDays(0 - Convert.ToInt16(Date.DayOfWeek));
        }

        public static DateTime GetDateWeekLast(this DateTime Date)
        {
            return Date.AddDays(6 - Convert.ToInt16(Date.DayOfWeek));
        }

 
        public static DateTime GetDateMouthFirst(this DateTime Date)
        {
            return DateTime.Parse(Date.ToString("yyyy-MM-01"));
        }

        public static DateTime GetDateMouthLast(this DateTime Date)
        {
            return DateTime.Parse(Date.AddMonths(1).ToString("yyyy-MM-01"));
        }


    }
}
