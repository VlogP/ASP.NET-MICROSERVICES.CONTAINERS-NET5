using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Authentication.Middleware;
using Ocelot.Authorisation.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace APIGateway
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationProviderKey = "token";

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(authenticationProviderKey, opt => 
                {
                    opt.Authority = _configuration["IdentityServer:URL"];
                    opt.ApiName = _configuration["IdentityServer:UserApiName"];
                    opt.ApiSecret = _configuration["IdentityServer:UserApiSecret"];
                    opt.SupportedTokens = SupportedTokens.Both;
                    opt.RequireHttpsMetadata = false;
                    opt.IntrospectionDiscoveryPolicy = new IdentityModel.Client.DiscoveryPolicy { ValidateIssuerName = false };
                });

            services.AddSwaggerForOcelot(_configuration);
            services.AddControllers();
            services.AddOcelot(_configuration);
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";               
            });

            await app.UseOcelot();     
        }
    }
}
