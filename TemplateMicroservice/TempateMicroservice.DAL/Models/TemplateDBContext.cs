using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TempateMicroservice.DAL.Models
{
    public class TemplateDBContext : DbContext
    {
        public TemplateDBContext(DbContextOptions<TemplateDBContext> options) : base(options)
        {
        }

        public DbSet<TemplateModel> Products { get; set; }
    }
}
