using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    public class BaseException: Exception
    {
        public BaseException(string msg) : base(msg) { }
    }
}
