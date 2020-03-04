using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NPOI;
using NPOI.HPSF;
using NPOI.HSSF;
using System.Data;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Microsoft.AspNetCore.Http.Internal;
using System.Text.RegularExpressions;

namespace GAOWebAPI.Services
{
    public class NpoiExcelHelper
    {
        public static ISheet GetSheet(IFormFile File, string SheetNameOrIndex)
        {
            var stream = File.OpenReadStream();
            var workBook = new XSSFWorkbook(stream);
            if(SheetNameOrIndex.IsNullOrEmpty())
                return workBook.GetSheetAt(0);

            if (SheetNameOrIndex.IsNumber())
            {
                int index = SheetNameOrIndex.GetInt();
                if((index +1) > workBook.NumberOfSheets)
                {
                    throw new Exception("Excel Sheet数量不足");
                }
                return workBook.GetSheetAt(index);
            }
            else
            {
                return workBook.GetSheet(SheetNameOrIndex);
            }
        }

        private static IWorkbook GetWorkBook(string fileName) { 
            if(fileName.ToLower().EndsWith("xlsx"))
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    return new XSSFWorkbook(file);
                }
            }
            else
            {
                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    return new HSSFWorkbook(file);
                }
            }
        }


        public static DataTable Import(string strFileName = "C://NPOI_TEST.xlsx")
        {
            DataTable dt = new DataTable();
            var workBook = GetWorkBook(strFileName); //POIDocument

            var sheet = workBook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            var headerRow = sheet.GetRow(2);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        //Console.WriteLine(Program.CalcBitLetter(null, uint.Parse(Console.ReadLine())).ToArray());

        //private static Stack<char> CalcBitLetter(Stack<char> result, uint number, int bits = 0, int previous = 0, int _base = 26)
        //{
        //    char[] letters = new char[] { 'Z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' };
        //    if (bits.Equals(0)) result = new Stack<char>();
        //    bool Iscomplete = true;
        //    if (number <= 0) result = new Stack<char>(new char[] { '?' });
        //    else
        //    {
        //        var divisor = (int)(number / (Math.Pow(_base, bits + 1)));
        //        var modulus = (int)(number % (Math.Pow(_base, bits + 1))) / (int)Math.Pow(_base, bits);
        //        modulus += previous;
        //        if (modulus > 0) previous = 0;
        //        else
        //        {
        //            if (modulus < 0) modulus = 25;
        //            previous = -1;
        //            divisor += previous;
        //        }
        //        result.Push(letters[modulus]);
        //        if (divisor <= 0) Iscomplete = false;
        //        bits++;
        //        if (Iscomplete) CalcBitLetter(result, number, bits, previous);
        //    }
        //    return result;
        //}

        public static int TaskExcelPostionNumber(string Pos) {
            char[] chars = Pos.ToUpper().ToCharArray();
            List<char> chars2 = new List<char>();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].ToString().IsNumber())
                {
                    chars2.Add(chars[i]);
                }
            }

            return int.Parse(string.Join("", chars2)) -1;
        }
        
        public static int ExcelNameToIndex(string columnName)
        {
            if (!Regex.IsMatch(columnName.ToUpper(), @"[A-Z]+")) { throw new Exception("invalid parameter"); }
            char[] chars = columnName.ToUpper().ToCharArray();
            List<char> chars2 = new List<char>();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].ToString().IsNumber())
                    break;
                chars2.Add(chars[i]);
            }

            int index = 0;
            for (int i = 0; i < chars2.Count; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars2.Count - i - 1);
            }
            return index - 1;
        }


        public static string IndexToExcelName(int index)
        {
            if (index < 0) { throw new Exception("invalid parameter"); }

            List<string> chars = new List<string>();
            do
            {
                if (chars.Count > 0) index--;
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }


        public static string GetCellStringValue(string Postion, ISheet Sheet)
        {
            try
            {
                int cellIndex = NpoiExcelHelper.ExcelNameToIndex(Postion);
                int rowIndex = NpoiExcelHelper.TaskExcelPostionNumber(Postion);
                var row = Sheet.GetRow(rowIndex);
                if (row == null) return "";

                var obj = row.GetCell(cellIndex);
                if (obj == null) return "";

                return obj.ToString();
            }
            catch(Exception ex)
            {
                throw new Exception("读取Sheet:{0} 坐标:{1}失败".FormatOrg(Sheet.SheetName, Postion));
            }
        }

        public static string GetCellStringValue(ICell Cell)
        {
            if (Cell == null)
                return string.Empty;

            if (Cell != null)
            {
                if (Cell.CellType == CellType.Formula || Cell.CellType == CellType.String)
                {
                    return Cell.StringCellValue;
                }
                else if (Cell.CellType == CellType.Numeric)
                {
                    return Cell.NumericCellValue.ToString();
                }
            }

            return Cell.ToString();
        }
    }

}
