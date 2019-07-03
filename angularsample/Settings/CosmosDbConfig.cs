using Microsoft.Extensions.Configuration;
using System.IO;

namespace AngularPerformanceSamples.Settings
{
    public class CosmosDbConfig : ICosmosDbConfig
    {
        private static IConfiguration configuration;
        static CosmosDbConfig()
        {
            var builder = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("./Settings/cosmosdb.json", optional: true, reloadOnChange: true);
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
