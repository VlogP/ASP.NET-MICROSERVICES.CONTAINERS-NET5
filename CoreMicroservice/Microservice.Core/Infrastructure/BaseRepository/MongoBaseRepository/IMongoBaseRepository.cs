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
        OperationResult<List<TDocument>> Get(Expression<Func<TDocument, bool>> filterExpression);

        Task<OperationResult<List<TDocument>>> GetAsync(Expression<Func<TDocument, bool>> filterExpression);

        OperationResult<TDocument> GetById(string id);

        Task<OperationResult<TDocument>> GetIdAsync(string id);

        OperationResult<object> Add(IClientSessionHandle clientSession, TDocument document);

        OperationResult<object> Add(TDocument document);

        Task<OperationResult<object>> AddAsync(TDocument document);

        Task<OperationResult<object>> AddAsync(IClientSessionHandle clientSession, TDocument document);

        OperationResult<object> AddMany(List<TDocument> documents);

        OperationResult<object> AddMany(IClientSessionHandle clientSession, List<TDocument> documents);

        Task<OperationResult<object>> AddManyAsync(List<TDocument> documents);

        Task<OperationResult<object>> AddManyAsync(IClientSessionHandle clientSession, List<TDocument> documents);

        OperationResult<object> ReplaceOne(TDocument document);

        OperationResult<object> ReplaceOne(IClientSessionHandle clientSession, TDocument document);

        Task<OperationResult<object>> ReplaceOneAsync(TDocument document);

        Task<OperationResult<object>> ReplaceOneAsync(IClientSessionHandle clientSession, TDocument document);

        OperationResult<object> DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

        OperationResult<object> DeleteOne(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        Task<OperationResult<object>> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<OperationResult<object>> DeleteOneAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        OperationResult<object> DeleteById(string id);

        OperationResult<object> DeleteById(IClientSessionHandle clientSession, string id);

        Task<OperationResult<object>> DeleteByIdAsync(string id);

        Task<OperationResult<object>> DeleteByIdAsync(IClientSessionHandle clientSession, string id);

        OperationResult<object> DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

        OperationResult<object> DeleteMany(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);

        Task<OperationResult<object>> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task<OperationResult<object>> DeleteManyAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression);
    }

}
