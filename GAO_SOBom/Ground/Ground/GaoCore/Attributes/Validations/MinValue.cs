using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [AttributeUsage(
          AttributeTargets.Property,
          AllowMultiple = false,
          Inherited = false)]
    public class MinValueAttribute : Attribute
    {
        public object MinValue { get; set; }

        public MinValueAttribute(int Year, int Mouth, int Day)
        {
            this.MinValue = new DateTime(Year, Mouth, Day);
        }

        public MinValueAttribute(object MinValue)
        {
            this.MinValue = MinValue;
        }
    }
}
