using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotionRecurringTasksCopier.Dto.Mentions;
using NotionRecurringTasksCopier.Dto.Parents;
using NotionRecurringTasksCopier.Dto.Properties;
using NotionRecurringTasksCopier.Dto.RichTexts;

namespace NotionRecurringTasksCopier.Converters
{
    internal sealed class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (!objectType.IsAbstract && AbstractClasses.Any(t => t.IsAssignableFrom(objectType)))
            {
                // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return null;
            }

            return base.ResolveContractConverter(objectType);
        }

        private static readonly IEnumerable<Type> AbstractClasses = new[]
        {
            typeof(Property),
            typeof(Parent),
            typeof(RichText),
            typeof(Mention)
        };
    }
}