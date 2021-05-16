using System;
using Newtonsoft.Json.Linq;
using NotionRecurringTasksCopier.Dto.Parents;

namespace NotionRecurringTasksCopier.Converters
{
    internal sealed class ParentConverter : Converter<Parent, Parent.TypeEnum>
    {
        protected override Parent Deserialize(Parent.TypeEnum value, JObject jObject)
        {
            switch (value)
            {
                case Parent.TypeEnum.DatabaseId:
                    return Deserialize<DatabaseParent>(jObject);
                default:
                    throw new Exception($"Unsupported type {value}!");
            }
        }
    }
}