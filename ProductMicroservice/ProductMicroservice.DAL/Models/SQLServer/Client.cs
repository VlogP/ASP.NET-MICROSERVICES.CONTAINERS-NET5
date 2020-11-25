using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProductMicroservice.DAL.Models.SQLServer
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
