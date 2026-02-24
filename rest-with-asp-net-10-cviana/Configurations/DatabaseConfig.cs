using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rest_with_asp_net_10_cviana.Models.Context;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["MSSQLServerConnection:MSSQLServerConnectionString"];

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("Connection string \"MSSQLServerConnection:MSSQLServerConnectionString\" is null or empty");

            services.AddDbContext<MSSQLContext>(options => options.UseSqlServer(connectionString));
            return services;
        }
    }
}
