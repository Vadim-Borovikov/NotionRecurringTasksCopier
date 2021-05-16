using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class DateProperty : Property
    {
        public sealed class Dates
        {
            [JsonProperty]
            public string Start { get; set; }

            [JsonProperty]
            public string End { get; set; }
        }

        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Date;

        [JsonProperty]
        public Dates Date { get; set; }
    }
}