using System.Collections.Generic;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class RelationProperty : Property
    {
        public sealed class PageReference
        {
            [JsonProperty]
            public string Id { get; set; }
        }

        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Relation;

        [JsonProperty]
        public List<PageReference> Relation { get; set; }
    }
}