using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Messages.Infrastructure.Mongo.Models
{
    public class MongoBaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
