using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Ocelot.Administration;
using Ocelot.Authentication.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

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

            //Validate JWT using JWT library
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            services
                .AddAuthentication(opt => 
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(authenticationProviderKey,opt => 
                {
                    opt.Authority = _configuration["IdentityServer:ServerURL"];
                    opt.Audience = _configuration["IdentityServer:UserApiName"];
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true
                    };
                });

            //Validate JWT using Identity Server 4 introspection endpoing
            /*services.AddDistributedMemoryCache();
            services.AddAuthentication(authenticationProviderKey)
                .AddOAuth2Introspection(authenticationProviderKey, opt =>
                {
                    opt.Authority = _configuration["IdentityServer:ServerURL"];
                    opt.ClientId = _configuration["IdentityServer:UserApiName"];
                    opt.ClientSecret = _configuration["IdentityServer:UserApiSecret"];
                    opt.CacheDuration = TimeSpan.FromMinutes(3600);
                    opt.EnableCaching = true;
                    opt.DiscoveryPolicy = new IdentityModel.Client.DiscoveryPolicy { 
                        RequireHttps = false, 
                        AdditionalEndpointBaseAddresses = 
                        new List<string> 
                            { 
                                _configuration["IdentityServer:BaseURL"] 
                            } 
                    };
                });*/

            services.AddSwaggerForOcelot(_configuration);
            services.AddControllers();
            services.AddOcelot().AddAdministration("/administration","12345");
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";               
            });

            await app.UseOcelot();     
        }
    }
}
