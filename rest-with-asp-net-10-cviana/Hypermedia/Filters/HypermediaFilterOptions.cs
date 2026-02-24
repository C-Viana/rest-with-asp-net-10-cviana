using rest_with_asp_net_10_cviana.Hypermedia.Abstract;

namespace rest_with_asp_net_10_cviana.Hypermedia.Filters
{
    public class HypermediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList {get; set;} = [];
    }
}