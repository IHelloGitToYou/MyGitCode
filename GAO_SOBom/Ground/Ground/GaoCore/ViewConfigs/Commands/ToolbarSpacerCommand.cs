using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class ToolbarSpacerCommand : ExtjsViewCommand
    {
        public ToolbarSpacerCommand()
        {
            this.Text = "";
            this.CommandPath = "Ext.toolbar.Spacer";
        }

        public override object Execute(string PostData)
        {
            return "";
        }
    }
}
