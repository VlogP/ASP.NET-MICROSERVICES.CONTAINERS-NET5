using AuthMicroservice.BLL.Models;
using Microservice.Messages.Infrastructure.OperationResult;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthMicroservice.BLL.Services.Interfaces
{
    public interface IAuthService
    {
       Task<OperationResult<string>> GetToken(UserLoginModelBL userInfo);
    }
}
