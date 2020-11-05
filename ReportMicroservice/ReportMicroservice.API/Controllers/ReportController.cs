using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportMicroservice.API.Models;
using ReportMicroservice.BLL.Models.DTO;
using ReportMicroservice.BLL.Services.Interfaces;
using System.Collections.Generic;

namespace ReportMicroservice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IReportService _reportService;

        public ReportController(ILogger<ReportController> logger,IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        [HttpGet]
        [Produces(typeof(OperationResult<List<ReportDTO>>))]
        public ActionResult GetReports()
        {
            var result = _reportService.GetAll();

            return Ok(result);
        }

        [HttpPost]
        [Produces(typeof(OperationResult<ReportDTO>))]
        public ActionResult AddReport([FromBody] ReportAPI report)
        {
            var result = _reportService.Add(new ReportDTO { 
                Name = report.Name 
            });

            return Ok(result);
        }
    }
}
