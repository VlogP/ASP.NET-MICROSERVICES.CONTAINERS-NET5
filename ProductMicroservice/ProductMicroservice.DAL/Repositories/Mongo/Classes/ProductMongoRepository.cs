using Microservice.Core.Infrastructure.BaseRepository.MongoBaseRepository;
using ProductMicroservice.DAL.Models.Mongo;
using ProductMicroservice.DAL.Repositories.Mongo.Interfaces;


namespace ProductMicroservice.DAL.Repositories.Mongo.Classes
{
    public class ProductMongoRepository : MongoBaseRepository<ProductMongoDbContext, Product>, IProductMongoRepository
    {
        public ProductMongoRepository(ProductMongoDbContext context) : base(context)
        {

        }
    }
}
