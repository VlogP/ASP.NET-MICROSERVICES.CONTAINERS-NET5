using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.BLL.Models.DTO.Product
{
    public class ProductPostDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
