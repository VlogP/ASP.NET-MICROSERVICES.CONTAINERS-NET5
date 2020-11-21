using AuthMicroservice.BLL.Models.DTO.User;
using Microservice.Core.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthMicroservice.BLL.Services.Interfaces
{
    public interface IUserService
    {
        OperationResult<List<UserDTO>> GetUsers();
    }
}
