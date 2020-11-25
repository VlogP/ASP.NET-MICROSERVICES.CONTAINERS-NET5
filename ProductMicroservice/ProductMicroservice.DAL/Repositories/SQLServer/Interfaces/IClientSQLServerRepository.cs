using Microservice.Core.Infrastructure.SQLBaseRepository;
using ProductMicroservice.DAL.Models.SQLServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Repositories.SQLServer.Interfaces
{
    public interface IClientSQLServerRepository : ISQLBaseRepository<Client>
    {
    }
}
