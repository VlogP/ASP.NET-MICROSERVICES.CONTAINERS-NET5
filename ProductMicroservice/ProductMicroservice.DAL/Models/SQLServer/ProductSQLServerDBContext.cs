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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.BsonId, p.ClientId })
                .IsUnique();
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Client> Clients { get; set; }
    }
}
