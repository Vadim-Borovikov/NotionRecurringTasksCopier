using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NotionRecurringTasksCopier.Dto.Properties;

namespace NotionRecurringTasksCopier
{
    internal sealed class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(Property).IsAssignableFrom(objectType) && !objectType.IsAbstract)
            {
                // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
                return null;
            }
            return base.ResolveContractConverter(objectType);
        }
    }
}