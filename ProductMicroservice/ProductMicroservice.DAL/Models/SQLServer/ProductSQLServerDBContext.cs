using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Models.SQLServer
{
    public class ProductSQLServerDbContext : DbContext
    {
        public ProductSQLServerDbContext(DbContextOptions<ProductSQLServerDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
