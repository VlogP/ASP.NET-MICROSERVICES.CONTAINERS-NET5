using Microservice.Messages.Infrastructure.OperationResult;
using ReportMicroservice.BLL.Services.Interfaces;
using ReportMicroservice.DAL.Infrastructure.UnitofWork;
using ReportMicroservice.DAL.Models;
using ReportMicroservice.DAL.Repositories.Classes;
using ReportMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.BLL.Services.Classes
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public OperationResult<object> Add(Report product)
        {
            var reportRepository = _unitOfWork.GetRepository<IReportRepository>();

            var dataResult = reportRepository.Add(product);
            _unitOfWork.Save();

            var result = new OperationResult<object>
            {
                Type = dataResult.Type,
                Errors = dataResult.Errors
            };

            return result;
        }

        public OperationResult<List<Report>> GetAll()
        {
            var reportRepository = _unitOfWork.GetRepository<IReportRepository>();

            return reportRepository.Get();
        }
    }
}
