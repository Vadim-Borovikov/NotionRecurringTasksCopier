using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class CheckboxProperty : Property
    {
        [JsonProperty]
        public bool Checkbox { get; set; }
    }
}