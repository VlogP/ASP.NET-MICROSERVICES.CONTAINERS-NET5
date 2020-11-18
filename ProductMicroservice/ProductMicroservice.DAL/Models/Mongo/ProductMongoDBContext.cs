using Microservice.Messages.Infrastructure.Mongo.DependencyInjection;
using MongoDB.Driver;

namespace ProductMicroservice.DAL.Models.Mongo
{
    public class ProductMongoDBContext : MongoDbContext
    {
        public ProductMongoDBContext(IMongoClient mongoClient, string databaseName) : base(mongoClient, databaseName)
        {
        }

        public IMongoCollection<Product> Products => Database.GetCollection<Product>("Product");
    }
}
