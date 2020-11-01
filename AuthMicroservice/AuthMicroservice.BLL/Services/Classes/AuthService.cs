using AuthMicroservice.BLL.Models;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.DAL.Infrastructure.UnitofWork;
using AuthMicroservice.DAL.Repositories.Classes;
using AuthMicroservice.DAL.Repositories.Interfaces;
using IdentityModel.Client;
using Microservice.Messages.Infrastructure.OperationResult;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthMicroservice.BLL.Services.Classes
{
    public class AuthService : IAuthService
    {
        private IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult<string>> GetToken(UserLoginModelBL userInfo)
        {
            var client = new HttpClient();
            var token = "";
            var isTokenReceived = false;
            var errorsList = new List<string>();
            var result = new OperationResult<string>();
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var dataResult = userRepository
                .Get(user => user.Email.Equals(userInfo.Email) && user.Password.Equals(userInfo.Password));

            var user = dataResult?.Data.FirstOrDefault();

            if (user != null)
            {
                var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = _configuration["IdentityServer:AccessTokenUrl"],
                    ClientId = _configuration["IdentityServer:UserClientId"],
                    ClientSecret = _configuration["IdentityServer:UserClientSecret"],
                    Scope = "application.read application.write",
                    UserName = userInfo.Email,
                    Password = userInfo.Password
                });

                if (response.IsError)
                {
                    errorsList.Add(response.ErrorDescription);
                }

                token = response.AccessToken;
            }

            isTokenReceived = !string.IsNullOrEmpty(token);

            if (isTokenReceived)
            {
                result.Data = token;
                result.Type = ResultType.Success;                
            }
            else
            {
                errorsList.Add("Invalid username or password");
                result.Errors = errorsList;
                result.Type = ResultType.BadRequest;
            }

            return result;
        }
    }
}
