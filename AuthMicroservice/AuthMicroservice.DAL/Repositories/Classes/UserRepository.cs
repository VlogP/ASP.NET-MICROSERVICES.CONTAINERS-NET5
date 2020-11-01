using AuthMicroservice.DAL.Models;
using AuthMicroservice.DAL.Repositories.Interfaces;
using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthMicroservice.DAL.Repositories.Classes
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private DbSet<User> _users;

        public UserRepository(AuthDBContext context) : base(context)
        {
            _users = context.Users;
        }

        public OperationResult<User> GetUserWithRole(string email, string password)
        {
           var data = _users
                .Include(option => option.Role)
                .FirstOrDefault(option => option.Email.Equals(email) && option.Password.Equals(password));

            var result = new OperationResult<User>
            {
                Data = data,
                Type = ResultType.Success
            };

            return result;
        }
    }
}
