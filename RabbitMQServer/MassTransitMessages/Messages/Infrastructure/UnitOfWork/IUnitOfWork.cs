﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Messages.Infrastructure.UnitofWork
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