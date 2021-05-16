using Newtonsoft.Json;
using NotionRecurringTasksCopier.Converters;
// ReSharper disable UnusedMember.Global

namespace NotionRecurringTasksCopier.Dto.RichTexts
{
    [JsonConverter(typeof(RichTextConverter))]
    internal abstract class RichText
    {
        public sealed class AnnotationsType
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
                Red,
                GrayBackground,
                BrownBackground,
                OrangeBackground,
                YellowBackground,
                GreenBackground,
                BlueBackground,
                PurpleBackground,
                PinkBackground,
                RedBackground
            }

            [JsonProperty]
            public bool Bold { get; set; }

            [JsonProperty]
            public bool Italic { get; set; }

            [JsonProperty]
            public bool Strikethrough { get; set; }

            [JsonProperty]
            public bool Underline { get; set; }

            [JsonProperty]
            public bool Code { get; set; }

            [JsonProperty]
            public ColorEnum Color { get; set; }
        }

        public enum TypeEnum
        {
            Text,
            Mention
        }

        [JsonProperty]
        public string PlainText { get; set; }

        [JsonProperty]
        public string Href { get; set; }

        [JsonProperty]
        public AnnotationsType Annotations { get; set; }

        [JsonProperty]
        public abstract TypeEnum Type { get; }
    }
}