using Newtonsoft.Json;
using NotionRecurringTasksCopier.Converters;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    [JsonConverter(typeof(PropertyConverter))]
    internal abstract class Property
    {
        public enum TypeEnum
        {
            Text,
            MultiSelect,
            Date,
            Relation,
            Title,
            Checkbox
        }

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public abstract TypeEnum Type { get; }
    }
}