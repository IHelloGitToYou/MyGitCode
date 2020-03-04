using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class QueryViewSearchCommand : ExtjsViewCommand
    {
        public static string CommonCommandPath = "ExtGAO.view.main.QueryViewSearchCommand";
        public QueryViewSearchCommand()
        {
            this.Text = "查询";
            this.CommandPath = CommonCommandPath;
        }


        public override object Execute(string PostData)
        {

            return "abc";
        }
    }
}
