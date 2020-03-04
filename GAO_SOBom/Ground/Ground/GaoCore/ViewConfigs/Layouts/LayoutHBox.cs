using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class LayoutHBox : Layout
    {
        public string align { get; set; } = "stretch";
        public LayoutHBox()
        {
            type = "hbox";
        }
    }
}
