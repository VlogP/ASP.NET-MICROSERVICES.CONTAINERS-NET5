﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Models
{
    public class UserLoginAPI
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
