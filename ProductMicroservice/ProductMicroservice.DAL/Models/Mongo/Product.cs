using Microservice.Core.Infrastructure.Attributes.Mongo;
using Microservice.Core.Infrastructure.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ProductMicroservice.DAL.Models.Mongo
{
    [BsonCollection("Product")]
    public class Product : MongoBaseModel
    {
        public string Name { get; set; }

        public string ClientName { get; set; }

        public Guid ClientId{ get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; }
    }
}
