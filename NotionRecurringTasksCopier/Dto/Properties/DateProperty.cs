using System;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class DateProperty : Property
    {
        public sealed class Dates
        {
            [JsonProperty]
            public DateTime Start { get; set; }

            [JsonProperty]
            public DateTime? End { get; set; }
        }

        [JsonProperty]
        public Dates Date { get; set; }
    }
}