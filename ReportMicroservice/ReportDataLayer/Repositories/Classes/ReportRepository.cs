using ReportDataLayer.Models;
using ReportDataLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportDataLayer.Repositories.Classes
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(ReportDBContext context) : base(context)
        {

        }

    }
}
