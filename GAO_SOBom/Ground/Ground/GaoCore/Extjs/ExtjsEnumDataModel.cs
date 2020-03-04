using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.Extjs
{
    [Serializable]
    public class ExtjsEnumDataModel
    {
        public int Id { get; set; }
        public string NAME { get; set; }

        public override string ToString()
        {
            return "{\"Id\":" + Id + ", \"NAME\":\"" + NAME + "\" }";
        }
    }
}
