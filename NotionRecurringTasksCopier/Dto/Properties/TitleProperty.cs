using System.Collections.Generic;
using Newtonsoft.Json;
using NotionRecurringTasksCopier.Dto.RichTexts;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class TitleProperty : Property
    {
        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Title;

        [JsonProperty]
        public List<RichText> Title { get; set; }
    }
}