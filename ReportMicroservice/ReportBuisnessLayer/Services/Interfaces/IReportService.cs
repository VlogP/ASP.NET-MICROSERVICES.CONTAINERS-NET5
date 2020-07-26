using ReportDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuisnessLayer.Services.Interfaces
{
    public interface IReportService
    {
        List<Report> GetAll();

        void Add(Report report);
    }
}
