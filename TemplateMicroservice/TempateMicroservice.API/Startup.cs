using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microservice.Core.Constants.EnvironmentVariables;
using Microsoft.OpenApi.Models;
using MassTransit;
using System;
using Microservice.Core.Messages.Test;
using Microservice.Core.Enums;
using Microservice.Core.Infrastructure.Extensions;
using Microservice.Core.Infrastructure.Filters;
using AutoMapper;
using TempateMicroservice.API.Infrastructure.Automapper;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using TempateMicroservice.DAL.Models.SQLServer;
using Microservice.Core.Infrastructure.UnitofWork.SQL;

namespace TempateMicroservice.API
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

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);
            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<TemplateSQLServerDBContext>(o => {
                o.UseSqlServer(sqlServerUrl);
            });

            services.AddScoped<ISQLUnitOfWork, SQLUnitOfWork<TemplateSQLServerDBContext>>();
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

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "APIProduct documentation" });
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIProduct documentation");
            });
        }
    }
}
