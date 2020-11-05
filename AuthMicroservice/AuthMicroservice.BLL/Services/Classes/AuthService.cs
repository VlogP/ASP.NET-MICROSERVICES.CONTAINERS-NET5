using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Models.User;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.DAL.Infrastructure.Enums;
using AuthMicroservice.DAL.Infrastructure.PasswordHasher;
using AuthMicroservice.DAL.Models;
using AuthMicroservice.DAL.Repositories.Interfaces;
using AutoMapper;
using IdentityModel.Client;
using Microservice.Messages.Infrastructure.OperationResult;
using Microservice.Messages.Infrastructure.UnitofWork;
using Microsoft.Extensions.Configuration;
using System;
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
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<string>> GetToken(UserLogin userInfo)
        {
            var client = new HttpClient();
            var token = "";
            var isTokenReceived = false;
            var errorsList = new List<string>();
            var result = new OperationResult<string>();
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();

            var dataResult = userRepository
                .GetUserWithRole(userInfo.Email, userInfo.Password);

            var user = dataResult.Type == ResultType.Success ? dataResult.Data : null;

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
        
        public OperationResult<UserDTO> Add(UserRegister newUser)
        {
            var userRepository = _unitOfWork.GetRepository<IUserRepository>();
            var isUserExist = userRepository.Get(element => element.Email.Equals(newUser.Email)).Data.Count != 0;
            var result = new OperationResult<UserDTO>();

            if (!isUserExist)
            {
                var passwordSalt = PasswordHasher.Hash(newUser.Password);
                var user = _mapper.Map<User>(newUser);
                user.Id = Guid.NewGuid();
                user.RoleId = (int)UserRole.User;
                user.Password = passwordSalt.Hash;
                user.Salt = passwordSalt.Salt;

                var userResult = userRepository.Add(user);
                _unitOfWork.Save();
                var userData = userRepository.GetUserWithRole(userResult.Data.Email);

                result.Errors = userResult.Errors;
                result.Data = _mapper.Map<UserDTO>(userData.Data);
                result.Type = userResult.Type;
            }
            else
            {
                result.Errors = new List<string> { "Email is already used" };
                result.Type = ResultType.BadRequest;
            }

            return result;
        }
    }
}
