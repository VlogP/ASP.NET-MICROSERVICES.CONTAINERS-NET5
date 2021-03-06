using Microservice.Core.Infrastructure.SQLBaseRepository;
using ReportMicroservice.DAL.Models.SQLServer;
using ReportMicroservice.DAL.Repositories.Interfaces.SQLServer;
using ReportMicroservice.DAL.Repositories.SQLServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.DAL.Repositories.SQLServer.Classes
{
    public class FileReportSQLServerRepository : SQLBaseRepository<ReportSQLServerDBContext, FileReport>, IFileReportSQLServerRepository
    {
        public FileReportSQLServerRepository(ReportSQLServerDBContext context) : base(context)
        {
        }
    }
}
