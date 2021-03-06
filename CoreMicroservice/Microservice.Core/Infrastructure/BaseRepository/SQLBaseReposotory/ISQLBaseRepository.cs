using Microservice.Core.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.SQLBaseRepository
{
    public interface ISQLBaseRepository<TEntity> where TEntity : class
    {
        public OperationResult<List<TEntity>> Get();

        public Task<OperationResult<List<TEntity>>> GetAsync();

        public OperationResult<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);

        public Task<OperationResult<List<TEntity>>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        public OperationResult<TEntity> Update(TEntity entity);

        public Task<OperationResult<TEntity>> UpdateAsync(TEntity entity);

        public OperationResult.OperationResult UpdateMany(List<TEntity> entities);

        public Task<OperationResult.OperationResult> UpdateManyAsync(List<TEntity> entities);

        public OperationResult.OperationResult Delete(TEntity entity);

        public Task<OperationResult.OperationResult> DeleteAsync(TEntity entity);

        public OperationResult.OperationResult DeleteMany(List<TEntity> entities);

        public Task<OperationResult.OperationResult> DeleteManyAsync(List<TEntity> entities);

        public OperationResult<TEntity> Add(TEntity entity);

        public Task<OperationResult<TEntity>> AddAsync(TEntity entity);

        public OperationResult.OperationResult AddMany(List<TEntity> entities);

        public Task<OperationResult.OperationResult> AddManyAsync(List<TEntity> entities);
    }
}
