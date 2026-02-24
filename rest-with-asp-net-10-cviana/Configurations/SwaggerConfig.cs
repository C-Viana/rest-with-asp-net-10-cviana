using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class SwaggerConfig
    {
        // https://localhost:7244/swagger-ui/index.html
        private static readonly string AppName = "Curso Erudio ASP.NET 2026 por Leandro Costa";
        private static readonly string AppDescription = $"REST API RESTful desenvolvido pelo {AppName}";
        private static readonly string DocVersion = "v1";

        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(DocVersion, new OpenApiInfo
                {
                    Title = AppName,
                    Version = DocVersion,
                    Description = AppDescription,
                    Contact = new OpenApiContact
                    {
                        Name = "Erudio",
                        Email = "dontcontactit@unknown.com",
                        Url = new Uri("https://erudio.com.br")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/license")
                    }
                });
                config.CustomSchemaIds(type => type.FullName);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerSpecification(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/"+ DocVersion + "/swagger.json", DocVersion);
                config.RoutePrefix = "swagger-ui";
                config.DocumentTitle = AppName;
            });
            return app;
        }
    }
}
