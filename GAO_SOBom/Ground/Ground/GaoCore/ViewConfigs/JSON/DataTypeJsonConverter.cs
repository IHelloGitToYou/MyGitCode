using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs
{
    public class DataTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Type fieldType = (Type)value;
            if (fieldType == typeof(String))
            {
                writer.WriteValue("string");
            }
            else if (fieldType == typeof(int) || fieldType == typeof(int?))
            {
                writer.WriteValue("int");
            }
            else if (fieldType == typeof(double) || fieldType == typeof(decimal)
                    || fieldType == typeof(double?) || fieldType == typeof(decimal?))
            {
                writer.WriteValue("number");
            }
            else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
            {
                writer.WriteValue("date");
            }
            else if (fieldType == typeof(Boolean) || fieldType == typeof(Boolean?))
            {
                writer.WriteValue("boolean");
            }
        }
    }
}
