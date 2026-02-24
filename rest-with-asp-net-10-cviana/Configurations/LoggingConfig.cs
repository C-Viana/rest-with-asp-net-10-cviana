using Serilog;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class LoggingConfig
    {
        public static void AddSeriLogging(this WebApplicationBuilder builder) {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
            builder.Host.UseSerilog();
        }
    }
}
