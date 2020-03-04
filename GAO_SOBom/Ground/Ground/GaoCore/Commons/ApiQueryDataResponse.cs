using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [Serializable]
    public class ApiQueryDataResponse
    {
        public IEnumerable<object> items { get; set; }
        public int total { get; set; }
    }

     
}
