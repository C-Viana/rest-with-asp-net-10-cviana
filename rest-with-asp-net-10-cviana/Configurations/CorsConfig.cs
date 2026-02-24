using Microsoft.Extensions.Configuration;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class CorsConfig
    {
        private static string[] GetAllowedOrigins(IConfiguration configuration)
        {
            return configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
        }

        public static void AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy",
                    policy =>
                        policy
                        .WithOrigins(
                            GetAllowedOrigins(configuration)
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                );
                options.AddPolicy("LocalPolicy",
                    policy =>
                        policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        //.AllowAnyOrigin()
                );
            });
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IConfiguration configuration) {
            var origins = GetAllowedOrigins(configuration);

            app.Use(async (context, next) =>
            {
                var selfOrigin = $"{context.Request.Scheme}://{context.Request.Host}";
                //Mesmo que a origem não seja permitida, a requisição poderá ser executada se o header "Origin" informar uma URL válida
                var origin = context.Request.Headers["Origin"].ToString();
                if ( !string.IsNullOrEmpty(origin) && 
                    !origin.Equals(selfOrigin, StringComparison.OrdinalIgnoreCase) && 
                    !origins.Contains(origin, StringComparer.OrdinalIgnoreCase) )
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("CORS policy not allowed");
                    return;
                }
                await next();
            });

            app.UseCors("DefaultPolicy");
            return app;
        }
    }
}
