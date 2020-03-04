using GaoCore.ViewConfigs.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gao
{
    public class QueryViewSearchCommand : ExtjsViewCommand
    {
        public QueryViewSearchCommand()
        {
            this.Text = "查询";
            this.CommandPath = "ExtGAO.view.main.QueryViewSearchCommand";
        }


        public override object Execute(string PostData)
        {

            return "abc";
        }
    }
}
