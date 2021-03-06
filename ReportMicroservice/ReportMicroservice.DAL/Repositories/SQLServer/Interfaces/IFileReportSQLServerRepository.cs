using Microservice.Core.Infrastructure.SQLBaseRepository;
using ReportMicroservice.DAL.Models.SQLServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.DAL.Repositories.SQLServer.Interfaces
{
    public interface IFileReportSQLServerRepository: ISQLBaseRepository<FileReport>
    {
    }
}
