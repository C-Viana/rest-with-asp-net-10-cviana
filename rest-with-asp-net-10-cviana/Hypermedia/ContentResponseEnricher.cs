using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using rest_with_asp_net_10_cviana.Hypermedia.Abstract;
using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Hypermedia
{
    public abstract class ContentResponseEnricher<T> : IResponseEnricher where T : ISupportHypermedia
    {

        protected abstract Task EnrichModel(T content, IUrlHelper urlHelper);
        
        public virtual bool CanEnrich(Type contentType)
        {
            return contentType == typeof(T) 
                || contentType == typeof(List<T>)
                || contentType == typeof(PagedSearch<T>);
        }

        public bool CanEnrich(ResultExecutingContext response)
        {
            if(response.Result is OkObjectResult okObjectResult)
            {
                return CanEnrich(okObjectResult.Value?.GetType());
            }
            else if(response.Result is CreatedAtActionResult createdAtActionResult)
            {
                return CanEnrich(createdAtActionResult.Value?.GetType());
            }
            return false;
        }

        public async Task Enrich(ResultExecutingContext response)
        {
            var urlHelper = new UrlHelperFactory().GetUrlHelper(response);
            var responseType = response.Result;

            if (responseType is OkObjectResult okObjectResult)
            {
                if(okObjectResult.Value is T model)
                {
                    await EnrichModel(model, urlHelper);
                }
                else if (okObjectResult.Value is List<T> collection)
                {
                    foreach (var item in collection)
                    {
                        await EnrichModel(item, urlHelper);
                    }
                }
            }
            else if(responseType is CreatedAtActionResult createdAtActionResult)
            {
                if(createdAtActionResult.Value is T model)
                {
                    await EnrichModel(model, urlHelper);
                }
            }
            else if (responseType is PagedSearch<T> pagedSearchDto)
            {
                foreach (var element in pagedSearchDto.Items)
                {
                    element.Links?.Clear();
                    await EnrichModel(element, urlHelper);
                }
            }
            await Task.CompletedTask;
        }
        
    }
}