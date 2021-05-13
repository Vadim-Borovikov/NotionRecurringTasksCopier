using Newtonsoft.Json;

namespace NotionRecurringTasksCopier
{
    internal sealed class Config
    {
        [JsonProperty]
        public string DatabaseId { get; set; }

        [JsonProperty]
        public string Token { get; set; }
    }
}
