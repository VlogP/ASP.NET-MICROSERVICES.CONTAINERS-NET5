using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Models
{
    public class UserLoginModelWeb
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
