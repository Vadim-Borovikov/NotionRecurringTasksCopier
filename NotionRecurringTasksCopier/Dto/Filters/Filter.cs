using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace NotionRecurringTasksCopier.Dto.Filters
{
    internal abstract class Filter
    {
        [JsonProperty]
        public string Property { get; set; }
    }
}