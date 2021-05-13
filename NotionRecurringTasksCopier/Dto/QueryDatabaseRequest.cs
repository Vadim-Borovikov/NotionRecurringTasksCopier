using Newtonsoft.Json;
using System.Collections.Generic;

namespace NotionRecurringTasksCopier.Dto
{
    internal class QueryDatabaseRequest
    {
        public class Sort
        {
            public enum SortTimestamp
            {
                CreatedTime,
                LastEditedTime,
            }

            public enum SortDirection
            {
                Ascending,
                Descending
            }

            [JsonProperty]
            public string Property { get; set; }

            [JsonProperty]
            public SortTimestamp Timestamp { get; set; }

            [JsonProperty]
            public SortDirection Direction { get; set; }
        }

        [JsonProperty]
        public Filters.Filter Filter { get; set; }

        [JsonProperty]
        public List<Sort> Sorts { get; set; }

        [JsonProperty]
        public string StartCursor { get; set; }

        [JsonProperty]
        public int PageSize { get; set; }
    }
}