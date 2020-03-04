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
    public class Propery : System.Attribute
    {
        public string Label { get; private set; }
        public Propery(string P_Label)
        {
            this.Label = P_Label;
        }
    }
}
