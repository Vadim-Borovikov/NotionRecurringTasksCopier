using System.Collections.Generic;
using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class MultiSelectProperty : Property
    {
        public sealed class Option
        {
            public enum ColorEnum
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
            public ColorEnum Color { get; set; }
        }

        [JsonProperty]
        public override TypeEnum Type => TypeEnum.MultiSelect;

        [JsonProperty]
        public List<Option> MultiSelect { get; set; }
    }
}