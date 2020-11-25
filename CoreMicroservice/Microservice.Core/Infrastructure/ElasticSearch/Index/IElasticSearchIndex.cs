using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Infrastructure.ElasticSearch.Index
{
    public interface IElasticSearchIndex
    {
        public string IndexName { get; set; }
    }
}
