using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileMicroservice.BLL.Models.File
{
    public class FilePost
    {
        public IFormFile File { get; set; }

        public Guid ReportId { get; set; }

        public string Description { get; set; }
    }
}
