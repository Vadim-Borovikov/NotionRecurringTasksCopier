using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NotionRecurringTasksCopier.Dto;
using NotionRecurringTasksCopier.Dto.Filters;

namespace NotionRecurringTasksCopier
{
    internal static class NotionProvider
    {
        public static QueryDatabaseResult QueryDatabase(string datebaseId, string token, Filter filter = null,
            List<QueryDatabaseRequest.Sort> sorts = null, string startCursor = null, int pageSize = MaxPageSize)
        {
            string apiProvider = $"{ApiProviderPrefix}{datebaseId}/";

            var dto = new QueryDatabaseRequest
            {
                Filter = filter,
                Sorts = sorts,
                StartCursor = startCursor,
                PageSize = pageSize
            };

            return RestHelper.CallPostMethod<QueryDatabaseResult>(apiProvider, QueryMethod, dto, Settings, token);
        }

        private static readonly BaseSpecifiedConcreteClassConverter ContractResolver =
            new BaseSpecifiedConcreteClassConverter
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };
        private static readonly List<JsonConverter> Converters = new List<JsonConverter>
        {
            new StringEnumConverter(ContractResolver.NamingStrategy)
        };
        internal static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = ContractResolver,
            Converters = Converters,
            NullValueHandling = NullValueHandling.Ignore
        };

        private const int MaxPageSize = 100;
        private const string ApiProviderPrefix = "https://api.notion.com/v1/databases/";
        private const string QueryMethod = "query";
    }
}
