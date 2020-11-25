using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Infrastructure.ElasticSearch.Mapping
{
    public interface IElasticSearchMapping
    {
        public string IndexName { get; set; } 

        public Func<CreateIndexDescriptor, ICreateIndexRequest> Map { get; }

        public ICreateIndexRequest MapMethod(CreateIndexDescriptor request);
    }
}
