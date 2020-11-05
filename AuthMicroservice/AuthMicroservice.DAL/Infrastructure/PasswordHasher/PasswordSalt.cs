using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.DAL.Infrastructure.PasswordHasher
{
    public class PasswordSalt
    {
        public string Hash { get; set; }

        public string Salt { get; set; }
    }
}
