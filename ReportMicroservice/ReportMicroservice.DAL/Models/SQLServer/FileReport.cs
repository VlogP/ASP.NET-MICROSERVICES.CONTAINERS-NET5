using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ReportMicroservice.DAL.Models.SQLServer
{
    public class FileReport
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Mime { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public string GoogleId { get; set; }

        [Required]
        public Report Report { get; set; }

        [Required]
        public Guid ReportId { get; set; }
    }
}
