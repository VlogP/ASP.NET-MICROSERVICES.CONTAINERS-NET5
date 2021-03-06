using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Messages.FileReport
{
    public class FileReportUploadRequest
    {
        public string GoogleId { get; set; }

        public Guid ReportId { get; set; }

        public string Name { get; set; }

        public string Mime { get; set; }

        public string Description { get; set; }
    }
}
