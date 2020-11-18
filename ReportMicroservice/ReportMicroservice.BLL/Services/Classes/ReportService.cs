using AutoMapper;
using Microservice.Messages.Infrastructure.OperationResult;
using Microservice.Messages.Infrastructure.UnitofWork;
using ReportMicroservice.BLL.Models.DTO;
using ReportMicroservice.BLL.Services.Interfaces;
using ReportMicroservice.DAL.Models.SQLServer;
using ReportMicroservice.DAL.Repositories.Interfaces.SQLServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.BLL.Services.Classes
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public OperationResult<ReportDTO> Add(ReportDTO newReport)
        {
            newReport.Id = Guid.NewGuid();
            var reportRepository = _unitOfWork.GetRepository<IReportSQLServerRepository>();
            var report = _mapper.Map<Report>(newReport);

            var dataResult = reportRepository.Add(report);
            _unitOfWork.Save();

            var result = new OperationResult<ReportDTO>
            {
                Type = dataResult.Type,
                Errors = dataResult.Errors,
                Data = _mapper.Map<ReportDTO>(dataResult.Data)
            };

            return result;
        }

        public OperationResult<List<ReportDTO>> GetAll()
        {
            var reportRepository = _unitOfWork.GetRepository<IReportSQLServerRepository>();

            var reportData = reportRepository.Get();
            var result = new OperationResult<List<ReportDTO>>
            {
                Type = reportData.Type,
                Errors = reportData.Errors,
                Data = _mapper.Map<List<ReportDTO>>(reportData.Data)
            };

            return result;
        }
    }
}
