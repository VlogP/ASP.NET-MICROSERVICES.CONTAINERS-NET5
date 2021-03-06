using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Infrastructure.IdentityServer
{
    public class ProfileService : IProfileService
    {

        public ProfileService()
        {
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var claims = context.Subject.Claims;

                context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
            }
            catch (Exception ex)
            {
                
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                context.IsActive = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
