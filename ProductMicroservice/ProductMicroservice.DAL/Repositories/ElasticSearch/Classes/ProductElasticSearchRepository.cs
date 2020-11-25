using Microservice.Core.Infrastructure.BaseRepository.ElasticSearchBaseRepository;
using Microservice.Core.Infrastructure.ElasticSearch.Index;
using Nest;
using ProductMicroservice.DAL.Models.ElasticSearch;
using ProductMicroservice.DAL.Repositories.ElasticSearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Repositories.ElasticSearch.Classes
{
    public class ProductElasticSearchRepository : ElasticSearchBaseRepository<ProductElasticSearchIndex, Product>, IProductElasticSearchRepository
    {
        public ProductElasticSearchRepository(ElasticClient elasticClient, ProductElasticSearchIndex elasticSearchIndex) : base(elasticClient, elasticSearchIndex)
        {

        }
    }
}
