using System.Collections.Generic;
using Newtonsoft.Json;
using NotionRecurringTasksCopier.Dto.Parents;
using NotionRecurringTasksCopier.Dto.Properties;

namespace NotionRecurringTasksCopier.Dto
{
    internal sealed class CreatePageRequest
    {
        [JsonProperty]
        public Parent Parent { get; set; }

        [JsonProperty]
        public Dictionary<string, Property> Properties { get; set; }
    }
}