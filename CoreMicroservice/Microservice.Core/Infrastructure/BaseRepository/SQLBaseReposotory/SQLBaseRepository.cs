using Microservice.Core.Infrastructure.OperationResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.SQLBaseRepository
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
                result.Errors.Add(exception.Message);                
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
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult AddMany(List<TEntity> entities)
        {
            var result = new OperationResult.OperationResult();

            try {
                _dbSet.AddRange(entities);
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            _dbSet.AddRange(entities);

            return result;
        }

        public async Task<OperationResult.OperationResult> AddManyAsync(List<TEntity> entities)
        {
            var result = new OperationResult.OperationResult();

            try {
                await _dbSet.AddRangeAsync(entities);
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult Delete(TEntity entity)
        {
            var result = new OperationResult.OperationResult();
            
            try {
                _dbSet.Remove(entity);
                result.Type = ResultType.Success;
            }
            catch(Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteAsync(TEntity entity)
        {
            var result = new OperationResult.OperationResult();

            try {
                await Task.Run(() => _dbSet.Remove(entity));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult DeleteMany(List<TEntity> entities)
        {
            var result = new OperationResult.OperationResult();

            try {
                _dbSet.RemoveRange(entities);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> DeleteManyAsync(List<TEntity> entities)
        {
            var result = new OperationResult.OperationResult();

            try {
                await Task.Run(() => _dbSet.RemoveRange(entities));
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
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
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public OperationResult.OperationResult UpdateMany(List<TEntity> entities)
        {
            var result = new OperationResult.OperationResult();

            try {
                _dbSet.UpdateRange(entities);
                result.Type = ResultType.Success;
            }
            catch (Exception exception) {
                result.Type = ResultType.Invalid;
                result.Errors.Add(exception.Message);
            }

            return result;
        }

        public async Task<OperationResult.OperationResult> UpdateManyAsync(List<TEntity> entities)
        {
            var result = new OperationResult.OperationResult();

            try {
                await Task.Run(() => _dbSet.UpdateRange(entities));
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
