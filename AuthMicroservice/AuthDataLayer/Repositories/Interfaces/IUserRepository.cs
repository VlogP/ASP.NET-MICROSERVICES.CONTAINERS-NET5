using AuthDataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthDataLayer.Repositories.Interfaces
{
    public interface IUserRepository: IBaseRepository<User>
    {
        public User GetUserWithRole(string email, string password);
    }
}
