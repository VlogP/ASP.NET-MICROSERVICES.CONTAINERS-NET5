using MassTransit.ExtensionsDependencyInjectionIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservice.Messages.Infrastructure.Extensions
{
    public static class MassTransitExtension
    {
        public static void AddConsumers(this IServiceCollectionBusConfigurator massTransitConfig, string assemblyName)
        {
            var consumerPostfix = "Consumer";
            var responseConsumerPostfix = "ResponseConsumer";

            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(item => item.FullName.Contains(assemblyName));

            var types = assembly.GetTypes()
                .Where(item => item.Name.EndsWith(consumerPostfix) || item.Name.EndsWith(responseConsumerPostfix));

            foreach (var type in types)
            {
                massTransitConfig.AddConsumer(type);
            }
        }
    }
}
