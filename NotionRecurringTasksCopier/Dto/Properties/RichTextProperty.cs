using System.Collections.Generic;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class RichTextProperty : Property
    {
        [JsonProperty]
        public List<RichText> RichText { get; set; }
    }
}