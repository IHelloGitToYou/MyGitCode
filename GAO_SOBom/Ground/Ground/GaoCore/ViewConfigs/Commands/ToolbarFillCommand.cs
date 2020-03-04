using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class ToolbarFillCommand : ExtjsViewCommand
    {
        public ToolbarFillCommand()
        {
            this.Text = "->";
            this.CommandPath = "Ext.toolbar.Fill";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
