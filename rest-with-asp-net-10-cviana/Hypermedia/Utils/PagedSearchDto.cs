using rest_with_asp_net_10_cviana.Hypermedia.Abstract;
using System.Xml.Serialization;

namespace rest_with_asp_net_10_cviana.Hypermedia.Utils
{
    public class PagedSearchDto<T> where T : ISupportHypermedia
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalResults { get; set; }

        public string SortFields { get; set; }

        public string SortDirection { get; set; } = "asc";

        [XmlIgnore]
        public Dictionary<string, object> Filters { get; set; } = [];

        public List<T> Items { get; set; } = [];

        public PagedSearchDto() { }

        public PagedSearchDto(int currentPage, int pageSize, string sortFields, string sortDirection, Dictionary<string, object> filters)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortFields = sortFields;
            SortDirection = sortDirection;
            Filters = filters ?? [];
        }

        public PagedSearchDto(int currentPage, string sortFields, string sortDirection) : this(currentPage, 10, sortFields, sortDirection, null) { }

        public int GetCurrentPage() => CurrentPage == 0 ? 1 : CurrentPage;

        public int GetPageSize() => PageSize == 0 ? 10 : PageSize;
    }
}
