using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [AttributeUsage(
           AttributeTargets.Class,
           AllowMultiple = false,
           Inherited = false)]
    public class RootEntity : System.Attribute
    {

    }
}
