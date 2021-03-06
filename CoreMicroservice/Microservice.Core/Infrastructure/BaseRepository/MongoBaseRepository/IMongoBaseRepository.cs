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

        public OperationResult.OperationResult Add(IClientSessionHandle clientSession, TDocument document);

        public OperationResult.OperationResult Add(TDocument document);

        public Task<OperationResult.OperationResult> AddAsync(TDocument document);

        public Task<OperationResult.OperationResult> AddAsync(IClientSessionHandle clientSession, TDocument document);

        public OperationResult.OperationResult AddMany(List<TDocument> documents);

        public OperationResult.OperationResult AddMany(IClientSessionHandle clientSession, List<TDocument> documents);

        public Task<OperationResult.OperationResult> AddManyAsync(List<TDocument> documents);

        public Task<OperationResult.OperationResult> AddManyAsync(IClientSessionHandle clientSession, List<TDocument> documents);

        public OperationResult.OperationResult ReplaceOne(TDocument document);

        public OperationResult.OperationResult ReplaceOne(IClientSessionHandle clientSession, TDocument document);

        public Task<OperationResult.OperationResult> ReplaceOneAsync(TDocument document);

        public Task<OperationResult.OperationResult> ReplaceOneAsync(IClientSessionHandle clientSession, TDocument document);

        public OperationResult.OperationResult DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult.OperationResult DeleteOne(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult.OperationResult> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult.OperationResult> DeleteOneAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult.OperationResult DeleteById(string id);

        public OperationResult.OperationResult DeleteById(IClientSessionHandle clientSession, string id);

        public Task<OperationResult.OperationResult> DeleteByIdAsync(string id);

        public Task<OperationResult.OperationResult> DeleteByIdAsync(IClientSessionHandle clientSession, string id);

        public OperationResult.OperationResult DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        public OperationResult.OperationResult DeleteMany(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult.OperationResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        public Task<OperationResult.OperationResult> DeleteManyAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);
    }

}
