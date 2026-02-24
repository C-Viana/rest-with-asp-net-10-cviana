using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Hypermedia.Utils;

namespace rest_with_asp_net_10_cviana.Services
{
    public interface IBookServices
    {
        BookDTO Create(BookDTO book);
        BookDTO Update(BookDTO book);
        void Delete(long id);
        List<BookDTO> FindAllBooks();
        BookDTO FindById(long id);
        PagedSearchDto<BookDTO> GetPagedSearchBookByTitle(string title, string sortDirection, int pageSize, int page);
        PagedSearchDto<BookDTO> GetPagedSearchBookByAuthor(string author, string sortDirection, int pageSize, int page);
    }
}
