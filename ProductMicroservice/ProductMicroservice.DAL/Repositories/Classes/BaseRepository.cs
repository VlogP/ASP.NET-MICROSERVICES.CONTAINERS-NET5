using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.EntityFrameworkCore;
using ProductMicroservice.DAL.Models;
using ProductMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductMicroservice.DAL.Repositories.Classes
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ProductDBContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(ProductDBContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public OperationResult<T> Add(T entity)
        {
            var result = new OperationResult<T>
            {
                Data = _dbSet.Add(entity).Entity,
                Type = ResultType.Success
            };
            _context.SaveChanges();

            return result;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public OperationResult<List<T>> Get()
        {
            var result = new OperationResult<List<T>>
            {
                Data = _dbSet.ToList(),
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<List<T>> Get(Expression<Func<T,bool>> predicate)
        {
            var result = new OperationResult<List<T>>
            {
                Data = _dbSet.Where(predicate).ToList(),
                Type = ResultType.Success
            };

            return result;
        }

        public OperationResult<T> Update(T entity)
        {
            var result = new OperationResult<T>
            {
                Data = _dbSet.Update(entity).Entity,
                Type = ResultType.Success
            };

            return result;
        }
    }
}
