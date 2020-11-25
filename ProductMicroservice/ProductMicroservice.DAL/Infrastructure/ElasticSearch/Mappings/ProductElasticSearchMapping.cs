using Microservice.Core.Constants.ElasticSearchIndexes;
using Microservice.Core.Infrastructure.ElasticSearch.Mapping;
using Nest;
using ProductMicroservice.DAL.Models.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Infrastructure.ElasticSearch.Mappings
{
    public class ProductElasticSearchMapping: IElasticSearchMapping
    {
        public Func<CreateIndexDescriptor, ICreateIndexRequest> Map { get => MapMethod; }

        public string IndexName { get; set; }

        public ProductElasticSearchMapping()
        {
            IndexName = ElasticSearchIndexes.ProductIndex;
        }

        public ICreateIndexRequest MapMethod(CreateIndexDescriptor request)
        {
            return request
                   .Map<Product>(item => item.AutoMap());
        }
    }
}
