using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProductMicroservice.DAL.Models.SQLServer
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string BsonId { get; set; }

        [ForeignKey("Client")]
        public Guid ClientId { get; set; }

        public Client Client { get; set; }
    }
}
