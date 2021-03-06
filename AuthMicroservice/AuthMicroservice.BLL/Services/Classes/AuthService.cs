using AuthMicroservice.BLL.Models.DTO.Token;
using AuthMicroservice.BLL.Models.DTO.User;
using AuthMicroservice.BLL.Models.User;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.DAL.Infrastructure.Enums;
using AuthMicroservice.DAL.Infrastructure.PasswordHasher;
using AuthMicroservice.DAL.Models.SQLServer;
using AuthMicroservice.DAL.Repositories.SQLServer.Interfaces;
using AutoMapper;
using IdentityModel.Client;
using MailKit.Net.Smtp;
using Microservice.Core.Constants.ConfigurationVariables;
using Microservice.Core.Constants.EnvironmentVariables;
using Microservice.Core.Infrastructure.OperationResult;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AuthMicroservice.BLL.Services.Classes
{
    public class AuthService : IAuthService
    {
        private IConfiguration _configuration;
        private readonly ISQLUnitOfWork _sqlUnitOfWork;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration, ISQLUnitOfWork sqlUnitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _sqlUnitOfWork = sqlUnitOfWork;
            _mapper = mapper;
        }

        public async Task<OperationResult<TokenDTO>> GetToken(UserLogin userInfo)
        {
            TokenResponse response = new TokenResponse();
            var isTokenReceived = false;
            var errorsList = new List<string>();
            var result = new OperationResult<TokenDTO>();
            var userRepository = _sqlUnitOfWork.GetRepository<IUserSQLServerRepository>();

            var dataResult = userRepository
                .GetUserWithRole(userInfo.Email, userInfo.Password);

            var user = dataResult.IsSuccess ? dataResult.Data : null;

            if (user != null && user.IsActivated)
            {
                using (var client = new HttpClient())
                {
                    response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                    {
                        Address = _configuration[MicroserviceConfigurationVariables.IdentityServer.ACCESS_TOKEN_URL],
                        ClientId = _configuration[MicroserviceConfigurationVariables.IdentityServer.USER_CLIENT_ID],
                        ClientSecret = _configuration[MicroserviceConfigurationVariables.IdentityServer.USER_CLIENT_SECRET],
                        Scope = "application.read application.write offline_access",
                        UserName = userInfo.Email,
                        Password = userInfo.Password
                    });

                    if (response.IsError)
                    {
                        errorsList.Add(response.ErrorDescription);
                    }
                }
            }

            isTokenReceived = !string.IsNullOrEmpty(response.AccessToken);

            if (isTokenReceived)
            {
                result.Data = new TokenDTO
                {
                    AccessToken = response.AccessToken,
                    RefreshToken = response.RefreshToken ?? "Refresh token",
                    ExpiresIn = response.ExpiresIn
                };

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
        
        public async Task<OperationResult<UserDTO>> Add(UserRegister newUser)
        {
            var userRepository = _sqlUnitOfWork.GetRepository<IUserSQLServerRepository>();
            var isUserExist = userRepository.Get(element => element.Email.Equals(newUser.Email)).Data.Count != 0;
            var result = new OperationResult<UserDTO>();

            if (!isUserExist)
            {
                var id = Guid.NewGuid();
                var activationHash = PasswordHasher.ActivationHash();
                var passwordSalt = PasswordHasher.Hash(newUser.Password);
                var user = _mapper.Map<User>(newUser);

                user.Id = id;
                user.RoleId = (int)UserRole.User;
                user.Password = passwordSalt.Hash;
                user.Salt = passwordSalt.Salt;
                user.ActivationCode = activationHash;

                var userResult = userRepository.Add(user);
                _sqlUnitOfWork.Save();
                var userData = userRepository.GetUserWithRole(userResult.Data.Email);

                result.Errors = userResult.Errors;
                result.Data = _mapper.Map<UserDTO>(userData.Data);
                result.Type = userResult.Type;

                if (userResult.IsSuccess)
                {
                    await SendEmailAsync(userData.Data.Email, userData.Data.ActivationCode);
                }
            }
            else
            {
                result.Errors = new List<string> { "Email is already used" };
                result.Type = ResultType.BadRequest;
            }

            return result;
        }

        public OperationResult ActivateUser(string activationCode, string activationEmail)
        {
            var userRepository = _sqlUnitOfWork.GetRepository<IUserSQLServerRepository>();
            var result = new OperationResult();

            var userResult = userRepository.Get(item => item.Email.Equals(activationEmail));
            var user = userResult.Data.FirstOrDefault();
            var isActivationCodeCorrect = activationCode.Equals(user?.ActivationCode);

            if (isActivationCodeCorrect)
            {
                user.IsActivated = true;

                userRepository.Update(user);
                _sqlUnitOfWork.Save();

                result.Type = ResultType.Success;
            }
            else
            {
                result.Type = ResultType.BadRequest;
                result.Errors = new List<string> { "Activation code is not correct" };
            }

            return result;
        }

        private async Task SendEmailAsync(string email, string activationHash)
        {
            var fromName = "Microservice Project";
            var toName = "New User";
            var subject = "ASP Core Microservice Project";
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(fromName, _configuration[MicroserviceConfigurationVariables.Smtp.EMAIL]));
            emailMessage.To.Add(new MailboxAddress(toName, email));
            emailMessage.Subject = subject;

            var encodedActivationHash = HttpUtility.UrlEncode(activationHash);
            var encodedEmail = HttpUtility.UrlEncode(email);
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"https://localhost:11000/api/auth/activate?ActivationCode={encodedActivationHash}&ActivationEmail={encodedEmail}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration[MicroserviceConfigurationVariables.Smtp.SERVER], 465, true);
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                await client.AuthenticateAsync(_configuration[MicroserviceConfigurationVariables.Smtp.EMAIL], _configuration[MicroserviceConfigurationVariables.Smtp.PASSWORD]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
