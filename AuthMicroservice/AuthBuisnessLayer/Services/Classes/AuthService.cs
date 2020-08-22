using AuthBuisnessLayer.Models;
using AuthBuisnessLayer.Services.Interfaces;
using AuthDataLayer.Repositories.Interfaces;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthBuisnessLayer.Services.Classes
{
    public class AuthService : IAuthService
    {
        private IConfiguration _configuration;
        private IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<string> GetToken(UserLoginModelBL userInfo)
        {
            var client = new HttpClient();
            var token = "";

            var user = _userRepository
                .Get(user => user.Email.Equals(userInfo.Email) && user.Password.Equals(userInfo.Password))
                .FirstOrDefault();

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

                token = response.AccessToken;
            }

            return token;
        }
    }
}
