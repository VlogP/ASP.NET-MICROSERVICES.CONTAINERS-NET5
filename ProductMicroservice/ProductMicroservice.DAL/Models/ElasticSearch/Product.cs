using Microservice.Core.Infrastructure.ElasticSearch.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Models.ElasticSearch
{
    [ElasticsearchType(RelationName = "product")]
    public class Product : ElasticSearchBaseModel
    {
        [Text]
        public string Name { get; set; }

        [Text]
        public string Description { get; set; }

        public List<string> Tags { get; set; }
    }
}
