using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportMicroservice.DAL.Models.SQLServer
{
    public class ReportSQLServerDBContext : DbContext
    {
        public ReportSQLServerDBContext(DbContextOptions<ReportSQLServerDBContext> options) : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }
    }
}
