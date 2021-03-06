﻿using Microservice.Core.Infrastructure.Attributes.Mongo;
using Microservice.Core.Infrastructure.Mongo.DependencyInjection;
using Microservice.Core.Infrastructure.Mongo.Models;
using Microservice.Core.Infrastructure.OperationResult;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.BaseRepository.MongoBaseRepository
{
    public class MongoBaseRepository<TContext, TDocument> : IMongoBaseRepository<TDocument> 
        where TDocument : MongoBaseModel
        where TContext : MongoDbContext
    {
        protected readonly TContext _context;
        protected readonly IMongoDatabase _mongoDatabase;
        protected readonly IMongoCollection<TDocument> _mongoCollection;

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

        public OperationResult.OperationResult Add(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                _mongoCollection.InsertOne(document);
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
                await _mongoCollection.InsertOneAsync(document);
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
                _mongoCollection.InsertMany(documents);
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
                await _mongoCollection.InsertManyAsync(documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteById(string id)
        {
            var result = new OperationResult.OperationResult();
            var objectId = new ObjectId(id);

            try {
                _mongoCollection.DeleteOne(item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteByIdAsync(string id)
        {
            var result = new OperationResult.OperationResult();
            var objectId = new ObjectId(id);

            try {
                await _mongoCollection.DeleteOneAsync(item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try {
                _mongoCollection.DeleteMany(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _mongoCollection.DeleteManyAsync(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try {
                _mongoCollection.DeleteOne(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try {
                await  _mongoCollection.DeleteOneAsync(filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult<TDocument>> GetByIdAsync(string id)
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
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult<List<TProjection>> Get<TProjection>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjection>> projectionExpression)
        {
            var result = new OperationResult<List<TProjection>>();

            try
            {
                var data = _mongoCollection.Find(filterExpression).Project(projectionExpression).ToList();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult<TProjection> GetById<TProjection>(string id, Expression<Func<TDocument, TProjection>> projectionExpression)
        {
            var result = new OperationResult<TProjection>();
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);

            try
            {
                var data = _mongoCollection.Find(filter).Project(projectionExpression).FirstOrDefault();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult ReplaceOne(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                _mongoCollection.ReplaceOne(doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> ReplaceOneAsync(TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _mongoCollection.ReplaceOneAsync(doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        #region ClientSession Operation
        public OperationResult.OperationResult Add(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                _mongoCollection.InsertOne(clientSession, document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> AddAsync(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                await _mongoCollection.InsertOneAsync(clientSession, document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult AddMany(IClientSessionHandle clientSession, List<TDocument> documents)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                _mongoCollection.InsertMany(clientSession, documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> AddManyAsync(IClientSessionHandle clientSession, List<TDocument> documents)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                await _mongoCollection.InsertManyAsync(clientSession, documents);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult ReplaceOne(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                _mongoCollection.ReplaceOne(clientSession, doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> ReplaceOneAsync(IClientSessionHandle clientSession, TDocument document)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                await _mongoCollection.ReplaceOneAsync(clientSession, doc => doc.Id.Equals(document.Id), document);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteOne(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                _mongoCollection.DeleteOne(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteOneAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                await _mongoCollection.DeleteOneAsync(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteById(IClientSessionHandle clientSession, string id)
        {
            var result = new OperationResult.OperationResult();
            var objectId = new ObjectId(id);

            try
            {
                _mongoCollection.DeleteOne(clientSession, item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteByIdAsync(IClientSessionHandle clientSession, string id)
        {
            var result = new OperationResult.OperationResult();
            var objectId = new ObjectId(id);

            try
            {
                await _mongoCollection.DeleteOneAsync(clientSession, item => item.Id.Equals(objectId));
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteMany(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                _mongoCollection.DeleteMany(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteManyAsync(IClientSessionHandle clientSession, Expression<Func<TDocument, bool>> filterExpression)
        {
            var result = new OperationResult.OperationResult();

            try
            {
                await _mongoCollection.DeleteManyAsync(clientSession, filterExpression);
                result.Type = ResultType.Success;
            }
            catch (Exception exception)
            {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }      
        #endregion
    }
}
