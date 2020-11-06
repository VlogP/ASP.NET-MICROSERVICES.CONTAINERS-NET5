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
        [Required]
        public string Email { get; set; }

        [MaxLength(200)]
        [Required]
        public string Password { get; set; }

        [MaxLength(100)]
        [Required]
        public string Salt { get; set; }

        [MaxLength(200)]
        [Required]
        public string ActivationCode { get; set; }

        [Required]
        public bool IsActivated { get; set; }

        [Required]
        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
