﻿using AuthDataLayer.Models;
using AuthDataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AuthDataLayer.Repositories.Classes
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

        public T Add(T entity)
        {
            var result = _dbSet.Add(entity).Entity;
            _context.SaveChanges();

            return result;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public List<T> Get()
        {
            return _dbSet.ToList();
        }

        public List<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public T Update(T entity)
        {
            return _dbSet.Update(entity).Entity;
        }
    }
}
