using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAOWebAPI.Models
{
    public class BomConflictException:BaseException
    {
        public BomConflictException(string msg) : base(msg)
        {

        }
    }
}
