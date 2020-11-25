using Microservice.Core.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.BaseRepository.ElasticSearchBaseRepository
{
    public interface IElasticSearchBaseRepository<TDocument>
    {
        public OperationResult<object> Add(TDocument document);

        public Task<OperationResult<object>> AddAsync(TDocument document);

        public OperationResult<object> AddMany(List<TDocument> documents);

        public Task<OperationResult<object>> AddManyAsync(List<TDocument> documents);

        public OperationResult<object> Delete(TDocument document);

        public Task<OperationResult<object>> DeleteAsync(TDocument document);

        public OperationResult<object> DeleteMany(List<TDocument> documents);

        public Task<OperationResult<object>> DeleteManyAsync(List<TDocument> documents);

        public OperationResult<object> Update(TDocument document);

        public Task<OperationResult<object>> UpdateAsync(TDocument document);

    }
}
