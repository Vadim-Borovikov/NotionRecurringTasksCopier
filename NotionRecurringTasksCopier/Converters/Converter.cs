using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NotionRecurringTasksCopier.Converters
{
    internal abstract class Converter<T, TEnum> : JsonConverter
        where TEnum : struct
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(T);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObject = serializer.Deserialize<JObject>(reader);
            string type = jObject["type"].Value<string>();
            TEnum? parsedType = ParseType(type);
            if (!parsedType.HasValue)
            {
                throw new Exception($"Can't parse type {type}!");
            }
            return Deserialize(parsedType.Value, jObject);
        }

        protected abstract T Deserialize(TEnum value, JObject jObject);

        private static TEnum? ParseType(string type)
        {
            bool parsed = Enum.TryParse(type.SnakeToPascal(), out TEnum p);
            if (parsed)
            {
                return p;
            }

            return null;
        }

        protected static TConcrente Deserialize<TConcrente>(JObject jObject) where TConcrente : T
        {
            return JsonConvert.DeserializeObject<TConcrente>(jObject.ToString(), NotionProvider.Settings);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // won't be called because CanWrite returns false
            throw new NotImplementedException();
        }
    }
}