using Microservice.Messages.Infrastructure.SQLBaseRepository;
using ReportMicroservice.DAL.Models.SQLServer;
using ReportMicroservice.DAL.Repositories.Interfaces.SQLServer;

namespace ReportMicroservice.DAL.Repositories.Classes.SQLServer
{
    public class ReportSQLServerRepository : SQLBaseRepository<ReportSQLServerDBContext, Report>, IReportSQLServerRepository
    {
        public ReportSQLServerRepository(ReportSQLServerDBContext context) : base(context)
        {

        }

    }
}
