using System.Collections.Generic;
using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto
{
    internal abstract class Response<T> : ObjectBase
    {
        [JsonProperty]
        public override TypeEnum Object => TypeEnum.List;

        [JsonProperty]
        public bool HasMore { get; set; }

        [JsonProperty]
        public string NextCursor { get; set; }

        [JsonProperty]
        public List<T> Results { get; set; }
    }
}