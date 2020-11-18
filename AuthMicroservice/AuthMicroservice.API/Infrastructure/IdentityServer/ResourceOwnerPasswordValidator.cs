using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microservice.Messages.Infrastructure.UnitofWork;
using Microservice.Messages.Infrastructure.OperationResult;
using AuthMicroservice.DAL.Repositories.SQLServer.Interfaces;

namespace AuthMicroservice.API.Infrastructure.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResourceOwnerPasswordValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                var userRepository = _unitOfWork.GetRepository<IUserSQLServerRepository>();
                var user = userRepository
                    .GetUserWithRole(context.UserName, context.Password);

                var userClaims = new UserClaims
                {
                    Id = user?.Data.Id.ToString(),
                    Role = user?.Data.Role?.Name
                };

                if (user.Type == ResultType.Success)
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

        public static Claim[] GetUserClaims(UserClaims claims)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Id, claims.Id),
                new Claim(JwtClaimTypes.Role, claims.Role)
            };
        }
    }
}
