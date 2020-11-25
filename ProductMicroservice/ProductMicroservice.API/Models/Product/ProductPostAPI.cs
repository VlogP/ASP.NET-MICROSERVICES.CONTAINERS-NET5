using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Models.Product
{
    public class ProductPostAPI
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }

        public string ClientId { get; set; }
    }
}
