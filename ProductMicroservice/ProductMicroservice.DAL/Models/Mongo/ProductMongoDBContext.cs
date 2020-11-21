using Microservice.Core.Infrastructure.Mongo.DependencyInjection;
using MongoDB.Driver;

namespace ProductMicroservice.DAL.Models.Mongo
{
    public class ProductMongoDbContext : MongoDbContext
    {
        public ProductMongoDbContext(IMongoClient mongoClient, string databaseName) : base(mongoClient, databaseName)
        {
        }

        public IMongoCollection<Product> Products => Database.GetCollection<Product>("Product");
    }
}
