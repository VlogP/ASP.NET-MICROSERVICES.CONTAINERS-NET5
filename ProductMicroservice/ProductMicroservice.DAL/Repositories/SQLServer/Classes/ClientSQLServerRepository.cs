using Microservice.Core.Infrastructure.SQLBaseRepository;
using ProductMicroservice.DAL.Models.SQLServer;
using ProductMicroservice.DAL.Repositories.SQLServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Repositories.SQLServer.Classes
{
    public class ClientSQLServerRepository: SQLBaseRepository<ProductSQLServerDbContext, Client>, IClientSQLServerRepository
    {
        public ClientSQLServerRepository(ProductSQLServerDbContext context) : base(context)
        {
        }
    }
}
