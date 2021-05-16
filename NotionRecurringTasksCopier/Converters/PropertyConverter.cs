using System;
using Newtonsoft.Json.Linq;
using NotionRecurringTasksCopier.Dto.Properties;

namespace NotionRecurringTasksCopier.Converters
{
    internal sealed class PropertyConverter : Converter<Property, Property.TypeEnum>
    {
        protected override Property Deserialize(Property.TypeEnum value, JObject jObject)
        {
            switch (value)
            {
                case Property.TypeEnum.Text:
                    return Deserialize<RichTextProperty>(jObject);
                case Property.TypeEnum.MultiSelect:
                    return Deserialize<MultiSelectProperty>(jObject);
                case Property.TypeEnum.Date:
                    return Deserialize<DateProperty>(jObject);
                case Property.TypeEnum.Relation:
                    return Deserialize<RelationProperty>(jObject);
                case Property.TypeEnum.Title:
                    return Deserialize<TitleProperty>(jObject);
                case Property.TypeEnum.Checkbox:
                    return Deserialize<CheckboxProperty>(jObject);
                default:
                    throw new Exception($"Unsupported type {value}!");
            }
        }

        protected override Property.TypeEnum? ParseType(string type)
        {
            switch (type)
            {
                case "multi_select":
                    return Property.TypeEnum.MultiSelect;
                default:
                    return base.ParseType(type);
            }
        }
    }
}