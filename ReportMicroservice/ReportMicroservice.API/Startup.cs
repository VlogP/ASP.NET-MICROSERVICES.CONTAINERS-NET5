using MassTransit;
using MassTransit.RabbitMqTransport;
using Microservice.Messages.Constants.EnvironmentVariables;
using Microservice.Messages.Enums;
using Microservice.Messages.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReportMicroservice.API.Infrastructure.Filters;
using ReportMicroservice.BLL.Consumers;
using ReportMicroservice.BLL.ResponseConsumers;
using ReportMicroservice.BLL.Services.Classes;
using ReportMicroservice.BLL.Services.Interfaces;
using ReportMicroservice.DAL.Models;
using ReportMicroservice.DAL.Repositories.Classes;
using ReportMicroservice.DAL.Repositories.Interfaces;
using System;
using System.Linq;
using System.Reflection;

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
            var rabbitMQHost = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.RABBITMQ_HOST);
            var sqlServerUrl = _configuration.GetConnectionString("SQLServerReportDB");

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);

            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
            });

            services.AddDbContext<ReportDBContext>(o => { 
                o.UseSqlServer(sqlServerUrl);            
            });
           
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);

            services.AddMassTransit(massTransitConfig =>
            {
                massTransitConfig.AddConsumers(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);

                massTransitConfig.UsingRabbitMq((context, rabbitConfig) =>
                {
                    rabbitConfig.Host(rabbitMQHost, config =>
                    {
                        config.Username(_configuration["RabbitMQ:Username"]);
                        config.Password(_configuration["RabbitMQ:Password"]);
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
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "APIReport documentation" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIReport documentation");
            });
        }
    }
}
