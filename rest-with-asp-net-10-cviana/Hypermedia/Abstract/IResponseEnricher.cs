using Microsoft.AspNetCore.Mvc.Filters;

namespace rest_with_asp_net_10_cviana.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        bool CanEnrich(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}