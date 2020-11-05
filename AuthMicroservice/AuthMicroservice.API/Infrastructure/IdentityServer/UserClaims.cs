using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.API.Infrastructure.IdentityServer
{
    public class UserClaims
    {
        public string Id { get; set; }

        public string Role { get; set; }
    }
}
