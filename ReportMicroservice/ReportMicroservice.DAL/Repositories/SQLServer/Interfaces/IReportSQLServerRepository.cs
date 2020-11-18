using Microservice.Messages.Infrastructure.SQLBaseRepository;
using ReportMicroservice.DAL.Models.SQLServer;

namespace ReportMicroservice.DAL.Repositories.Interfaces.SQLServer
{
    public interface IReportSQLServerRepository : ISQLBaseRepository<Report> 
    {
    }
}
