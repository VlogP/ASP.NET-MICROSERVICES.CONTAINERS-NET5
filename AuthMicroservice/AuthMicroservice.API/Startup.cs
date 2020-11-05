using System;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microservice.Messages.Infrastructure.Extensions;
using Microservice.Messages.Constants.EnvironmentVariables;
using Microservice.Messages.Enums;
using Microservice.Messages.Infrastructure.Filters;
using Microservice.Messages.Infrastructure.UnitofWork;
using AutoMapper;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using AuthMicroservice.DAL.Models;
using AuthMicroservice.API.Infrastructure.IdentityServer;
using AuthMicroservice.API.Infrasrtucture.Automapper;

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
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
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

            services.AddScoped<IUnitOfWork, UnitOfWork<AuthDBContext>>();
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);
            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthAPI Documentation" });
                swagger.AddFluentValidationRules();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthAPI Documentation");
            });
        }
    }
}
