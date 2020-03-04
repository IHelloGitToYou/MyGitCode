using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    public class StructureException : Exception
    {
        public StructureException() : base()
        {
        }

        public StructureException(string Msg) : base(Msg)
        {
        }
    }
}
