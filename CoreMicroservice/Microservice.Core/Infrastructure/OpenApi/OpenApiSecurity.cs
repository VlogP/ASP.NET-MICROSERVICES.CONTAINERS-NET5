using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservice.Core.Infrastructure.OpenApi
{
    public static class OpenApiSecurity
    {
        static OpenApiSecurity()
        {
            OpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            };

            OpenApiSecurityRequirement = new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            };
        }


        public static OpenApiSecurityScheme OpenApiSecurityScheme { get; set; }

        public static OpenApiSecurityRequirement OpenApiSecurityRequirement { get; set; }

    }
}
