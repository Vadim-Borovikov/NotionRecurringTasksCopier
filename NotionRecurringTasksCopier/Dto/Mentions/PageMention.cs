using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Mentions
{
    internal sealed class PageMention : Mention
    {
        internal sealed class PageType
        {
            [JsonProperty]
            public string Id { get; set; }
        }

        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Page;

        [JsonProperty]
        public PageType Page { get; set; }
    }
}