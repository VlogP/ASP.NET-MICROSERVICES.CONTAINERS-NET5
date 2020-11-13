using Consul;
using Microservice.Messages.Constants.EnvironmentVariables;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Messages.Infrastructure.Extensions
{
    public static class ConsulExtension
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var consulAddress = configuration[MicroserviceEnvironmentVariables.CONSUL.CONSUL_URL];
            services.AddSingleton<IConsulClient, ConsulClient>(p =>
                new ConsulClient(consulConfig => {
                    consulConfig.Address = new Uri(consulAddress);
                }
            ));

            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var host = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.CONSUL.MICROSERVICE_HOST);
            var serviceName = Environment.GetEnvironmentVariable(MicroserviceEnvironmentVariables.CONSUL.CONSUL_SERVICE_NAME);

            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("ConsulLogger");
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            var uri = new Uri($"http://{host}");
            var id = $"{host}-{uri.Port}";
            var address = $"{uri.Host}";
            var registration = new AgentServiceRegistration()
            {
                ID = id,
                Name = serviceName,
                Address = address,
                Port = uri.Port
            };

            logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });

            return app;
        }
    }
}
