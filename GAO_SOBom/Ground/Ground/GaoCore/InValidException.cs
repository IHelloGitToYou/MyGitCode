using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    public class InValidException :Exception
    {
        public InValidException() : base()
        {

        }

        public InValidException(string Msg) : base(Msg)
        {

        }
    }
}
