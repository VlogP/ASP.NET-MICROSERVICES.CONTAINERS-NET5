using AuthMicroservice.DAL.Models;
using Microservice.Messages.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.DAL.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        public OperationResult<User> GetUserWithRole(string email, string password);
    }
}
