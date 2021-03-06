using Microservice.Core.Infrastructure.ElasticSearch.Index;
using Microservice.Core.Infrastructure.OperationResult;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.BaseRepository.ElasticSearchBaseRepository
{
    public class ElasticSearchBaseRepository<TIndex, TDocument> : IElasticSearchBaseRepository<TDocument> 
        where TDocument : class
        where TIndex : IElasticSearchIndex
    {
        protected readonly ElasticClient _elasticClient;
        protected readonly IElasticSearchIndex _elasticSearchIndex;

        public ElasticSearchBaseRepository(ElasticClient elasticClient, IElasticSearchIndex elasticSearchIndex)
        {
            _elasticClient = elasticClient;
            _elasticSearchIndex = elasticSearchIndex;
        }

        public OperationResult.OperationResult Add(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                _elasticClient.Index(document, request => request.Index(_elasticSearchIndex.IndexName));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> AddAsync(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _elasticClient.IndexAsync(document, request => request.Index(_elasticSearchIndex.IndexName));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult AddMany(List<TDocument> documents)
        {
            var result = new OperationResult.OperationResult();

            try {
                _elasticClient.IndexMany(documents, _elasticSearchIndex.IndexName);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> AddManyAsync(List<TDocument> documents)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _elasticClient.IndexManyAsync(documents, _elasticSearchIndex.IndexName);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult Delete(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                _elasticClient.Delete<TDocument>(document, request => request.Index(_elasticSearchIndex.IndexName));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteAsync(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _elasticClient.DeleteAsync<TDocument>(document, request => request.Index(_elasticSearchIndex.IndexName));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteMany(List<TDocument> documents)
        {
            var result = new OperationResult.OperationResult();

            try {
                _elasticClient.DeleteMany(documents, _elasticSearchIndex.IndexName);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteManyAsync(List<TDocument> documents)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _elasticClient.DeleteManyAsync(documents, _elasticSearchIndex.IndexName);

                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult Update(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                 _elasticClient.Update<TDocument>(document, request => {
                    return request
                           .Index(_elasticSearchIndex.IndexName)
                           .Doc(document);
                });
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> UpdateAsync(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _elasticClient.UpdateAsync<TDocument>(document, updateRequest => { 
                    return updateRequest
                            .Index(_elasticSearchIndex.IndexName)
                            .Doc(document); 
                });
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

    }
}
