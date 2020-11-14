using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Models.User
{

    public class UserActivationAPI
    {
        public string ActivationCode { get; set; }

        public string ActivationEmail { get; set; }
    }
}
