using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FileMicroservice.DAL.Models.SQLServer
{
    [Table("File")]
    public class FileModel
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        [Required]
        public string GoogleId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Mime { get; set; }
    }
}
