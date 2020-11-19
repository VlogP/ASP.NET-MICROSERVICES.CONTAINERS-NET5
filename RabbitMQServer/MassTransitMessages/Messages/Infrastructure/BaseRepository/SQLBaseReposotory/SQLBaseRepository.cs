using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Messages.Infrastructure.SQLBaseRepository
{
    public class SQLBaseRepository<TContext, TEntity> : ISQLBaseRepository<TEntity> 
        where TEntity : class 
        where TContext : DbContext
    {
        protected TContext _context;
        protected DbSet<TEntity> _dbSet;

        public SQLBaseRepository(TContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public OperationResult<TEntity> Add(TEntity entity)
        {
            var result = new OperationResult<TEntity>();

            try {
                var data = _dbSet.Add(entity).Entity;
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<TEntity>> AddAsync(TEntity entity)
        {
            var result = new OperationResult<TEntity>();

            try {
                var data = (await _dbSet.AddAsync(entity)).Entity;
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> AddMany(List<TEntity> entities)
        {
            var result = new OperationResult<object>();

            try {
                _dbSet.AddRange(entities);
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            _dbSet.AddRange(entities);

            return result;
        }

        public async Task<OperationResult<object>> AddManyAsync(List<TEntity> entities)
        {
            var result = new OperationResult<object>();

            try {
                await _dbSet.AddRangeAsync(entities);
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> Delete(TEntity entity)
        {
            var result = new OperationResult<object>();
            
            try {
                _dbSet.Remove(entity);
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteAsync(TEntity entity)
        {
            var result = new OperationResult<object>();

            try {
                await Task.Run(() => _dbSet.Remove(entity));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> DeleteMany(List<TEntity> entities)
        {
            var result = new OperationResult<object>();

            try {
                _dbSet.RemoveRange(entities);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> DeleteManyAsync(List<TEntity> entities)
        {
            var result = new OperationResult<object>();

            try {
                await Task.Run(() => _dbSet.RemoveRange(entities));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<List<TEntity>> Get()
        {
            var result = new OperationResult<List<TEntity>>();

            try {
                var data = _dbSet.ToList();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<List<TEntity>>> GetAsync()
        {
            var result = new OperationResult<List<TEntity>>();

            try {
                var data = await _dbSet.ToListAsync();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var result = new OperationResult<List<TEntity>>();

            try {
                var data = _dbSet.Where(predicate).ToList();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<List<TEntity>>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = new OperationResult<List<TEntity>>();

            try {
                var data = await _dbSet.Where(predicate).ToListAsync();
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<TEntity> Update(TEntity entity)
        {
            var result = new OperationResult<TEntity>(); 

            try {
                var data = _dbSet.Update(entity).Entity;
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
        {
            var result = new OperationResult<TEntity>();

            try {
                var data = await Task.Run(() => _dbSet.Update(entity).Entity);
                result.Data = data;
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public OperationResult<object> UpdateMany(List<TEntity> entities)
        {
            var result = new OperationResult<object>();

            try {
                _dbSet.UpdateRange(entities);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }

        public async Task<OperationResult<object>> UpdateManyAsync(List<TEntity> entities)
        {
            var result = new OperationResult<object>();

            try {
                await Task.Run(() => _dbSet.UpdateRange(entities));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors = new List<string>() { exception.Message };
            }

            return result;
        }
    }
}
