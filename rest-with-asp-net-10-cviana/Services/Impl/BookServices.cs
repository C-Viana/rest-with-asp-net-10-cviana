using Mapster;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Hypermedia.Utils;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Repositories.BookRepository;

namespace rest_with_asp_net_10_cviana.Services.Impl
{
    public class BookServices(IBookRepository repositry) : IBookServices
    {
        private readonly IBookRepository _repositry = repositry;

        BookDTO IBookServices.Create(BookDTO book)
        {
            return _repositry.Create(book.Adapt<Book>()).Adapt<BookDTO>();
        }

        void IBookServices.Delete(long id)
        {
            _repositry.Delete(id);
        }

        List<BookDTO> IBookServices.FindAllBooks()
        {
            return _repositry.FindAll().Adapt<List<BookDTO>>();
        }

        BookDTO IBookServices.FindById(long id)
        {
            return _repositry.FindById(id).Adapt<BookDTO>();
        }

        BookDTO IBookServices.Update(BookDTO book)
        {
            return _repositry.Update(book.Adapt<Book>()).Adapt<BookDTO>();
        }

        public PagedSearchDto<BookDTO> GetPagedSearchBookByTitle(string title, string sortDirection, int pageSize, int page)
        {
            PagedSearch<Book> resultListOfBooks = _repositry.FindByTitleWithPagedSearch(title, sortDirection, pageSize, page);
            return resultListOfBooks.Adapt<PagedSearchDto<BookDTO>>();
        }

        public PagedSearchDto<BookDTO> GetPagedSearchBookByAuthor(string author, string sortDirection, int pageSize, int page)
        {
            PagedSearch<Book> resultListOfBooks = _repositry.FindByAuthorWithPagedSearch(author, sortDirection, pageSize, page);
            return resultListOfBooks.Adapt<PagedSearchDto<BookDTO>>();
        }

    }
}
