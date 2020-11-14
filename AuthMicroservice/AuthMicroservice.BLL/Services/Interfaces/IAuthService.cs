using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Models.User;
using Microservice.Messages.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthMicroservice.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<string>> GetToken(UserLogin userInfo);

        Task<OperationResult<UserDTO>> Add(UserRegister newUser);

        OperationResult<object> ActivateUser(string activationCode, string activationEmail);
    }
}
