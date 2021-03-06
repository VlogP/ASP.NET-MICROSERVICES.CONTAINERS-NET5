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
using FileMicroservice.API.Infrastructure.Automapper;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using FileMicroservice.DAL.Models.SQLServer;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microservice.Core.Constants.ConfigurationVariables;
using System.Collections.Generic;
using Microservice.Core.Infrastructure.OpenApi;
using Microservice.Core.Messages.FileReport;

namespace FileMicroservice.API
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
            var rabbitMQHost = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.RabbitMQ.RABBITMQ_HOST);

            currentDomain.LoadAssemblies(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME]);
            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddMassTransit(massTransitConfig =>
            {
                massTransitConfig.AddConsumers(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME]);

                massTransitConfig.UsingRabbitMq((context, rabbitConfig) => 
                {
                    rabbitConfig.Host(rabbitMQHost, config => 
                    {
                        config.Username(_configuration[MicroserviceConfigurationVariables.RabbitMQ.RABBITMQ_USER]);
                        config.Password(_configuration[MicroserviceConfigurationVariables.RabbitMQ.RABBITMQ_PASSWORD]);
                    });            
                });

                massTransitConfig.AddRequestClient<FileReportUploadRequest>();
                massTransitConfig.AddRequestClient<FileReportDownloadRequest>();
            });

            services.AddMassTransitHostedService();

            services.AddConsulConfig(_configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "FileAPI Documentation" });
                swagger.AddFluentValidationRules();
                swagger.AddSecurityDefinition("Bearer", OpenApiSecurity.OpenApiSecurityScheme);
                swagger.AddSecurityRequirement(OpenApiSecurity.OpenApiSecurityRequirement);
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileAPI Documentation");
            });
        }
    }
}
