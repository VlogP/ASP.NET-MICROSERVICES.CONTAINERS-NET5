using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Messages.FileReport
{
    public class FileReportDownloadRequest
    {
        public Guid FileId { get; set; }
    }
}
