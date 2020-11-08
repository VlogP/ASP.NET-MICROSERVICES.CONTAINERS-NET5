using AutoMapper;
using FluentValidation.AspNetCore;
using MassTransit;
using Microservice.Messages.Constants.EnvironmentVariables;
using Microservice.Messages.Enums;
using Microservice.Messages.Infrastructure.Extensions;
using Microservice.Messages.Infrastructure.Filters;
using Microservice.Messages.Infrastructure.UnitofWork;
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
using ReportMicroservice.DAL.Models;
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
            var rabbitMQHost = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.Rabbitmq.RABBITMQ_HOST);
            var sqlServerUrl = _configuration.GetConnectionString("SQLServerReportDB");

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);

            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<ReportDBContext>(o => {
                o.UseSqlServer(sqlServerUrl);            
            });

            services.AddScoped<IUnitOfWork,UnitOfWork<ReportDBContext>>();
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

                    rabbitConfig.ReceiveEndpoint("report-listener", endpoingConfig =>
                    {
                        endpoingConfig.ConfigureConsumer<ReportConsumer>(context);
                        endpoingConfig.ConfigureConsumer<ReportResponseConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportAPI Documentation" });
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

            app.UseAuthorization();

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
