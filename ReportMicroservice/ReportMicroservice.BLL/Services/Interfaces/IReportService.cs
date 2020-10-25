using Microservice.Messages.Infrastructure.OperationResult;
using ReportMicroservice.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.BLL.Services.Interfaces
{
    public interface IReportService
    {
        OperationResult<List<Report>> GetAll();

        OperationResult<object> Add(Report report);
    }
}
