using Microservice.Messages.Infrastructure.SQLBaseRepository;
using ProductMicroservice.DAL.Models.SQLServer;
using ProductMicroservice.DAL.Repositories.SQLServer.Interfaces;

namespace ProductMicroservice.DAL.Repositories.SQLServer.Classes
{
    public class ProductSQLServerRepository : SQLBaseRepository<ProductSQLServerDBContext, Product>, IProductSQLServerRepository
    {

        public ProductSQLServerRepository(ProductSQLServerDBContext context) : base(context)
        {
        }

    }
}
