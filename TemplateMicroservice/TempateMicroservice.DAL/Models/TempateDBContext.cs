using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TempateMicroservice.DAL.Models
{
    public class TempateDBContext : DbContext
    {
        public TempateDBContext(DbContextOptions<TempateDBContext> options) : base(options)
        {
        }

        public DbSet<TempateModel> Products { get; set; }
    }
}
