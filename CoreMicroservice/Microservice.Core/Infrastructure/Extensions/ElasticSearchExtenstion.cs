using Microservice.Core.Constants.ConfigurationVariables;
using Microservice.Core.Enums;
using Microservice.Core.Infrastructure.ElasticSearch.Index;
using Microservice.Core.Infrastructure.ElasticSearch.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Linq;
using ConnectionSettings = Nest.ConnectionSettings;

namespace Microservice.Core.Infrastructure.Extensions
{
    public static class ElasticSearchExtenstion
    {
        public static void AddElasticsearchClient(this IServiceCollection services, IConfiguration configuration, string mappingAssemblyName)
        {
            var url = configuration[MicroserviceConfigurationVariables.ElasticSearch.SERVER];
            var defaultIndex = configuration[MicroserviceConfigurationVariables.ElasticSearch.INDEX];
            var user = configuration[MicroserviceConfigurationVariables.ElasticSearch.USERNAME];
            var password = configuration[MicroserviceConfigurationVariables.ElasticSearch.PASSWORD];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .BasicAuthentication(user, password)
                .ThrowExceptions();

            var client = new ElasticClient(settings);

            AddElasticsearchMappings(client, mappingAssemblyName);
            services.AddSingleton(client);
        }

        public static void AddElasticsearchIndex<TIndex>(this IServiceCollection services, string indexName) where TIndex : class, IElasticSearchIndex
        {           
            services.AddScoped(service =>
                Activator.CreateInstance(typeof(TIndex), indexName) as TIndex
            );
        }

        private static void AddElasticsearchMappings(ElasticClient elasticClient, string mappingAssemblyName)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(item => item.FullName.Contains(mappingAssemblyName));

            var mappingTypes = assembly.GetTypes()
                .Where(item => item.Name.EndsWith(CommonClassName.ElasticSearchMapping.GetDisplayName()));

            foreach(var mappingType in mappingTypes)
            {
                if (!mappingType.IsInterface)
                {
                    var mapping = Activator.CreateInstance(mappingType) as IElasticSearchMapping;
                    try {
                        elasticClient.Indices.Create(mapping.IndexName, mapping.Map);
                    }
                    catch (Exception exception) {

                    }
                }
            }

        }
    }
}
