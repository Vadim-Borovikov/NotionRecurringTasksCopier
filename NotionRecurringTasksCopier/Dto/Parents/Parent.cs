using Newtonsoft.Json;
using NotionRecurringTasksCopier.Converters;

namespace NotionRecurringTasksCopier.Dto.Parents
{
    [JsonConverter(typeof(ParentConverter))]
    internal abstract class Parent
    {
        public enum TypeEnum
        {
            DatabaseId
        }

        [JsonProperty]
        public abstract TypeEnum Type { get; }
    }
}