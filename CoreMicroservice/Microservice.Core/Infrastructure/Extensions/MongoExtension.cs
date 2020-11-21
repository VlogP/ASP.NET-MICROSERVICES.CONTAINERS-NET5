using Microservice.Core.Infrastructure.Mongo.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Microservice.Core.Infrastructure.Extensions
{
    public static class MongoExtension
    {
        public static void AddMongoDbContext<TContext>(this IServiceCollection services, string connectionString, string databaseName = null) where TContext : MongoDbContext
        {
            var mongoUrl = new MongoUrl(connectionString);
            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());

            if (string.IsNullOrEmpty(databaseName))
            {
                databaseName = mongoUrl.DatabaseName;                
            }

            var isMongoServiceExist = services.FirstOrDefault(item => item.ServiceType.Equals(typeof(IMongoClient))) != null;

            if (!isMongoServiceExist)
            {
                var mongoClient = new MongoClient(mongoUrl);
                ConventionRegistry.Register("camelCase", pack, t => true);
                services.AddSingleton<IMongoClient>(mongoClient);
            }

            services.AddScoped(service => Activator.CreateInstance(
                typeof(TContext), service.GetRequiredService<IMongoClient>(), databaseName) as TContext
            );
        }
    }
}
