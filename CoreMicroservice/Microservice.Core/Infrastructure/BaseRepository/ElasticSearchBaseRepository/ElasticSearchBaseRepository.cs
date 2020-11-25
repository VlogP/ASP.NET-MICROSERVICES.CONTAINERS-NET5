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

        public OperationResult<object> Add(TDocument document)
        {
            var result = new OperationResult<object>();

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

        public async Task<OperationResult<object>> AddAsync(TDocument document)
        {
            var result = new OperationResult<object>();

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

        public OperationResult<object> AddMany(List<TDocument> documents)
        {
            var result = new OperationResult<object>();

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

        public async Task<OperationResult<object>> AddManyAsync(List<TDocument> documents)
        {
            var result = new OperationResult<object>();

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

        public OperationResult<object> Delete(TDocument document)
        {
            var result = new OperationResult<object>();

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

        public async Task<OperationResult<object>> DeleteAsync(TDocument document)
        {
            var result = new OperationResult<object>();

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

        public OperationResult<object> DeleteMany(List<TDocument> documents)
        {
            var result = new OperationResult<object>();

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

        public async Task<OperationResult<object>> DeleteManyAsync(List<TDocument> documents)
        {
            var result = new OperationResult<object>();

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

        public OperationResult<object> Update(TDocument document)
        {
            var result = new OperationResult<object>();

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

        public async Task<OperationResult<object>> UpdateAsync(TDocument document)
        {
            var result = new OperationResult<object>();

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
