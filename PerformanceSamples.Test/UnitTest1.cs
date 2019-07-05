using AngularPerformanceSamples.Models;
using AngularPerformanceSamples.Services;
using AngularPerformanceSamples.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PerformanceSamples.Test
{
    [TestClass]
    public class UnitTest1
    {
        private static TestContext _testContext;
        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        public async Task TestMethod1()
        {
            string endpointUrl = _testContext.Properties["EndpointUrl"].ToString();
            string primaryKey = _testContext.Properties["PrimaryKey"].ToString();
            var dbconfigMock = new Mock<ICosmosDbConfig>();
            dbconfigMock.SetupGet(x => x.EndpointUrl).Returns(endpointUrl);
            dbconfigMock.SetupGet(x => x.PrimaryKey).Returns(primaryKey);
            var dbClient = new CosmosDbClient(dbconfigMock.Object);
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
