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
        private static CosmosDbClient dbClient;
        private static DocumentObject documentObj;
        [ClassInitialize]
        public static void SetupTests(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        public async Task CosmosSingleRecord()
        {
            documentObj = new DocumentObject("Test-6", "smalldata2");
            await SetupCosmosDb(documentObj);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await dbClient.GetDocument(documentObj.Id);
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
            Assert.IsTrue(result.Id == documentObj.Id);
        }
        [TestMethod]
        public async Task CosmosSingleQuery()
        {
            documentObj = new DocumentObject("Test-5", "smalldata3");
            await SetupCosmosDb(documentObj);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await dbClient.QuerySingleOnDataId(documentObj.Id);
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
            Assert.IsTrue(result.Id == documentObj.Id);
        }
        [TestMethod]
        public async Task SqlSingleQuery()
        {
            var id = "id2";
            var data = "smalldata-1";
            var client = new SqlServerClient(_testContext.Properties["SqlServerConnectionString"].ToString());
            var createTableString = "CREATE TABLE Collection1 (Id nvarchar(255),  Data nvarchar(max)); ";
            //await client.ExecuteNonQuery(createTableString);
            await client.ExecuteNonQuery($"INSERT INTO Collection1 VALUES('{id}', '{data}'); ");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var returnedData = await client.ExecuteReader($"Select top 1 * from dbo.Collection1 where Id = '{id}'" );
            stopWatch.Stop();
            var elapsed = stopWatch.ElapsedMilliseconds;
        }
        private async Task SetupCosmosDb(DocumentObject documentObj)
        {
            string endpointUrl = _testContext.Properties["EndpointUrl"].ToString();
            string primaryKey = _testContext.Properties["PrimaryKey"].ToString();
            var dbconfigMock = new Mock<ICosmosDbConfig>();
            dbconfigMock.SetupGet(x => x.EndpointUrl).Returns(endpointUrl);
            dbconfigMock.SetupGet(x => x.PrimaryKey).Returns(primaryKey);
            dbClient = new CosmosDbClient(dbconfigMock.Object);
            await dbClient.CreateOrSetDatabase("db1");
            await dbClient.CreateCollection("collection1");
            await dbClient.CreateDocumentIfNotExists(documentObj);
        }
    }
}
