using Microservice.Core.Infrastructure.SQLBaseRepository;
using TempateMicroservice.DAL.Models.SQLServer;

namespace TempateMicroservice.DAL.Repositories.SQLServer.Interfaces
{
    public interface ITempateSQLServerRepository : ISQLBaseRepository<TemplateModel> 
    {
    }
}
