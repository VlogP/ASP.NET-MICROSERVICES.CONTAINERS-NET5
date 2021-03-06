using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileMicroservice.DAL.Models.SQLServer
{
    public class FileSQLServerDBContext : DbContext
    {
        public FileSQLServerDBContext(DbContextOptions<FileSQLServerDBContext> options) : base(options)
        {
        }

        public DbSet<FileModel> Products { get; set; }
    }
}
