using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class ToolbarSeparatorCommand : ExtjsViewCommand
    {
        public ToolbarSeparatorCommand()
        {
            this.Text = "-";
            this.CommandPath = "Ext.toolbar.Separator";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
