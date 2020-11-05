using AuthMicroservice.DAL.Models;
using Microservice.Messages.Infrastructure.OperationResult;
using System.Collections.Generic;

namespace AuthMicroservice.DAL.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        public OperationResult<User> GetUserWithRole(string email, string password);

        public OperationResult<User> GetUserWithRole(string email);

        public OperationResult<List<User>> GetUsersWithRole();

    }
}
