using AutoMapper;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using ReportMicroservice.BLL.Models.DTO;
using ReportMicroservice.BLL.Services.Interfaces;
using ReportMicroservice.DAL.Models.SQLServer;
using ReportMicroservice.DAL.Repositories.Interfaces.SQLServer;
using System;
using System.Collections.Generic;

namespace ReportMicroservice.BLL.Services.Classes
{
    public class ReportService : IReportService
    {
        private readonly ISQLUnitOfWork _sqlUnitOfWork;
        private readonly IMapper _mapper;

        public ReportService(ISQLUnitOfWork sqlUnitOfWork, IMapper mapper)
        {
            _sqlUnitOfWork = sqlUnitOfWork;
            _mapper = mapper;
        }

        public OperationResult<ReportDTO> Add(ReportDTO newReport)
        {
            newReport.Id = Guid.NewGuid();
            var reportRepository = _sqlUnitOfWork.GetRepository<IReportSQLServerRepository>();
            var report = _mapper.Map<Report>(newReport);

            var dataResult = reportRepository.Add(report);
            _sqlUnitOfWork.Save();

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
            var reportRepository = _sqlUnitOfWork.GetRepository<IReportSQLServerRepository>();

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
