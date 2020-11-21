using AuthMicroservice.DAL.Models;
using AuthMicroservice.DAL.Models.SQLServer;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.SQLBaseRepository;
using System.Collections.Generic;

namespace AuthMicroservice.DAL.Repositories.SQLServer.Interfaces
{
    public interface IUserSQLServerRepository: ISQLBaseRepository<User>
    {
        public OperationResult<User> GetUserWithRole(string email, string password);

        public OperationResult<User> GetUserWithRole(string email);

        public OperationResult<List<User>> GetUsersWithRole();

    }
}
