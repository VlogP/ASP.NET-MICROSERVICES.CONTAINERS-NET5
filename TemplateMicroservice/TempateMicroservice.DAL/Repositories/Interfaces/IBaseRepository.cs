using Microservice.Messages.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TempateMicroservice.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public OperationResult<List<T>> Get();

        public OperationResult<List<T>> Get(Expression<Func<T, bool>> predicate);

        public OperationResult<T> Update(T entity);

        public void Delete(T entity);

        public OperationResult<T> Add(T entity);
    }
}
