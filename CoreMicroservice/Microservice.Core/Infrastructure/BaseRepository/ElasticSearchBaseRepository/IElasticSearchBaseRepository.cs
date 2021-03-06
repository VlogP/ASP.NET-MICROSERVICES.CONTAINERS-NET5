using Microservice.Core.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.BaseRepository.ElasticSearchBaseRepository
{
    public interface IElasticSearchBaseRepository<TDocument>
    {
        public OperationResult.OperationResult Add(TDocument document);

        public Task<OperationResult.OperationResult> AddAsync(TDocument document);

        public OperationResult.OperationResult AddMany(List<TDocument> documents);

        public Task<OperationResult.OperationResult> AddManyAsync(List<TDocument> documents);

        public OperationResult.OperationResult Delete(TDocument document);

        public Task<OperationResult.OperationResult> DeleteAsync(TDocument document);

        public OperationResult.OperationResult DeleteMany(List<TDocument> documents);

        public Task<OperationResult.OperationResult> DeleteManyAsync(List<TDocument> documents);

        public OperationResult.OperationResult Update(TDocument document);

        public Task<OperationResult.OperationResult> UpdateAsync(TDocument document);

    }
}
