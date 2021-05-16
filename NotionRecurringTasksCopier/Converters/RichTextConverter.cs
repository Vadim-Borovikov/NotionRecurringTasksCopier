using System;
using Newtonsoft.Json.Linq;
using NotionRecurringTasksCopier.Dto.RichTexts;

namespace NotionRecurringTasksCopier.Converters
{
    internal sealed class RichTextConverter : Converter<RichText, RichText.TypeEnum>
    {
        protected override RichText Deserialize(RichText.TypeEnum value, JObject jObject)
        {
            switch (value)
            {
                case RichText.TypeEnum.Text:
                    return Deserialize<RichTextText>(jObject);
                case RichText.TypeEnum.Mention:
                    return Deserialize<RichTextMention>(jObject);
                default:
                    throw new Exception($"Unsupported type {value}!");
            }
        }
    }
}