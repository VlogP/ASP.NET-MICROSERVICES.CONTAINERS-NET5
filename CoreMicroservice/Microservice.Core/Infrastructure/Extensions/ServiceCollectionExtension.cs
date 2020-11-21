using Microservice.Core.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microservice.Core.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services, string assemblyName, CommonClassName commonClassName)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(item => item.FullName.Contains(assemblyName));
            
            var types = assembly.GetTypes()
                .Where(item => item.Name.EndsWith(commonClassName.GetDisplayName()));

            foreach (var type in types)
            {
                var isNotInterface = !type.IsInterface;

                if (isNotInterface)
                {
                    var interfaceName = String.Concat("I", type.Name);
                    var interfaceType = assembly.GetTypes()
                        .FirstOrDefault(item => item.Name.Equals(interfaceName));

                    switch(commonClassName)
                    {
                        case CommonClassName.Repository:
                        {
                            services.AddScoped(interfaceType, type);
                            break;
                        }

                        case CommonClassName.Service:
                        {
                            services.AddTransient(interfaceType, type);
                            break;
                        }
                    }
                }
            }
        }
    }
}
