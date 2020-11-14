using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Messages.Constants.EnvironmentVariables
{
    public static class MicroserviceEnvironmentVariables
    {
        public const string MICROSERVICE_BLL_NAME = "MicroserviceBLLName";

        public const string MICROSERVICE_DAL_NAME = "MicroserviceDALName";

        public static class SMTP
        {
            public const string Email = "SMTP:Email";

            public const string Password = "SMTP:Password";

            public const string Server = "SMTP:Server";
        }

        public static class Rabbitmq
        {
            public const string RABBITMQ_HOST = "RABBITMQ_HOST";

            public const string RABBITMQ_USER = "RabbitMQ:Username";

            public const string RABBITMQ_PASSWORD = "RabbitMQ:Password";
        }

        public static class IdentityServer
        {
            public const string SERVER_URL = "IdentityServer:ServerURL";

            public const string BASE_URL = "IdentityServer:BaseURL";

            public const string USER_CLIENT_ID = "IdentityServer:UserClientId";

            public const string USER_CLIENT_SECRET = "IdentityServer:UserClientSecret";

            public const string USER_API_SECRET= "IdentityServer:UserApiSecret";

            public const string USER_API_NAME = "IdentityServer:UserApiName";

            public const string ACCESS_TOKEN_URL = "IdentityServer:AccessTokenUrl";

            public const string ISSUER_URL = "IdentityServer:IssuerUrl";
        }

        public static class CONSUL
        {
            public const string MICROSERVICE_HOST = "MICROSERVICE_HOST";

            public const string CONSUL_SERVICE_NAME = "CONSUL_SERVICE_NAME";

            public const string CONSUL_URL = "Consul:Server";
        }
    }
}
