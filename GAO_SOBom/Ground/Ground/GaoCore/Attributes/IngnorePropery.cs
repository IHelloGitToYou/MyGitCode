using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [AttributeUsage(
        AttributeTargets.Field |
        AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public class IngnorePropery : System.Attribute
    {
        public string Label { get; private set; }
        public IngnorePropery(string P_Label)
        {
            this.Label = P_Label;
        }
    }


}
