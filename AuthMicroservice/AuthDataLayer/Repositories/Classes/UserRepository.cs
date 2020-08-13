using AuthDataLayer.Models;
using AuthDataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuthDataLayer.Repositories.Classes
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private DbSet<User> _users;

        public UserRepository(AuthDBContext context) : base(context)
        {
            _users = context.Users;
        }

        public User GetUserWithRole(string email, string password)
        {
           var result = _users
                .Include(option => option.Role)
                .FirstOrDefault(option => option.Email.Equals(email) && option.Password.Equals(password));

            return result;
        }
    }
}
