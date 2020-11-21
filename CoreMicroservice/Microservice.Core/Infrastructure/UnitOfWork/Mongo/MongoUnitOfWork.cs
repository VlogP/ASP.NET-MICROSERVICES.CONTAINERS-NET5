using Microservice.Core.Infrastructure.Mongo.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.UnitofWork.Mongo
{
    public class MongoUnitOfWork<TContext>: IMongoUnitOfWork where TContext : MongoDbContext
    {
        private readonly TContext _context;

        private IClientSessionHandle _clientSession;

        private Dictionary<string, object> _repositories;

        public MongoUnitOfWork(TContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, object>();
        }

        public void CreateSession()
        {
            _clientSession = _context.Database.Client.StartSession();
        }

        public async Task CreateSessionAsync()
        {
            _clientSession = await _context.Database.Client.StartSessionAsync();
        }

        public void CloseSession()
        {
            _clientSession.Dispose();
        }

        public async Task CloseSessionAsync()
        {
            await Task.Run(() => _clientSession.Dispose());
        }

        public void CreateTransaction()
        {
            _clientSession.StartTransaction();
        }

        public async Task CreateTransactionAsync()
        {
            await Task.Run(() => _clientSession.StartTransaction());
        }

        public void Rollback()
        {
            _clientSession.CommitTransaction();
            _clientSession.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _clientSession.CommitTransactionAsync();
            await Task.Run(() => _clientSession.Dispose());
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
