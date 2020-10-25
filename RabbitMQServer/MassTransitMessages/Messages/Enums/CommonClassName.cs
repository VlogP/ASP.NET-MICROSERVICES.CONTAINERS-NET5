﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Microservice.Messages.Enums
{
    public enum CommonClassName
    {
        [Display(Name = "Repository")]
        Repository,
        [Display(Name = "Service")]
        Service
    }
}
