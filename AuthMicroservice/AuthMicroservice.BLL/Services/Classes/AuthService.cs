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
using Microservice.Messages.Constants.EnvironmentVariables;
using Microservice.Messages.Infrastructure.OperationResult;
using Microservice.Messages.Infrastructure.UnitofWork;
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
            var userRepository = _unitOfWork.GetRepository<IUserSQLServerRepository>();

            var dataResult = userRepository
                .GetUserWithRole(userInfo.Email, userInfo.Password);

            var user = dataResult.Type == ResultType.Success ? dataResult.Data : null;

            if (user != null && user.IsActivated)
            {
                var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = _configuration[MicroserviceEnvironmentVariables.IdentityServer.ACCESS_TOKEN_URL],
                    ClientId = _configuration[MicroserviceEnvironmentVariables.IdentityServer.USER_CLIENT_ID],
                    ClientSecret = _configuration[MicroserviceEnvironmentVariables.IdentityServer.USER_CLIENT_SECRET],
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
        
        public async Task<OperationResult<UserDTO>> Add(UserRegister newUser)
        {
            var userRepository = _unitOfWork.GetRepository<IUserSQLServerRepository>();
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
                _unitOfWork.Save();
                var userData = userRepository.GetUserWithRole(userResult.Data.Email);

                result.Errors = userResult.Errors;
                result.Data = _mapper.Map<UserDTO>(userData.Data);
                result.Type = userResult.Type;

                var isSuccess = userResult.Type == ResultType.Success;

                if (isSuccess)
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

        public OperationResult<object> ActivateUser(string activationCode, string activationEmail)
        {
            var userRepository = _unitOfWork.GetRepository<IUserSQLServerRepository>();
            var result = new OperationResult<object>();

            var userResult = userRepository.Get(item => item.Email.Equals(activationEmail));
            var user = userResult.Data.FirstOrDefault();
            var isActivationCodeCorrect = activationCode.Equals(user?.ActivationCode);

            if (isActivationCodeCorrect)
            {
                user.IsActivated = true;

                userRepository.Update(user);
                _unitOfWork.Save();

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

            emailMessage.From.Add(new MailboxAddress(fromName, _configuration[MicroserviceEnvironmentVariables.SMTP.Email]));
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
                await client.ConnectAsync(_configuration[MicroserviceEnvironmentVariables.SMTP.Server], 465, true);
                client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                await client.AuthenticateAsync(_configuration[MicroserviceEnvironmentVariables.SMTP.Email], _configuration[MicroserviceEnvironmentVariables.SMTP.Password]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
