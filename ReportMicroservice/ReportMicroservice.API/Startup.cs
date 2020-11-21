using AutoMapper;
using FluentValidation.AspNetCore;
using MassTransit;
using Microservice.Core.Constants.EnvironmentVariables;
using Microservice.Core.Enums;
using Microservice.Core.Infrastructure.Extensions;
using Microservice.Core.Infrastructure.Filters;
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
using ReportMicroservice.DAL.Models.SQLServer;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);

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

            services.AddConsulConfig(_configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportAPI Documentation" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportAPI Documentation");
            });
        }
    }
}
