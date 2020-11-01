using Microsoft.EntityFrameworkCore.Storage;
using TempateMicroservice.DAL.Models;
using TempateMicroservice.DAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TempateMicroservice.DAL.Infrastructure.UnitofWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly TempateDBContext _context;

        private IDbContextTransaction _objTran;

        private Dictionary<string, object> _repositories;

        public UnitOfWork(TempateDBContext context)
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
            var type = typeof(TRepository).Name;
            var isExist = _repositories.ContainsKey(type);

            if(!isExist)
            {             
                var repositoryInstance = Activator.CreateInstance(typeof(TRepository), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return _repositories[type] as TRepository;
        }
    }
}
