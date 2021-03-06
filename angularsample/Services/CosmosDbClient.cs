﻿using AngularPerformanceSamples.Extensions;
using AngularPerformanceSamples.Models;
using AngularPerformanceSamples.Settings;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AngularPerformanceSamples.Services
{
    public class CosmosDbClient
    {
        private IDocumentClient client;
        private string databaseId;
        private string collectionId;
        public CosmosDbClient(ICosmosDbConfig config)
        {
            client = new DocumentClient(new Uri(config.EndpointUrl), config.PrimaryKey);
        }
        public async Task CreateOrSetDatabase(string databaseId)
        {
            this.databaseId = databaseId;
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
        }
        //potentially put this method on the db client so it will be initialized without potential runtime exception?
        public async Task CreateCollection(string collectionId)
        {
            if (string.IsNullOrEmpty(databaseId))
            {
                throw new Exception("Must create a data base before you can create a collection");
            }
            this.collectionId = collectionId;
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId), new DocumentCollection { Id = collectionId });
        }
        public async Task CreateDocumentIfNotExists(IDocumentObject document)
        {
            try
            {
                await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, document.Id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document);
                }
                else
                {
                    throw;
                }
            }
        }
        public async Task<DocumentObject> GetDocument(string id)
        {

            var link = UriFactory.CreateDocumentUri(databaseId, collectionId, id);

            var docObj = await client.ReadDocumentAsync<DocumentObject>(UriFactory.CreateDocumentUri(databaseId, collectionId, id));
            return docObj.Document;

        }
        public async Task<DocumentObject> QuerySingleOnDataId(string id)
        {
            // Set some common query options.
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = 10 };

            IQueryable<DocumentObject> idQuery = client.CreateDocumentQuery<DocumentObject>(
                UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), queryOptions)
                .Where(f => f.DataId == id);

            return (await idQuery.ToListAsync()).FirstOrDefault();
        }
       
    }
}
