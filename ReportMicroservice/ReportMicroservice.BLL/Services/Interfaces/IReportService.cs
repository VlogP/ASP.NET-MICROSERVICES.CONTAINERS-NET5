using Microservice.Messages.Infrastructure.OperationResult;
using ReportMicroservice.BLL.Models.DTO;
using ReportMicroservice.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.BLL.Services.Interfaces
{
    public interface IReportService
    {
        OperationResult<List<ReportDTO>> GetAll();

        OperationResult<ReportDTO> Add(ReportDTO newReport);
    }
}
