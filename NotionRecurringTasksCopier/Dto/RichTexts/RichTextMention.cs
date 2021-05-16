using Newtonsoft.Json;
using NotionRecurringTasksCopier.Dto.Mentions;

namespace NotionRecurringTasksCopier.Dto.RichTexts
{
    internal sealed class RichTextMention : RichText
    {
        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Mention;

        [JsonProperty]
        public Mention Mention { get; set; }
    }
}