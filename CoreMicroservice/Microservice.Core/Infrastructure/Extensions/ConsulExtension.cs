using Consul;
using Microservice.Core.Constants.EnvironmentVariables;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Core.Infrastructure.Extensions
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

        public static async Task<IApplicationBuilder> UseConsul(this IApplicationBuilder app)
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
            await consulClient.Agent.ServiceDeregister(registration.ID);
            await consulClient.Agent.ServiceRegister(registration);

            lifetime.ApplicationStopping.Register(async () =>
            {
                logger.LogInformation("Unregistering from Consul");
                await consulClient.Agent.ServiceDeregister(registration.ID);
            });

            return app;
        }
    }
}
