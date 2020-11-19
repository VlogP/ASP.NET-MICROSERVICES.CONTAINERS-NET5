using Microservice.Messages.Infrastructure.Attributes.Mongo;
using Microservice.Messages.Infrastructure.Mongo.DependencyInjection;
using Microservice.Messages.Infrastructure.Mongo.Models;
using Microservice.Messages.Infrastructure.OperationResult;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Messages.Infrastructure.BaseRepository.MongoBaseRepository
{
    public class MongoBaseRepository<TContext, TDocument> : IMongoBaseRepository<TDocument> 
        where TDocument : MongoBaseModel
        where TContext : MongoDbContext
    {
        private readonly TContext _context;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<TDocument> _mongoCollection;

        public MongoBaseRepository(TContext context)
        {
            _context = context;
            _mongoDatabase = _context.Database;
            _mongoCollection = _mongoDatabase.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private string GetCollectionName(Type documentType)
        {
            var attribute = documentType.GetCustomAttributes(typeof(BsonCollectionAttribute), true);
            var value = ((BsonCollectionAttribute)attribute.FirstOrDefault())?.CollectionName;

            return value;
        }

        public OperationResult<object> Add(TDocument document)
        {
            var result = new OperationResult<object>();

            try {
                _mongoCollection.InsertOne(document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> AddAsync(TDocument document)
        {
            var result = new OperationResult<object>();

            try {
                await _mongoCollection.InsertOneAsync(document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> AddMany(List<TDocument> documents)
        {
            var result = new OperationResult<object>();

            try {
                _mongoCollection.InsertMany(documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> AddManyAsync(List<TDocument> documents)
        {
            var result = new OperationResult<object>();

            try {
                await _mongoCollection.InsertManyAsync(documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteById(string id)
        {
            var result = new OperationResult<object>();
            var objectId = new ObjectId(id);

            try {
                _mongoCollection.DeleteOne(item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteByIdAsync(string id)
        {
            var result = new OperationResult<object>();
            var objectId = new ObjectId(id);

            try {
                await _mongoCollection.DeleteOneAsync(item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try {
                _mongoCollection.DeleteMany(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try {
                await _mongoCollection.DeleteManyAsync(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try {
                _mongoCollection.DeleteOne(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try {
                await  _mongoCollection.DeleteOneAsync(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<List<TDocument>> Get(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<List<TDocument>>();

            try
            {
                var data = _mongoCollection.Find(filterExpression).ToList();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<List<TDocument>>> GetAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<List<TDocument>>();

            try {
                var data = await (await _mongoCollection.FindAsync(filterExpression)).ToListAsync();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<TDocument> GetById(string id)
        {
            var result = new OperationResult<TDocument>();
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            try {
                var data = _mongoCollection.Find(filter).FirstOrDefault();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<TDocument>> GetIdAsync(string id)
        {
            var result = new OperationResult<TDocument>();
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            try {
                var data = await (await _mongoCollection.FindAsync(filter)).FirstOrDefaultAsync();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> ReplaceOne(TDocument document)
        {
            var result = new OperationResult<object>();

            try {
                _mongoCollection.ReplaceOne(doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> ReplaceOneAsync(TDocument document)
        {
            var result = new OperationResult<object>();

            try {
                await _mongoCollection.ReplaceOneAsync(doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        #region ClientSession Operation
        public OperationResult<object> Add(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult<object>();

            try
            {
                _mongoCollection.InsertOne(clientSession, document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> AddAsync(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult<object>();

            try
            {
                await _mongoCollection.InsertOneAsync(clientSession, document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> AddMany(IClientSessionHandle clientSession, List<TDocument> documents)
        {
            var result = new OperationResult<object>();

            try
            {
                _mongoCollection.InsertMany(clientSession, documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> AddManyAsync(IClientSessionHandle clientSession, List<TDocument> documents)
        {
            var result = new OperationResult<object>();

            try
            {
                await _mongoCollection.InsertManyAsync(clientSession, documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> ReplaceOne(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult<object>();

            try
            {
                _mongoCollection.ReplaceOne(clientSession, doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> ReplaceOneAsync(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult<object>();

            try
            {
                await _mongoCollection.ReplaceOneAsync(clientSession, doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteOne(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try
            {
                _mongoCollection.DeleteOne(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteOneAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try
            {
                await _mongoCollection.DeleteOneAsync(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteById(IClientSessionHandle clientSession, string id)
        {
            var result = new OperationResult<object>();
            var objectId = new ObjectId(id);

            try
            {
                _mongoCollection.DeleteOne(clientSession, item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteByIdAsync(IClientSessionHandle clientSession, string id)
        {
            var result = new OperationResult<object>();
            var objectId = new ObjectId(id);

            try
            {
                await _mongoCollection.DeleteOneAsync(clientSession, item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteMany(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try
            {
                _mongoCollection.DeleteMany(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteManyAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult<object>();

            try
            {
                await _mongoCollection.DeleteManyAsync(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }
        #endregion
    }
}
