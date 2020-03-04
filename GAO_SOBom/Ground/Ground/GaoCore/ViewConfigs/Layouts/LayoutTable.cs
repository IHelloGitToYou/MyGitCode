using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class LayoutTable : Layout
    {
        public int columns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="P_Columns">列数</param>
        public LayoutTable(int P_Columns )
        {
            type = "table";
            columns = P_Columns;
        }

        //public override string ToString()
        //{
        //    return ToConfigString();
        //}

        //public string ToConfigString()
        //{
        //    return "{\'type\':'table', \'columns\':" + Columns + "}";
        //}
    }
}
