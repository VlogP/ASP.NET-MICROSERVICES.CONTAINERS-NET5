using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Models.SQLServer
{
    public class ProductSQLServerDBContext : DbContext
    {
        public ProductSQLServerDBContext(DbContextOptions<ProductSQLServerDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
