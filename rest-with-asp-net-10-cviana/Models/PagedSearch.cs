namespace rest_with_asp_net_10_cviana.Models
{
    public class PagedSearch<T>
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalResults { get; set; }

        public string SortDirection { get; set; } = "asc";

        public List<T> Items { get; set; } = [];

        public PagedSearch() {}

        public PagedSearch(int currentPage, int pageSize, string sortDirection)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortDirection = sortDirection;
        }
    }
}
