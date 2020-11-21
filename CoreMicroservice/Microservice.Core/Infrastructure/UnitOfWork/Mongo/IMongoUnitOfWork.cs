using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.UnitofWork.Mongo
{
    public interface IMongoUnitOfWork
    {
        public void CreateSession();

        public Task CreateSessionAsync();

        public void CloseSession();

        public Task CloseSessionAsync();

        public void CreateTransaction();

        public Task CreateTransactionAsync();

        public void Rollback();

        public Task RollbackAsync();

        public TRepository GetRepository<TRepository>() where TRepository : class;
    }
}
