using AngularPerformanceSamples.Models;
using AngularPerformanceSamples.Services;
using AngularPerformanceSamples.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PerformanceSamples.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [DeploymentItem("./Settings/cosmosdb.json")]
        public async Task TestMethod1()
        {
            var dbConfig = new CosmosDbConfig();
            var dbClient = new CosmosDbClient(dbConfig);
            var document = new DocumentObject("Test-2", "smalldata1");
            await dbClient.CreateOrSetDatabase("db1");
            await dbClient.CreateCollection("collection1");
            await dbClient.CreateDocumentIfNotExists(document);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await dbClient.GetDocument(document.Id);
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
            Assert.IsTrue(result.Id == document.Id);
        }
    }
}
