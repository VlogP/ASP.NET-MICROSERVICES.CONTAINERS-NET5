using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.DAL.Models.SQLServer
{
    public class AuthSQLServerDbContext : DbContext
    {
        public AuthSQLServerDbContext(DbContextOptions<AuthSQLServerDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }
    }
}
