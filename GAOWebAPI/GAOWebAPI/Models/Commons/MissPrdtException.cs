using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    public class MissPrdtException:BaseException
    {
        public MissPrdtException(string msg) : base(msg)
        {

        }
    }
}
