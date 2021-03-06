using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;

namespace FileMicroservice.BLL.Models.DTO
{
    public class FileDTO
    {
        public Guid FileId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public Stream FileStream { get; set; }

        public string Mime { get; set; }
    }
}
