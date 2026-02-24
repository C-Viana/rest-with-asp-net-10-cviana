using Microsoft.OpenApi;
using System;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class OpenApiConfig
    {
        private static readonly string AppName = "Curso Erudio ASP.NET 2026 por Leandro Costa";
        private static readonly string AppDescription = $"REST API RESTful desenvolvido pelo {AppName}";
        private static readonly string DocVersion = "v1";

        public static IServiceCollection AddOpenApiConfig(this IServiceCollection services)
        {
            //services.AddEndpointsApiExplorer(); //Lookup on Program.cs
            
            services.AddSingleton(new OpenApiInfo
            {
                Title = AppName,
                Version = DocVersion,
                Description = AppDescription,
                Contact = new OpenApiContact
                {
                    Name = "Erudio",
                    Url = new Uri("https://erudio.com.br")
                },
                License = new OpenApiLicense{
                    Name = "MIT",
                    Url = new Uri("https://opensource.org/license")
                }
            });

            return services;
        }
    }
}
