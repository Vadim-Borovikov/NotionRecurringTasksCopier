using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.Properties
{
    internal sealed class CheckboxProperty : Property
    {
        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Checkbox;

        [JsonProperty]
        public bool Checkbox { get; set; }
    }
}