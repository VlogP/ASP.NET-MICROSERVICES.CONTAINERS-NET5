using Microservice.Core.Infrastructure.SQLBaseRepository;
using TempateMicroservice.DAL.Models.SQLServer;
using TempateMicroservice.DAL.Repositories.SQLServer.Interfaces;

namespace TempateMicroservice.DAL.Repositories.SQLServer.Classes
{
    public class TempateSQLServerRepository : SQLBaseRepository<TemplateSQLServerDBContext, TemplateModel>, ITempateSQLServerRepository
    {
        public TempateSQLServerRepository(TemplateSQLServerDBContext context) : base(context)
        {

        }

    }
}
