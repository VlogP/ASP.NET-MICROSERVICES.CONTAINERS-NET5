using ReportBuisnessLayer.Services.Interfaces;
using ReportDataLayer.Models;
using ReportDataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuisnessLayer.Services.Classes
{
    public class ReportService : IReportService
    {
        private IReportRepository _reportRepository;

        public ReportService(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public void Add(Report product)
        {
            _reportRepository.Add(product);
        }

        public List<Report> GetAll()
        {
            return _reportRepository.Get();
        }
    }
}
