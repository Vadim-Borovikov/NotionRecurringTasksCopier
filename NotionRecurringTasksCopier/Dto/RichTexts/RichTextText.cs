using System;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto.RichTexts
{
    internal sealed class RichTextText : RichText
    {
        public sealed class TextType
        {
            public sealed class LinkType
            {
                [JsonProperty]
                public string Type => "url";

                [JsonProperty]
                public Uri Url { get; set; }
            }

            [JsonProperty]
            public string Content { get; set; }

            [JsonProperty]
            public LinkType Link { get; set; }
        }

        [JsonProperty]
        public override TypeEnum Type => TypeEnum.Text;

        [JsonProperty]
        public TextType Text { get; set; }
    }
}