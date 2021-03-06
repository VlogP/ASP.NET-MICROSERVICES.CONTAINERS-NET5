using Microservice.Core.Infrastructure.SQLBaseRepository;
using FileMicroservice.DAL.Models.SQLServer;
using FileMicroservice.DAL.Repositories.SQLServer.Interfaces;

namespace FileMicroservice.DAL.Repositories.SQLServer.Classes
{
    public class FileSQLServerRepository : SQLBaseRepository<FileSQLServerDBContext, FileModel>, IFileSQLServerRepository
    {
        public FileSQLServerRepository(FileSQLServerDBContext context) : base(context)
        {

        }

    }
}
