using AutoMapper;
using FluentValidation.AspNetCore;
using MassTransit;
using Microservice.Core.Constants.ConfigurationVariables;
using Microservice.Core.Constants.EnvironmentVariables;
using Microservice.Core.Enums;
using Microservice.Core.Infrastructure.Extensions;
using Microservice.Core.Infrastructure.Filters;
using Microservice.Core.Infrastructure.OpenApi;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReportMicroservice.API.Infrasrtucture.Automapper;
using ReportMicroservice.BLL.Consumers;
using ReportMicroservice.BLL.ResponseConsumers;
using ReportMicroservice.BLL.ResponseConsumers.FileReport;
using ReportMicroservice.DAL.Models.SQLServer;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace ReportMicroservice.API
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
            var sqlServerUrl = _configuration.GetConnectionString("SQLServerReportDB");

            currentDomain.LoadAssemblies(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME]);

            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<ReportSQLServerDBContext>(o => {
                o.UseSqlServer(sqlServerUrl);            
            });

            services.AddScoped<ISQLUnitOfWork,SQLUnitOfWork<ReportSQLServerDBContext>>();
            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);
            services.AddAutoMapper(typeof(AutomapperProfile));

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

                    rabbitConfig.ReceiveEndpoint("report-listener", endpoingConfig =>
                    {
                        endpoingConfig.ConfigureConsumer<ReportConsumer>(context);
                        endpoingConfig.ConfigureConsumer<ReportResponseConsumer>(context);
                    });

                    rabbitConfig.ReceiveEndpoint("filereport-listener", endpoingConfig =>
                    {
                        endpoingConfig.ConfigureConsumer<FileReportUploadResponseConsumer>(context);
                        endpoingConfig.ConfigureConsumer<FileReportDownloadResponseConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddConsulConfig(_configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportAPI Documentation" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportAPI Documentation");
            });
        }
    }
}
