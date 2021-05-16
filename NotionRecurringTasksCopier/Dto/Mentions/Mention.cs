using Newtonsoft.Json;
using NotionRecurringTasksCopier.Converters;

namespace NotionRecurringTasksCopier.Dto.Mentions
{
    [JsonConverter(typeof(MentionConverter))]
    internal abstract class Mention
    {
        public enum TypeEnum
        {
            Page
        }

        [JsonProperty]
        public abstract TypeEnum Type { get; }
    }
}