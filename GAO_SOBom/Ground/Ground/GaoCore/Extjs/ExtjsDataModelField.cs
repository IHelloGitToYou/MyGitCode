using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.Extjs
{
    [Serializable]
    public class ExtjsDataModelField
    {
        public ExtjsDataModelField(string Name , string Type)
        {
            name = Name;
            type = Type;
        }

        public ExtjsDataModelField(string Name, string Type, object DefaultValue)
        {
            name = Name;
            type = Type;
            if(DefaultValue != null)
                defaultValue = DefaultValue;
        }

        public string name { get; set; }
        public string type { get; set; }
        public object defaultValue { get; set; } = null;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings() {  
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatString = "yyyy-MM-dd HH:mm:ss"
                });
            //return base.ToString();
            //if(defaultValue == null)
            //    return "{\"name\":\"" + name + "\", \"type\":\"" + type + "\" }";
            //else
            //    return "{\"name\":\"" + name + "\", \"type\":\"" + type + "\" }";
        }
    }
}
