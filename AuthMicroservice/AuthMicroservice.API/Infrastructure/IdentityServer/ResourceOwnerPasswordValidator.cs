using AuthMicroservice.BLL.Models;
using AuthMicroservice.DAL.Repositories.Interfaces;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Infrastructure.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserRepository _userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var user = _userRepository
                    .GetUserWithRole(context.UserName, context.Password);

                var userClaims = new UserClaimsModelBL
                {
                    Id = user?.Data.Id.ToString(),
                    Role = user?.Data.Role?.Name
                };

                if (user != null)
                {
                    context.Result = new GrantValidationResult(
                        subject: user.Data.Id.ToString(),
                        authenticationMethod: "password",
                        claims: GetUserClaims(userClaims)
                        );

                    return;
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Invalid username or password");
                return;
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Server error");
            }
        }

        public static Claim[] GetUserClaims(UserClaimsModelBL claims)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, claims.Id),
                new Claim(JwtClaimTypes.Role, claims.Role)
            };
        }
    }
}
