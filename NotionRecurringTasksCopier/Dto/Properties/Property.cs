using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace NotionRecurringTasksCopier.Dto.Properties
{
    [JsonConverter(typeof(PropertyConverter))]
    internal abstract class Property
    {
        public enum PropertyType
        {
            Text,
            // Number,
            // Select,
            MultiSelect,
            Date,
            // Formula,
            Relation,
            // Rollup,
            Title,
            // People,
            // Files,
            Checkbox,
            // PhoneNumber,
            // CreatedTime,
            // CreatedBy,
            // LastEditedTime,
            // LastEditedBy
        }

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public PropertyType Type { get; set; }
    }
}