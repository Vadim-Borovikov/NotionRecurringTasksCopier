using System.Collections.Generic;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class TitleProperty : Property
    {
        [JsonProperty]
        public List<RichText> Title { get; set; }
    }
}