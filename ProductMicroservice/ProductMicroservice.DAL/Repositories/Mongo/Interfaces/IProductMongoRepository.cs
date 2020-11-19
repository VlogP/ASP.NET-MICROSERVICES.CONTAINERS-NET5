using Microservice.Messages.Infrastructure.BaseRepository.MongoBaseRepository;
using ProductMicroservice.DAL.Models.Mongo;

namespace ProductMicroservice.DAL.Repositories.Mongo.Interfaces
{
    public interface IProductMongoRepository: IMongoBaseRepository<Product>
    {
    }
}
