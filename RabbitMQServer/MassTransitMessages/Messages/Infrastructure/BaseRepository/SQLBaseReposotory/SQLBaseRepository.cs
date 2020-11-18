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
            var result = new OperationResult<TEntity>
            {
                Data = _dbSet.Add(entity).Entity,
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<TEntity>> AddAsync(TEntity entity)
        {
            var data = (await _dbSet.AddAsync(entity)).Entity;

            var result = new OperationResult<TEntity>
            {
                Data = data,
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<object> AddMany(List<TEntity> entities)
        {
            _dbSet.AddRange(entities);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<object>> AddManyAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<object> Delete(TEntity entity)
        {
            _dbSet.Remove(entity);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<object>> DeleteAsync(TEntity entity)
        {
            await Task.Run(() => _dbSet.Remove(entity));

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<object> DeleteMany(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<object>> DeleteManyAsync(List<TEntity> entities)
        {
            await Task.Run(() => _dbSet.RemoveRange(entities));

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<List<TEntity>> Get()
        {
            var result = new OperationResult<List<TEntity>>
            {
                Data = _dbSet.ToList(),
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<List<TEntity>>> GetAsync()
        {
            var data = await _dbSet.ToListAsync();

            var result = new OperationResult<List<TEntity>>
            {
                Data = data,
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            var data = _dbSet.Where(predicate).ToList();
            var type = data.Count == 0 ? ResultType.BadRequest : ResultType.Success;

            var result = new OperationResult<List<TEntity>>
            {
                Data = data,
                Type = type
            };

            return result;
        }

        public async Task<OperationResult<List<TEntity>>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var data = await _dbSet.Where(predicate).ToListAsync();
            var type = data.Count == 0 ? ResultType.BadRequest : ResultType.Success;

            var result = new OperationResult<List<TEntity>>
            {
                Data = data,
                Type = type
            };

            return result;
        }

        public OperationResult<TEntity> Update(TEntity entity)
        {
            var result = new OperationResult<TEntity>
            {
                Data = _dbSet.Update(entity).Entity,
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<TEntity>> UpdateAsync(TEntity entity)
        {
            var data = await Task.Run(() => _dbSet.Update(entity).Entity);

            var result = new OperationResult<TEntity>
            {
                Data = data,
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<object> UpdateMany(List<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }

        public async Task<OperationResult<object>> UpdateManyAsync(List<TEntity> entities)
        {
            await Task.Run(() => _dbSet.UpdateRange(entities));

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
        }
    }
}
