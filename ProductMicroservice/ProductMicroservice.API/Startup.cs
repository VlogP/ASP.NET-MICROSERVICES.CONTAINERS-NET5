using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProductMicroservice.DAL.Models;
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

            currentDomain.LoadAssemblies(_configuration[MicroserviceEnvironmentVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceEnvironmentVariables.MICROSERVICE_BLL_NAME]);
            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<ProductDBContext>(o => {
                o.UseSqlServer(sqlServerUrl);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork<ProductDBContext>>();
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
                        config.Username(_configuration["RabbitMQ:Username"]);
                        config.Password(_configuration["RabbitMQ:Password"]);
                    });            
                });

                massTransitConfig.AddRequestClient<TestMessageRequest>();
            });

            services.AddMassTransitHostedService();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductAPI Documentation" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI Documentation");
            });
        }
    }
}
