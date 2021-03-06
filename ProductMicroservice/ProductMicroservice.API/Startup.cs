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
using ProductMicroservice.API.Infrasrtucture.Automapper;
using Swashbuckle.AspNetCore.Swagger;
using FluentValidation.AspNetCore;
using System.Collections.Generic;
using ProductMicroservice.DAL.Models.SQLServer;
using ProductMicroservice.DAL.Models.Mongo;
using Microservice.Core.Infrastructure.UnitofWork.SQL;
using Microservice.Core.Infrastructure.UnitofWork.Mongo;
using Microservice.Core.Constants.ConfigurationVariables;
using System.Reflection;
using ProductMicroservice.DAL.Models.ElasticSearch;
using Microservice.Core.Constants.ElasticSearchIndexes;
using Microservice.Core.Infrastructure.OpenApi;

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
            var rabbitMQHost = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.RabbitMQ.RABBITMQ_HOST);
            var sqlServerUrl = _configuration.GetConnectionString("SQLServerProductDB");

            currentDomain.LoadAssemblies(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], _configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME]);
            services.AddControllers(opt => {
                opt.Filters.Add<ControllerExceptionFilter>();
                opt.Filters.Add<ControllerActionFilter>();
                opt.Filters.Add<ControllerResultFilter>();
            }).AddFluentValidation(fv => {
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddDbContext<ProductSQLServerDbContext>(o => {
                o.UseSqlServer(sqlServerUrl);
            });
            services.AddMongoDbContext<ProductMongoDbContext>(_configuration.GetConnectionString("MongoProductDB"));
            services.AddElasticsearchClient(_configuration, _configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME]);
            services.AddElasticsearchIndex<ProductElasticSearchIndex>(ElasticSearchIndexes.ProductIndex);

            services.AddScoped<ISQLUnitOfWork, SQLUnitOfWork<ProductSQLServerDbContext>>();
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork<ProductMongoDbContext>>();
            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_DAL_NAME], CommonClassName.Repository);
            services.AddServices(_configuration[MicroserviceConfigurationVariables.MICROSERVICE_BLL_NAME], CommonClassName.Service);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

                massTransitConfig.AddRequestClient<TestMessageRequest>();
            });

            services.AddMassTransitHostedService();

            services.AddConsulConfig(_configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductAPI Documentation" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductAPI Documentation");
            });
        }
    }
}
