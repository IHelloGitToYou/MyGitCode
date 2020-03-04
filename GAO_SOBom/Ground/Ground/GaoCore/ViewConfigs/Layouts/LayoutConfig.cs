using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    /// <summary>
    /// 布局的属性集 
    /// </summary>
    public class LayoutConfig
    {
        public LayoutConfig(decimal? ColumnWidth = null, decimal? Flex = null, int? Rowspan = null, int? Colspan = null)
        {
            if(ColumnWidth!=null)
                columnWidth = ColumnWidth.Value;
            if(Flex!=null)
                flex = Flex.Value;
            if (Rowspan != null)
                rowspan = Rowspan.Value;
            if (Colspan != null)
                colspan = Colspan.Value;
        }

        public decimal? columnWidth { get; set; }
        public decimal? flex { get; set; }
        public int? rowspan { get; set; }
        public int? colspan { get; set; }
        
    }
}
