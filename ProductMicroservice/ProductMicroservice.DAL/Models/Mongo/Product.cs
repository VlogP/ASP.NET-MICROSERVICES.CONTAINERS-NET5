using Microservice.Messages.Infrastructure.Attributes.Mongo;
using Microservice.Messages.Infrastructure.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Models.Mongo
{
    [BsonCollection("Product")]
    public class Product : MongoBaseModel
    {
        public List<string> Tags { get; set; }
    }
}
