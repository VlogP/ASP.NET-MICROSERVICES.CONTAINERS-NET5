using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AuthMicroservice.DAL.Infrastructure.Enums
{
    public enum UserRole
    {
        [Display(Name = "User")]
        User = 1,
    }
}
