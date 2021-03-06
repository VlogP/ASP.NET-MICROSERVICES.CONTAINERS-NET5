using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.BLL.Models.DTO.Product
{
    public class ProductGetDTO
    {
        public string Id { get; set; }

        public Guid ClientId { get; set; }

        public string ClientName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
