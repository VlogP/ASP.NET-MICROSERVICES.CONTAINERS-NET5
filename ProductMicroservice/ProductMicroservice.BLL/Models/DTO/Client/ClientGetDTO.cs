using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.BLL.Models.DTO.Client
{
    public class ClientGetDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
