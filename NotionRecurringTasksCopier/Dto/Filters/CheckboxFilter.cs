using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Filters
{
    internal sealed class CheckboxFilter : Filter
    {
        public sealed class CheckboxType : Filter
        {
            [JsonProperty]
#pragma warning disable 109
            public new bool Equals { get; set; }
#pragma warning restore 109

            [JsonProperty]
            public bool DoesNotEqual { get; set; }
        }

        [JsonProperty]
        public CheckboxType Checkbox { get; set; }
    }
}