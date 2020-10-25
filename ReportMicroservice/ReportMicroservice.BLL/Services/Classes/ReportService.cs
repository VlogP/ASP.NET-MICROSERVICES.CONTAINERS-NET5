using Microservice.Messages.Infrastructure.OperationResult;
using ReportMicroservice.BLL.Services.Interfaces;
using ReportMicroservice.DAL.Models;
using ReportMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.BLL.Services.Classes
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public OperationResult<object> Add(Report product)
        {
            var dataResult = _reportRepository.Add(product);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<List<Report>> GetAll()
        {
            return _reportRepository.Get();
        }
    }
}
