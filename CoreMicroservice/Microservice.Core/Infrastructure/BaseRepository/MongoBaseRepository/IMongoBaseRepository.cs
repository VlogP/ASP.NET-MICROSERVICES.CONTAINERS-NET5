using Microservice.Core.Infrastructure.Mongo.Models;
using Microservice.Core.Infrastructure.OperationResult;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.BaseRepository.MongoBaseRepository
{
    public interface IMongoBaseRepository<TDocument> where TDocument : MongoBaseModel
    {
        public OperationResult<List<TDocument>> Get(Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult<List<TDocument>>> GetAsync(Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult<TDocument> GetById(string id);

        public Task<OperationResult<TDocument>> GetByIdAsync(string id);

        public OperationResult<List<TProjection>> Get<TProjection>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjection>> projectionExpression);

        public OperationResult<TProjection> GetById<TProjection>(string id, Expression<Func<TDocument, TProjection>> projectionExpression);

        public OperationResult<object> Add(IClientSessionHandle clientSession, TDocument document);

        public OperationResult<object> Add(TDocument document);

        public Task<OperationResult<object>> AddAsync(TDocument document);

        public Task<OperationResult<object>> AddAsync(IClientSessionHandle clientSession, TDocument document);

        public OperationResult<object> AddMany(List<TDocument> documents);

        public OperationResult<object> AddMany(IClientSessionHandle clientSession, List<TDocument> documents);

        public Task<OperationResult<object>> AddManyAsync(List<TDocument> documents);

        public Task<OperationResult<object>> AddManyAsync(IClientSessionHandle clientSession, List<TDocument> documents);

        public OperationResult<object> ReplaceOne(TDocument document);

        public OperationResult<object> ReplaceOne(IClientSessionHandle clientSession, TDocument document);

        public Task<OperationResult<object>> ReplaceOneAsync(TDocument document);

        public Task<OperationResult<object>> ReplaceOneAsync(IClientSessionHandle clientSession, TDocument document);

        public OperationResult<object> DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult<object> DeleteOne(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult<object>> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult<object>> DeleteOneAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult<object> DeleteById(string id);

        public OperationResult<object> DeleteById(IClientSessionHandle clientSession, string id);

        public Task<OperationResult<object>> DeleteByIdAsync(string id);

        public Task<OperationResult<object>> DeleteByIdAsync(IClientSessionHandle clientSession, string id);

        public OperationResult<object> DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult<object> DeleteMany(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult<object>> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult<object>> DeleteManyAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);
    }

}
