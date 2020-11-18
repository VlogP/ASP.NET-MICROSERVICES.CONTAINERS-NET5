using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Models.Mongo
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        
        public List<string> Tags { get; set; }
    }
}
