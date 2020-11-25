using Microservice.Core.Infrastructure.BaseRepository.ElasticSearchBaseRepository;
using ProductMicroservice.DAL.Models.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductMicroservice.DAL.Repositories.ElasticSearch.Interfaces
{
    public interface IProductElasticSearchRepository: IElasticSearchBaseRepository<Product>
    {
    }
}
