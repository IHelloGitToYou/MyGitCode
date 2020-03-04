using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GaoCore
{
    [RootEntity]
    public class QueryEntity: Entity
    {
        public string CurrentXLoginDB { get; set; }
        public int? page { get; set; }
        //public int? start { get; set; }
        public int? limit { get; set; }

        public virtual ApiQueryDataResponse Fetch(QueryEntity vthis)
        {
            return null;
        }
    }
}
