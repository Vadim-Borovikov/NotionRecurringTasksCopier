using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NotionRecurringTasksCopier.Dto.Parents;

namespace NotionRecurringTasksCopier.Dto
{
    internal sealed class Page : ObjectBase
    {
        [JsonProperty]
        public override TypeEnum Object => TypeEnum.Page;

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public DateTime CreatedTime { get; set; }

        [JsonProperty]
        public DateTime LastEditedTime { get; set; }

        [JsonProperty]
        public Parent Parent { get; set; }

        [JsonProperty]
        public bool Archived { get; set; }

        [JsonProperty]
        public Dictionary<string, Properties.Property> Properties { get; set; }
    }
}