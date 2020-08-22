using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;

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
                });

            services.AddSwaggerForOcelot(_configuration);
            services.AddControllers();
            services.AddDistributedMemoryCache();
            services.AddOcelot(_configuration);
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
