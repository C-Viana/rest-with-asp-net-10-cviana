using Microsoft.AspNetCore.Mvc;

namespace rest_with_asp_net_10_cviana.Hypermedia.Utils
{
    public static class UrlHelper
    {
        private static readonly object _lock = new();

        public static string BuildBaseUrl(this IUrlHelper urlHelper, string routeName, string path)
        {
            lock (_lock)
            {
                var url = urlHelper.Link(routeName, new {controller = path}) ?? string.Empty;
                return url.Replace("%2F", "/").TrimEnd('/');
            }
        }
    }
}