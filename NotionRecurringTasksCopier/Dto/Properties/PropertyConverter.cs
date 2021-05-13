using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class PropertyConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Property);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            string type = jObject["type"].Value<string>();
            Property.PropertyType? parsedType = ParseType(type);
            if (!parsedType.HasValue)
            {
                throw new Exception($"Can't parse type {type}!");
            }
            switch (parsedType.Value)
            {
                case Property.PropertyType.Text:
                    return Deserialize<RichTextProperty>(jObject);
                case Property.PropertyType.MultiSelect:
                    return Deserialize<MultiSelectProperty>(jObject);
                case Property.PropertyType.Date:
                    return Deserialize<DateProperty>(jObject);
                case Property.PropertyType.Relation:
                    return Deserialize<RelationProperty>(jObject);
                case Property.PropertyType.Title:
                    return Deserialize<TitleProperty>(jObject);
                case Property.PropertyType.Checkbox:
                    return Deserialize<CheckboxProperty>(jObject);
                default:
                    throw new Exception($"Unsupported type {parsedType}!");
            }
        }

        private static Property.PropertyType? ParseType(string type)
        {
            bool parsed = Enum.TryParse(type, true, out Property.PropertyType p);
            if (parsed)
            {
                return p;
            }

            if (type == "multi_select")
            {
                return Property.PropertyType.MultiSelect;
            }

            return null;
        }

        private static T Deserialize<T>(JObject jObject)
        {
            return JsonConvert.DeserializeObject<T>(jObject.ToString(), NotionProvider.Settings);
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // won't be called because CanWrite returns false
            throw new NotImplementedException();
        }
    }
}