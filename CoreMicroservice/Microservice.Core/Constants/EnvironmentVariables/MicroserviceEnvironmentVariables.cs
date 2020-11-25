using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Constants.EnvironmentVariables
{
    public static class MicroserviceEnvironmentVariables
    {
        public static class RabbitMQ
        {
            public const string RABBITMQ_HOST = "RABBITMQ_HOST";
        }

        public static class Consul
        {
            public const string MICROSERVICE_HOST = "MICROSERVICE_HOST";

            public const string CONSUL_SERVICE_NAME = "CONSUL_SERVICE_NAME";
        }
    }
}
