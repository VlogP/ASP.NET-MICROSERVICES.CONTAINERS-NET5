using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthBuisnessLayer.Services.Classes;
using AuthBuisnessLayer.Services.Interfaces;
using AuthDataLayer.Models;
using AuthDataLayer.Repositories.Classes;
using AuthDataLayer.Repositories.Interfaces;
using AuthMicroservice.Utilities.IdentityServer;
using IdentityServer4.Configuration;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AuthMicroservice
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
            services.AddControllers();
            services.AddDbContext<AuthDBContext>(o => o.UseSqlServer(_configuration.GetConnectionString("SQLServerAuthDB")));

            services.AddIdentityServer(opt =>
                {
                    //opt.IssuerUri = "http://localhost:59118/";
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetAllApiResources(_configuration))
                .AddInMemoryApiScopes(Config.GetAllScopes())
                .AddInMemoryClients(Config.GetClients(_configuration))
                .AddProfileService<ProfileService>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "APIAuth documentation" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIAuth documentation");
            });
        }
    }
}
