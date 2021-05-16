using System.Collections.Generic;
using NotionRecurringTasksCopier.Dto;
using NotionRecurringTasksCopier.Dto.Filters;

namespace NotionRecurringTasksCopier
{
    internal static class DataManager
    {
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public static List<Page> QueryDatabase(string datebaseId, string token, Filter filter = null,
            List<QueryDatabaseRequest.Sort> sorts = null)
        {
            var result = new List<Page>();
            QueryDatabaseResult queryDatabase = null;
            do
            {
                queryDatabase =
                    NotionProvider.QueryDatabase(datebaseId, token, filter, sorts, queryDatabase?.NextCursor);
                result.AddRange(queryDatabase.Results);
            }
            while (queryDatabase.HasMore);
            return result;
        }

        public static void AddTaskToDatabase(Task task, string databaseId, string token)
        {
            NotionProvider.CreateDatabasePage(databaseId, task.GetPageProperties(), token);
        }
    }
}
