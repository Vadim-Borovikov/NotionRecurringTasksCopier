using System;
using Newtonsoft.Json.Linq;
using NotionRecurringTasksCopier.Dto.Mentions;

namespace NotionRecurringTasksCopier.Converters
{
    internal sealed class MentionConverter : Converter<Mention, Mention.TypeEnum>
    {
        protected override Mention Deserialize(Mention.TypeEnum value, JObject jObject)
        {
            switch (value)
            {
                case Mention.TypeEnum.Page:
                    return Deserialize<PageMention>(jObject);
                default:
                    throw new Exception($"Unsupported type {value}!");
            }
        }
    }
}