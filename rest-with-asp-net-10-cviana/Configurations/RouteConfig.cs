namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class RouteConfig
    {
        public static IServiceCollection AddRouteConfig(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });
            return services;
        }
    }
}
