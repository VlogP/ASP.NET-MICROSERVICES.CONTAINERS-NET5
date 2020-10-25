using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AuthMicroservice.DAL.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
