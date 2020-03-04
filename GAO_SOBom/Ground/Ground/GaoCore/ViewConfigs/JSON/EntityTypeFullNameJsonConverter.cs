using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GaoCore.ViewConfigs.JSON
{
    /// <summary>
    /// 实体类型转成 命名空间
    /// </summary>
    ///   
    public class EntityTypeFullNameJsonConverter : JsonConverter
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
            Type entityType = (Type)value;
            writer.WriteValue( entityType.FullName);
        }
    }
}
