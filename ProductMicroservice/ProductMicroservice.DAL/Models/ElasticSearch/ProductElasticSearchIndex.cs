using Microservice.Core.Constants.ElasticSearchIndexes;
using Microservice.Core.Infrastructure.ElasticSearch.Index;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Models.ElasticSearch
{
    public class ProductElasticSearchIndex : IElasticSearchIndex
    {
        public string IndexName { get; set; }

        public ProductElasticSearchIndex(string indexName) {
            IndexName = indexName;
        }
    }
}
