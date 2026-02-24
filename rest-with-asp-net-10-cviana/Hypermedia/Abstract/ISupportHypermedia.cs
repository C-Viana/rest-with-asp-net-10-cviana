using rest_with_asp_net_10_cviana.Hypermedia;

namespace rest_with_asp_net_10_cviana.Hypermedia.Abstract
{
    public interface ISupportHypermedia
    {
        List<HypermediaLink> Links { get; set; }
    }
}