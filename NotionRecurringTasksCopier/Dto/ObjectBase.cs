using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto
{
    internal abstract class ObjectBase
    {
        internal enum TypeEnum
        {
            Page,
            List
        }

        [JsonProperty]
        public abstract TypeEnum Object { get; }
    }
}