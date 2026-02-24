using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class EvolveConfig
    {
        public static IServiceCollection AddEvolveConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            string AppSettingsDbConnectionProperty = "MSSQLServerConnection:MSSQLServerConnectionString";

            if (environment.IsDevelopment())
            {
                string evolveConnectionString = configuration[AppSettingsDbConnectionProperty];
                if (string.IsNullOrWhiteSpace(evolveConnectionString))
                {
                    throw new ArgumentNullException("Connection String \"" + AppSettingsDbConnectionProperty + "\" is either null or empty");
                }
                try
                {
                    ExecuteMigrations(evolveConnectionString);
                }
                catch (Exception ex)
                {
                    {
                        Log.Error(ex, "An error occurred while migrating the database");
                        throw;
                    }
                }
            }
            return services;
        }

        public static void ExecuteMigrations(string evolveConnectionString)
        {
            using var evolveConnection = new SqlConnection(evolveConnectionString);
            var evolve = new Evolve(evolveConnection, msg => Log.Information(msg))
            {
                Locations = ["db/migrations", "db/dataset"],
                IsEraseDisabled = true
            };
            evolve.Migrate();
        }
    }
}