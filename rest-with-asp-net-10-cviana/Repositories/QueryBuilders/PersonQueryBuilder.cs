namespace rest_with_asp_net_10_cviana.Repositories.QueryBuilders
{
    public class PersonQueryBuilder
    {

        public (string query, string countQuery, string sort, int size, int offset) BuildQueries(string name, string sortDirection, int pageSize, int page)
        {
            page = Math.Max(1, page);
            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;
            var sort = !string.IsNullOrWhiteSpace(sortDirection) && !sortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase) ? "ASC" : "DESC";

            var whereQuery = $@"FROM person AS p WHERE 1 = 1";

            if (!string.IsNullOrWhiteSpace(name))
                whereQuery += $" AND (p.first_name LIKE '%{name}%' OR p.last_name LIKE '%{name}%')";

            var query = $@"SELECT * {whereQuery} ORDER BY p.first_name {sort} OFFSET {offset} ROWS FETCH NEXT {size} ROWS ONLY;";

            var countQuery = $"SELECT COUNT(*) {whereQuery};";

            return (query, countQuery, sort, size, offset);
        }

    }
}
