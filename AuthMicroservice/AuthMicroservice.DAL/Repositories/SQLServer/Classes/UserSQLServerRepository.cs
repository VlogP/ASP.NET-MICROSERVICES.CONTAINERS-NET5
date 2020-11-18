using AuthMicroservice.DAL.Infrastructure.PasswordHasher;
using AuthMicroservice.DAL.Models.SQLServer;
using AuthMicroservice.DAL.Repositories.SQLServer.Interfaces;
using Microservice.Messages.Infrastructure.OperationResult;
using Microservice.Messages.Infrastructure.SQLBaseRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AuthMicroservice.DAL.Repositories.SQLServer.Classes
{
    public class UserSQLServerRepository : SQLBaseRepository<AuthSQLServerDbContext, User>, IUserSQLServerRepository
    {
        private DbSet<User> _users;

        public UserSQLServerRepository(AuthSQLServerDbContext context) : base(context)
        {
            _users = context.Users;
        }

        public OperationResult<User> GetUserWithRole(string email, string password)
        {
            var result = new OperationResult<User>();
            var data = _users
                .Include(option => option.Role)
                .FirstOrDefault(option => option.Email.Equals(email));

            var isEmailCorrect = data != null;
            var isPasswordCorrect = isEmailCorrect ? PasswordHasher.Check(data.Salt, data.Password, password) : false;

            if (isPasswordCorrect)
            {
                result.Data = data;
                result.Type = ResultType.Success;
            }
            else
            {
                result.Type = ResultType.BadRequest;
            }

            return result;
        }

        public OperationResult<User> GetUserWithRole(string email)
        {
            var result = new OperationResult<User>();
            var data = _users
                 .Include(option => option.Role)
                 .FirstOrDefault(option => option.Email.Equals(email));

            if (data != null)
            {
                result = new OperationResult<User>
                {
                    Data = data,
                    Type = ResultType.Success
                };
            }
            else
            {
                result = new OperationResult<User>
                {
                    Type = ResultType.BadRequest
                };
            }

            return result;
        }

        public OperationResult<List<User>> GetUsersWithRole()
        {
            var data = _users
                 .Include(option => option.Role)
                 .ToList();

            var result = new OperationResult<List<User>>
            {
                Data = data,
                Type = ResultType.Success
            };

            return result;
        }
    }
}
