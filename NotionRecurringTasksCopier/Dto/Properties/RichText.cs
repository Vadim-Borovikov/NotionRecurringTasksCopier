using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace NotionRecurringTasksCopier.Dto.Properties
{
    [JsonObject]
    internal sealed class RichText
    {
        public enum RichTextType
        {
            Text,
            Mention,
            Equation
        }

        [JsonProperty]
        public string PlainText { get; set; }

        [JsonProperty]
        public string Href { get; set; }

        [JsonProperty]
        public object Annotations { get; set; }

        [JsonProperty]
        public RichTextType Type { get; set; }
    }
}