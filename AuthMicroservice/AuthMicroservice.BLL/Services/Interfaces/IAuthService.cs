using AuthMicroservice.BLL.Models.DTO.Token;
using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Models.User;
using Microservice.Core.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthMicroservice.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<OperationResult<TokenDTO>> GetToken(UserLogin userInfo);

        Task<OperationResult<UserDTO>> Add(UserRegister newUser);

        OperationResult ActivateUser(string activationCode, string activationEmail);
    }
}
