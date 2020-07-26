using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductDataLayer.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public List<T> Get();

        public List<T> Get(Expression<Func<T, bool>> predicate);

        public T Update(T entity);

        public void Delete(T entity);

        public T Add(T entity);
    }
}
