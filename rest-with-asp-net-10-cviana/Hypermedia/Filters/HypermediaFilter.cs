using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using Serilog;

namespace rest_with_asp_net_10_cviana.Hypermedia.Filters
{
    public class HypermediaFilter(HypermediaFilterOptions hypermediaFilterOptions) : ResultFilterAttribute
    {
        private readonly HypermediaFilterOptions _hypermediaFilterOptions = hypermediaFilterOptions;

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await TryEnrichResult(context);
            //base.OnResultExecuting(context);
            await next();
        }

        private async Task TryEnrichResult(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult && (objectResult is OkObjectResult || objectResult is CreatedAtActionResult))
            {
                var enricher = _hypermediaFilterOptions.ContentResponseEnricherList.FirstOrDefault(options => options.CanEnrich(context));
                if (enricher != null)
                {
                    await enricher.Enrich(context);
                }
            }
        }

    }
}