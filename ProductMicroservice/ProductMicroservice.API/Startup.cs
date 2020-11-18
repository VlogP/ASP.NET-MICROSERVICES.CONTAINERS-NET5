using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microservice.Messages.Constants.EnvironmentVariables;
using Microsoft.OpenApi.Models;
using MassTransit;
using System;
using Microservice.Messages.Messages.Test;
using Microservice.Messages.Enums;
using Microservice.Messages.Infrastructure.Extensions;
using Microservice.Messages.Infrastructure.Filters;
using Microservice.Messages.Infrastructure.UnitofWork;
using AutoMapper;
using ProductMicroservice.API.Infrasrtucture.Automapper;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using System.Collections.Generic;
using ProductMicroservice.DAL.Models.SQLServer;
using ProductMicroservice.DAL.Models.Mongo;

namespace ProductMicroservice.API
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
            var rabbitMQHost = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.Rabbitmq.RABBITMQ_HOST);
            var sqlServerUrl = _configuration.GetConnectionString("SQLServerProductDB");
            var openApiSecurityScheme = new OpenApiSecurityScheme {
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

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);
            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv => {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<ProductSQLServerDBContext>(o => {
                o.UseSqlServer(sqlServerUrl);
            });
            services.AddMongoDbContext<ProductMongoDBContext>(_configuration.GetConnectionString("MongoProductDB"));

            services.AddScoped<IUnitOfWork, UnitOfWork<ProductSQLServerDBContext>>();
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);
            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddMassTransit(massTransitConfig =>
            {
                massTransitConfig.AddConsumers(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);

                massTransitConfig.UsingRabbitMq((context, rabbitConfig) => 
                {
                    rabbitConfig.Host(rabbitMQHost, config => 
                    {
                        config.Username(_configuration[MicroserviceEnvironmentVariables.Rabbitmq.RABBITMQ_USER]);
                        config.Password(_configuration[MicroserviceEnvironmentVariables.Rabbitmq.RABBITMQ_PASSWORD]);
                    });            
                });

                massTransitConfig.AddRequestClient<TestMessageRequest>();
            });

            services.AddMassTransitHostedService();

            services.AddConsulConfig(_configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductAPI Documentation" });
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

            app.UseConsul().Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI Documentation");
            });
        }
    }
}
