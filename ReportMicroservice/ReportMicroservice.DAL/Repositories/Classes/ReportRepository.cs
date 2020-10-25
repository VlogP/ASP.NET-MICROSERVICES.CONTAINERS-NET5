using ReportMicroservice.DAL.Models;
using ReportMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReportMicroservice.DAL.Repositories.Classes
{
    public class ReportRepository : BaseRepository<Report>, IReportRepository
    {
        public ReportRepository(ReportDBContext context) : base(context)
        {

        }

    }
}
