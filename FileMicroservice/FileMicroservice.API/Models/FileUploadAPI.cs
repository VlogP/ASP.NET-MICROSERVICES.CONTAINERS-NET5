using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileMicroservice.API.Models
{
    public class FileUploadAPI
    {
        public IFormFile File { get; set; }

        public Guid ReportId { get; set; }

        public string Description { get; set; }
    }
}
