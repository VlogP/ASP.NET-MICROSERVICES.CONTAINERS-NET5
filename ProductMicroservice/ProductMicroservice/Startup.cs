using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProductDataLayer.Models;
using Microsoft.EntityFrameworkCore;
using BuisnessLogicLayer.Services.Interfaces;
using BuisnessLogicLayer.Services.Classes;
using ProductDataLayer.Repositories.Interfaces;
using ProductDataLayer.Repositories.Classes;
using Microsoft.OpenApi.Models;
using MassTransit;
using System;

namespace ProductMicroservice
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
            services.AddControllers();
            services.AddDbContext<ProductDBContext>(o => o.UseSqlServer(_configuration.GetConnectionString("SQLServerProductDB")));
            services.AddTransient<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context,cfg) => {
                    cfg.Host("rabbitmq://rabbitmqserver");            
                });
            });

            services.AddMassTransitHostedService();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "APIProduct documentation" });
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
