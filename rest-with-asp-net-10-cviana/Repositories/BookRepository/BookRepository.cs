using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Models.Context;
using rest_with_asp_net_10_cviana.Repositories.QueryBuilders;

namespace rest_with_asp_net_10_cviana.Repositories.BookRepository
{
    public class BookRepository(MSSQLContext context) : GenericRepository<Book>(context), IBookRepository
    {
        public PagedSearch<Book> FindByAuthorWithPagedSearch(string author, string sortDirection, int pageSize, int page)
        {
            BookQueryBuilder queryBuilder = new BookQueryBuilder();
            var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueryFetchByAuthor(author, sortDirection, pageSize, page);
            List<Book> listOfBook = base.FindWithPagedSearch(query);
            var totalResults = base.GetCount(countQuery);

            return new PagedSearch<Book>
            {
                CurrentPage = page,
                Items = listOfBook,
                PageSize = pageSize,
                SortDirection = sort,
                TotalResults = totalResults
            };
        }

        public PagedSearch<Book> FindByTitleWithPagedSearch(string title, string sortDirection, int pageSize, int page)
        {
            BookQueryBuilder queryBuilder = new BookQueryBuilder();
            var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueryFetchByTitle(title, sortDirection, pageSize, page);
            List<Book> listOfBook = base.FindWithPagedSearch(query);
            var totalResults = base.GetCount(countQuery);

            return new PagedSearch<Book>
            {
                CurrentPage = page,
                Items = listOfBook,
                PageSize = pageSize,
                SortDirection = sort,
                TotalResults = totalResults
            };
        }

    }
}
