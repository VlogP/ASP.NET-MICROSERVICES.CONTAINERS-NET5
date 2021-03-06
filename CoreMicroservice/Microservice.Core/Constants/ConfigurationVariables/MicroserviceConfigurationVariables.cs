using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Constants.ConfigurationVariables
{
    public static class MicroserviceConfigurationVariables
    {
        public const string MICROSERVICE_BLL_NAME = "MicroserviceBLLName";

        public const string MICROSERVICE_DAL_NAME = "MicroserviceDALName";

        public static class Smtp
        {
            public const string EMAIL = "SMTP:Email";

            public const string PASSWORD = "SMTP:Password";

            public const string SERVER = "SMTP:Server";
        }

        public static class RabbitMQ
        {
            public const string RABBITMQ_USER = "RabbitMQ:Username";

            public const string RABBITMQ_PASSWORD = "RabbitMQ:Password";
        }

        public static class IdentityServer
        {
            public const string SERVER_URL = "IdentityServer:ServerURL";

            public const string BASE_URL = "IdentityServer:BaseURL";

            public const string USER_CLIENT_ID = "IdentityServer:UserClientId";

            public const string USER_CLIENT_SECRET = "IdentityServer:UserClientSecret";

            public const string USER_API_SECRET = "IdentityServer:UserApiSecret";

            public const string USER_API_NAME = "IdentityServer:UserApiName";

            public const string ACCESS_TOKEN_URL = "IdentityServer:AccessTokenUrl";

            public const string ISSUER_URL = "IdentityServer:IssuerUrl";

            public const string CERTIFICATE_PATH = "IdentityServer:CertificatePath";

            public const string CERTIFICATE_PASSWORD = "IdentityServer:CertificatePassword";
        }

        public static class Consul
        {

            public const string CONSUL_URL = "Consul:Server";
        }

        public static class ElasticSearch
        {
            public const string SERVER = "ElasticSearch:Server";

            public const string INDEX = "ElasticSearch:Index";

            public const string USERNAME = "ElasticSearch:Username";

            public const string PASSWORD = "ElasticSearch:Password";
        }

        public static class Google
        {
            public const string CREDENTIALS = "Google:Credentials";

            public const string STORAGE_PATH = "Google:Storage";

            public const string USERNAME = "Google:Username";

            public const string APPLICATION = "Google:ApplicationName";

            public const string PARRENT_FOLDER_ID = "Google:FolderId";
        }
    }
}
