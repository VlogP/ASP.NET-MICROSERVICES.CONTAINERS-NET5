using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.BLL.Models.Product
{
    public class ProductPost
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Guid ClientId { get; set; }

        public List<string> Tags { get; set; }
    }
}
