using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Hypermedia.Constants;

namespace rest_with_asp_net_10_cviana.Hypermedia.Enricher
{
    public class BookEnricher : ContentResponseEnricher<BookDTO>
    {
        protected override Task EnrichModel(BookDTO content, IUrlHelper urlHelper)
        {
            var request = urlHelper.ActionContext.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host.ToUriComponent()}{request.PathBase.ToUriComponent()}/api/Book/v1";
            content.Links.AddRange(GenerateLinks(content.Id, baseUrl));
            return Task.CompletedTask;
        }

        private IEnumerable<HypermediaLink> GenerateLinks(long id, string baseUrl)
        {
            return
            [
              new()
              {
                  Rel = RelationType.COLLECTION,
                  Href = $"{baseUrl}",
                  Type = ResponseTypeFormat.DEFAULT_GET,
                  Action = HttpActionVerb.GET
              },
              new()
              {
                  Rel = RelationType.SELF,
                  Href = $"{baseUrl}/{id}",
                  Type = ResponseTypeFormat.DEFAULT_GET,
                  Action = HttpActionVerb.GET
              },
              new()
              {
                  Rel = RelationType.CREATE,
                  Href = $"{baseUrl}",
                  Type = ResponseTypeFormat.DEFAULT_POST,
                  Action = HttpActionVerb.POST
              },
              new()
              {
                  Rel = RelationType.UPDATE,
                  Href = $"{baseUrl}",
                  Type = ResponseTypeFormat.DEFAULT_PUT,
                  Action = HttpActionVerb.PUT
              },
              new()
              {
                  Rel = RelationType.PATCH,
                  Href = $"{baseUrl}/disable/{id}",
                  Type = ResponseTypeFormat.DEFAULT_PATCH,
                  Action = HttpActionVerb.PATCH
              },
              new()
              {
                  Rel = RelationType.PATCH,
                  Href = $"{baseUrl}/enable/{id}",
                  Type = ResponseTypeFormat.DEFAULT_PATCH,
                  Action = HttpActionVerb.PATCH
              },
              new()
              {
                  Rel = RelationType.DELETE,
                  Href = $"{baseUrl}/{id}",
                  Type = ResponseTypeFormat.DEFAULT_DELETE,
                  Action = HttpActionVerb.DELETE
              }
            ];
        }

    }
}
