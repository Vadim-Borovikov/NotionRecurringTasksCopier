using System.Collections.Generic;
using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class MultiSelectProperty : Property
    {
        public sealed class Option
        {
            public enum OptionColor
            {
                Default,
                Gray,
                Brown,
                Orange,
                Yellow,
                Green,
                Blue,
                Purple,
                Pink,
                Red
            }

            [JsonProperty]
            public string Name { get; set; }

            [JsonProperty]
            public string Id { get; set; }

            [JsonProperty]
            public OptionColor Color { get; set; }
        }

        [JsonProperty]
        public List<Option> MultiSelect { get; set; }
    }
}