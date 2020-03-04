using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [AttributeUsage(
          AttributeTargets.Property,
          AllowMultiple = false,
          Inherited = false)]
    public class MaxValueAttribute : Attribute
    {
        public object MaxValue { get; set; }

        public MaxValueAttribute(object MinValue)
        {
            this.MaxValue = MinValue;
        }
    }
}
