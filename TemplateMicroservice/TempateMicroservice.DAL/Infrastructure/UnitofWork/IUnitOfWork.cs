using TempateMicroservice.DAL.Repositories.Classes;
using TempateMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TempateMicroservice.DAL.Infrastructure.UnitofWork
{
    public interface IUnitOfWork
    {
        public void CreateTransaction();

        public Task CreateTransactionAsync();

        public void Rollback();

        public Task RollbackAsync();

        public void Save();

        public Task SaveAsync();

        public TRepository GetRepository<TRepository>() where TRepository : class;
    }
}
