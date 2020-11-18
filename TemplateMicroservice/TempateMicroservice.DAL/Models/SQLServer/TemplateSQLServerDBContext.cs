using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TempateMicroservice.DAL.Models.SQLServer
{
    public class TemplateSQLServerDBContext : DbContext
    {
        public TemplateSQLServerDBContext(DbContextOptions<TemplateSQLServerDBContext> options) : base(options)
        {
        }

        public DbSet<TemplateModel> Products { get; set; }
    }
}
