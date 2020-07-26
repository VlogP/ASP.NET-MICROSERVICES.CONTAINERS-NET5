using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportBuisnessLayer.Services.Interfaces;

namespace ReportMicroservice.Controllers
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
        public ActionResult GetReports()
        {
            var result = _reportService.GetAll();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult AddReport()
        {
            _reportService.Add(new ReportDataLayer.Models.Report { Name = "New Report" });

            return Ok();
        }
    }
}
