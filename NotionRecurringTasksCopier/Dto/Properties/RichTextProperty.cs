using System.Collections.Generic;
using Newtonsoft.Json;
using NotionRecurringTasksCopier.Dto.RichTexts;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class RichTextProperty : Property
    {
        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Text;

        [JsonProperty]
        public List<RichText> Text { get; set; }
    }
}