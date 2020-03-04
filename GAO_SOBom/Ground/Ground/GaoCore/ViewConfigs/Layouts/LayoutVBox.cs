using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class LayoutVBox : Layout
    {
        public string align { get; set; } = "stretch";
        public LayoutVBox()
        {
            type = "vbox";
        }
    }
}
