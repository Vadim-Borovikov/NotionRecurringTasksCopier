using System.IO;
using Microsoft.Extensions.Configuration;
using NotionRecurringTasksCopier.Dto;

namespace NotionRecurringTasksCopier
{
    internal static class Program
    {
        private static void Main()
        {
            System.Console.Write("Reading config... ");

            Config config = GetConfig();

            System.Console.WriteLine("done.");

            QueryDatabaseResult result = NotionProvider.QueryDatabase(config.DatabaseId, config.Token);
        }

        private static Config GetConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .Get<Config>();
        }

    }
}
