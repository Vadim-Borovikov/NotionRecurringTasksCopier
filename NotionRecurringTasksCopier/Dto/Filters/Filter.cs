using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Filters
{
    internal abstract class Filter
    {
        [JsonProperty]
        public string Property { get; set; }
    }
}