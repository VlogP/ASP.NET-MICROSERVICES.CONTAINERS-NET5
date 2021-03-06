using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Messages.FileReport
{
    public class FileReportDownloadResponse
    {
        public string GoogleId { get; set; }

        public string Name { get; set; }

        public string Mime { get; set; }
    }
}
