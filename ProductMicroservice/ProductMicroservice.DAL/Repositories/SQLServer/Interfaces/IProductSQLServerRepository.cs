using Microservice.Core.Infrastructure.SQLBaseRepository;
using ProductMicroservice.DAL.Models.SQLServer;

namespace ProductMicroservice.DAL.Repositories.SQLServer.Interfaces
{
    public interface IProductSQLServerRepository : ISQLBaseRepository<Product> 
    {
    }
}
