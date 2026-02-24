using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Repositories.BookRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        PagedSearch<Book> FindByTitleWithPagedSearch(string title, string sortDirection, int pageSize, int page);
        PagedSearch<Book> FindByAuthorWithPagedSearch(string author, string sortDirection, int pageSize, int page);
    }
}
