using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Infrastructure.Mongo.DependencyInjection
{
    public class MongoDbContext
    {
        public IMongoDatabase Database { get; }

        public MongoDbContext(IMongoClient mongoClient, string databaseName)
        {
            Database = mongoClient.GetDatabase(databaseName);
        }
    }
}
