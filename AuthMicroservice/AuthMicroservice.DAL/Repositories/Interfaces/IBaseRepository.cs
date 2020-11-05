using Microservice.Messages.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AuthMicroservice.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public OperationResult<List<T>> Get();

        public OperationResult<List<T>> Get(Expression<Func<T, bool>> predicate);

        public OperationResult<T> Update(T entity);

        public OperationResult<object> Delete(T entity);

        public OperationResult<T> Add(T entity);
    }
}
