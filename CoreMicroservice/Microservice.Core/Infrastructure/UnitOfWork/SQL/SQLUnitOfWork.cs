﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.UnitofWork.SQL
{
    public class SQLUnitOfWork<TContext>: ISQLUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;

        private IDbContextTransaction _objTran;

        private Dictionary<string, object> _repositories;

        public SQLUnitOfWork(TContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, object>();
        }

        public void CreateTransaction()
        {
            _objTran = _context.Database.BeginTransaction();
        }

        public async Task CreateTransactionAsync()
        {
            _objTran = await _context.Database.BeginTransactionAsync();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            var typeName = typeof(TRepository).Name;
            var isExist = _repositories.ContainsKey(typeName);

            if(!isExist)
            {
                var repositoryName = typeName.Substring(1);
                var assembly = Assembly.GetAssembly(typeof(TRepository));
                var repositoryType = assembly.ExportedTypes.FirstOrDefault(item => item.Name.Equals(repositoryName));
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);

                _repositories.Add(typeName, repositoryInstance);
            }

            return _repositories[typeName] as TRepository;
        }
    }
}
