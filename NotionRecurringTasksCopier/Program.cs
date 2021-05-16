using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NotionRecurringTasksCopier.Dto;
using NotionRecurringTasksCopier.Dto.Filters;

namespace NotionRecurringTasksCopier
{
    internal static class Program
    {
        private static void Main()
        {
            Console.Write("Reading config... ");

            Config config = GetConfig();

            Console.WriteLine("done.");

            var filter = new CheckboxFilter
            {
                Property = "Test",
                Checkbox = new CheckboxFilter.CheckboxType
                {
                    Equals = true
                }
            };

            List<Page> pages = DataManager.QueryDatabase(config.DatabaseId, config.Token, filter);
            List<Task> tasks = pages.Select(p => new Task(p)).ToList();
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
