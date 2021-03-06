using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportMicroservice.API.Models
{
    public class FileAPI
    {
        public IFormFile File { get; set; }

        public string Description { get; set; }
    }
}
