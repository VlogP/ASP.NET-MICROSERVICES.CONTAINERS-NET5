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
using Microservice.Core.Infrastructure.Extensions;
using Microservice.Core.Constants.EnvironmentVariables;
using Microservice.Core.Enums;
using Microservice.Core.Infrastructure.Filters;
using AutoMapper;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using AuthMicroservice.API.Infrastructure.IdentityServer;
using AuthMicroservice.API.Infrasrtucture.Automapper;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using AuthMicroservice.DAL.Models.SQLServer;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microservice.Core.Constants.ConfigurationVariables;

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
            var openApiSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };
            var openApiSecurityRequirement = new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            };

            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<AuthSQLServerDbContext>(o => {
                o.UseSqlServer(_configuration.GetConnectionString("SQLServerAuthDB"));
            });

            currentDomain.LoadAssemblies(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME]);

            services.AddIdentityServer(opt =>
                {
                    opt.IssuerUri = _configuration[MicroserviceConfigurationVariables.IdentityServer.ISSUER_URL];
                })
                .AddSigningCredential(new X509Certificate2(_configuration[MicroserviceConfigurationVariables.IdentityServer.CERTIFICATE_PATH], _configuration[MicroserviceConfigurationVariables.IdentityServer.CERTIFICATE_PASSWORD]))
                .AddInMemoryApiResources(Config.GetAllApiResources(_configuration))
                .AddInMemoryApiScopes(Config.GetAllScopes())
                .AddInMemoryClients(Config.GetClients(_configuration))
                .AddProfileService<ProfileService>();

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddTransient<IProfileService, ProfileService>();

            services.AddScoped<ISQLUnitOfWork, SQLUnitOfWork<AuthSQLServerDbContext>>();
            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);
            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthAPI Documentation" });
                swagger.AddFluentValidationRules();
                swagger.AddSecurityDefinition("Bearer", openApiSecurityScheme);
                swagger.AddSecurityRequirement(openApiSecurityRequirement);
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
