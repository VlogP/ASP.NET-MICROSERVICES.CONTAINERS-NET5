﻿using AuthMicroservice.DAL.Models;
using AuthMicroservice.DAL.Repositories.Interfaces;
using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AuthMicroservice.DAL.Repositories.Classes
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected AuthDBContext _context;
        protected DbSet<T> _dbSet;

        public BaseRepository(AuthDBContext context)
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

            return result;
        }

        public OperationResult<object> Delete(T entity)
        {
            _dbSet.Remove(entity);

            var result = new OperationResult<object>
            {
                Type = ResultType.Success
            };

            return result;
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

        public OperationResult<List<T>> Get(Expression<Func<T, bool>> predicate)
        {
            var data = _dbSet.Where(predicate).ToList();
            var type = data.Count == 0 ? ResultType.BadRequest : ResultType.Success;

            var result = new OperationResult<List<T>>
            {
                Data = data,
                Type = type
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
