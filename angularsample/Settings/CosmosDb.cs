using Microsoft.Extensions.Configuration;
using System.IO;

namespace AngularPerformanceSamples.Settings
{
    public class CosmosDb
    {
        private static IConfiguration configuration;
        static CosmosDb()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("cosmosdb.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();

        }
        public static string Get(string name)
        {
            string appSettings = configuration[name];
            return appSettings;
        }
        public string EndpointUrl { get => Get("EndpointUrl"); }
        public string PrimaryKey { get => Get("PrimaryKey"); }
    }
}
