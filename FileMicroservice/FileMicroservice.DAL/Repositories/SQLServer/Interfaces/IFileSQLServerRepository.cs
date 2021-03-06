using Microservice.Core.Infrastructure.SQLBaseRepository;
using FileMicroservice.DAL.Models.SQLServer;

namespace FileMicroservice.DAL.Repositories.SQLServer.Interfaces
{
    public interface IFileSQLServerRepository : ISQLBaseRepository<FileModel> 
    {
    }
}
