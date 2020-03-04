using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore
{
    [Serializable]
    public class ApiQueryDataRequest
    {
        public string QueryEntity { get; set; }
        public string Data { get; set; }
        public int? page { get; set; }
        public int? limit { get; set; }
    }

}
