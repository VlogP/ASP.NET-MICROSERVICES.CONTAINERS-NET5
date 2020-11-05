using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.BLL.Models.DTO.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
