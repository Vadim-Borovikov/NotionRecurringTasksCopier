using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto
{
    internal sealed class Page
    {
        public sealed class PageParent
        {
            [JsonProperty]
            public string Type { get; set; }

            [JsonProperty]
            public string DatabaseId { get; set; }
        }

        [JsonProperty]
        public string Object { get; set; }

        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public DateTime CreatedTime { get; set; }

        [JsonProperty]
        public DateTime LastEditedTime { get; set; }

        [JsonProperty]
        public PageParent Parent { get; set; }

        [JsonProperty]
        public bool Archived { get; set; }

        [JsonProperty]
        public Dictionary<string, Properties.Property> Properties { get; set; }
    }
}