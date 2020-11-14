using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.BLL.Models.User
{
    public class UserLogin
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
