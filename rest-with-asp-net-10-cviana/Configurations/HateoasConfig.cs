using rest_with_asp_net_10_cviana.Hypermedia.Enricher;
using rest_with_asp_net_10_cviana.Hypermedia.Filters;

namespace rest_with_asp_net_10_cviana.Configurations
{
    public static class HateoasConfig
    {
        public static IServiceCollection AddHateoasConfiguration(this IServiceCollection services)
        {
            var filterOptions = new HypermediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
            services.AddSingleton(filterOptions);

            services.AddScoped<HypermediaFilter>();
            return services;
        }

        public static void UseHateoasRoutes(this IEndpointRouteBuilder app)
        {
            app.MapControllerRoute("Default", "{controller=values}/v1/{id?}");
        }
    }
}