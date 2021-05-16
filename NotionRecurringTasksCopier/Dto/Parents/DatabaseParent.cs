using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Parents
{
    internal sealed class DatabaseParent : Parent
    {
        [JsonProperty]
        public override TypeEnum Type => TypeEnum.DatabaseId;

        [JsonProperty]
        public string DatabaseId { get; set; }
    }
}