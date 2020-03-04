using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class ExtjsViewCommand 
    {
        public LoginInfo LoginUser { get; set; }
        public string CurrentXLoginDB { get; set; }

        public string IconCls { get; set; }
        public string Text { get; set; }
        public string CommandPath { get; set; }

        public virtual object Execute(string PostData)
        {
            return null;
        }
    }
}
