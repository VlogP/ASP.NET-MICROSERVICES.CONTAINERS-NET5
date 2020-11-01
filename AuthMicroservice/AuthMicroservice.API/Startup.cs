using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthMicroservice.BLL.Services.Classes;
using AuthMicroservice.BLL.Services.Interfaces;
using AuthMicroservice.DAL.Models;
using AuthMicroservice.DAL.Repositories.Classes;
using AuthMicroservice.DAL.Repositories.Interfaces;
using AuthMicroservice.API.Infrastructure.IdentityServer;
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
using Microservice.Messages.Infrastructure.Extensions;
using Microservice.Messages.Constants.EnvironmentVariables;
using Microservice.Messages.Enums;
using AuthMicroservice.API.Infrastructure.Filters;
using AuthMicroservice.DAL.Infrastructure.UnitofWork;

namespace AuthMicroservice.API
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
            var currentDomain = AppDomain.CurrentDomain;

            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
            });

            services.AddDbContext<AuthDBContext>(o => {
                o.UseSqlServer(_configuration.GetConnectionString("SQLServerAuthDB"));
            });

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);

            services.AddIdentityServer(opt =>
                {
                    opt.IssuerUri = _configuration["IdentityServer:IssuerUrl"];
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryApiResources(Config.GetAllApiResources(_configuration))
                .AddInMemoryApiScopes(Config.GetAllScopes())
                .AddInMemoryClients(Config.GetClients(_configuration))
                .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);

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
