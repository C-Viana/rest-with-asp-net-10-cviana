namespace rest_with_asp_net_10_cviana.Repositories.QueryBuilders
{
    public class BookQueryBuilder
    {
        public (string query, string countQuery, string sort, int size, int offset) BuildQueryFetchByTitle (string title, string sortDirection, int pageSize, int page)
        {
            page = Math.Max(1, page);
            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";

            var whereQuery = $@"FROM book AS b WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(title))
                whereQuery += $" AND (b.title LIKE '%{title}%')";

            var query = $@"SELECT * {whereQuery} ORDER BY b.title {sort} OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY;";

            var countQuery = $"SELECT COUNT(*) {whereQuery};";

            return (query, countQuery, sort, size, offset);
        }

        public (string query, string countQuery, string sort, int size, int offset) BuildQueryFetchByAuthor(string author, string sortDirection, int pageSize, int page)
        {
            page = Math.Max(1, page);
            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";

            var whereQuery = $@"FROM book AS b WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(author))
                whereQuery += $" AND (b.author LIKE '%{author}%')";

            var query = $@"SELECT * {whereQuery} ORDER BY b.author {sort} OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY;";

            var countQuery = $"SELECT COUNT(*) {whereQuery};";

            return (query, countQuery, sort, size, offset);
        }

    }
}
