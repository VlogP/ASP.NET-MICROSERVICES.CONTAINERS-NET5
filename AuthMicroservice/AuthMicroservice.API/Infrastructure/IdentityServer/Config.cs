using IdentityModel;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Models;
using Microservice.Core.Constants.ConfigurationVariables;
using Microservice.Core.Constants.EnvironmentVariables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Infrastructure.IdentityServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetAllApiResources(IConfiguration configuration)
        {
            return new List<ApiResource>
            {
                new ApiResource {
                    Name = configuration[MicroserviceConfigurationVariables.IdentityServer.USER_API_NAME],
                    DisplayName = "User data resource",
                    ApiSecrets = { new Secret(configuration["IdentityServer:UserApiSecret"].Sha256()) },
                    Scopes = 
                    {
                        "application.read",
                        "application.write" 
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.Role,
                        JwtClaimTypes.Id
                    }
                }
            };
        }

        public static IEnumerable<ApiScope> GetAllScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("application.read","Read permission"),
                new ApiScope("application.write","Write permission"),
            };
        }

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = configuration[MicroserviceConfigurationVariables.IdentityServer.USER_CLIENT_ID],
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret(configuration[MicroserviceConfigurationVariables.IdentityServer.USER_CLIENT_SECRET].Sha256())
                    },
                    AllowedScopes = 
                    {
                        "application.read",
                        "application.write"
                    },                   
                    AccessTokenLifetime = 100000,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AllowOfflineAccess = true
                }
            };
        }
    }
}
