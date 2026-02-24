using Scalar.AspNetCore;
using System.Runtime.CompilerServices;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class ScalarConfig
    {
        // https://localhost:7244/scalar
        private static readonly string _appName = "Curso Erudio ASP.NET 2026 por Leandro Costa";
        //private static readonly string AppDescription = $"REST API RESTful desenvolvido pelo {_appName}";
        //private static readonly string DocVersion = "v1";

        public static WebApplication UseScalarSpecification(this WebApplication app)
        {
            app.MapScalarApiReference("/scalar", options =>
                {
                    options.WithTitle(_appName);
                    options.WithOpenApiRoutePattern("/swagger/v1/swagger.json");
                }
            );
            return app;
        }
    }
}
