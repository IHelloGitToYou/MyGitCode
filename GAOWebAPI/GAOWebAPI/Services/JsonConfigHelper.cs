using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GAOWebAPI.Services
{
    public class JsonConfigHelper
    {
        public JObject jObject = null;
        public string this[string key]
        {
            get
            {
                string str = "";
                if (jObject != null)
                {
                    str = GetValue(key);
                }
                return str;
            }
        }
        public JsonConfigHelper(string path)
        {
            jObject = new JObject();
            using (System.IO.StreamReader file = System.IO.File.OpenText(path))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    jObject = JObject.Load(reader);
                }
            };
        }

        public T GetValue<T>(string key) where T : class
        {
            return JsonConvert.DeserializeObject<T>(jObject.SelectToken(key).ToString());
        }
        public string GetValue(string key)
        {
            var a = jObject.SelectToken("T8默认值").SelectTokens("BOM表头234234");
            var b = jObject.SelectToken("T8默认值").SelectToken("BOM表头234234");
            var c = b.Children();
            var d = c.ElementAt(0);

            return Regex.Replace((jObject.SelectToken(key).ToString()), @"\s", "");
        }
    }
}
