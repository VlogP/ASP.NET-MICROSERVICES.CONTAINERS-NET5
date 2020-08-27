using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ReportBuisnessLayer.Consumers;
using ReportBuisnessLayer.Services.Classes;
using ReportBuisnessLayer.Services.Interfaces;
using ReportDataLayer.Models;
using ReportDataLayer.Repositories.Classes;
using ReportDataLayer.Repositories.Interfaces;
using System;

namespace ReportMicroservice
{
    public class Startup
    {
        private IConfiguration _configuration { get; }

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ReportDBContext>(o => o.UseSqlServer(_configuration.GetConnectionString("SQLServerReportDB")));
            services.AddTransient<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ReportConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://rabbitmqserver");

                    cfg.ReceiveEndpoint("report-listener", e =>
                    {
                        e.ConfigureConsumer<ReportConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "APIReport documentation" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
