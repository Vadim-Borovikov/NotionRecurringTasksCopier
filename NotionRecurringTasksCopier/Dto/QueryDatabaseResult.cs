using Newtonsoft.Json;

namespace NotionRecurringTasksCopier.Dto
{
    [JsonObject]
    internal sealed class QueryDatabaseResult : Response<Page>
    {
    }
}