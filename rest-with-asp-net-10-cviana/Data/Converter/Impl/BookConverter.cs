using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using rest_with_asp_net_10_cviana.Data.Converter.Contract;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Data.Converter.Impl
{
    public class BookConverter : IParser<Book, BookDTO>, IParser<BookDTO, Book>
    {
        public BookDTO Parse(Book origin)
        {
            return new BookDTO(
                origin.Id,
                origin.Author,
                origin.Title,
                origin.LaunchDate,
                origin.Price
            );
        }

        public Book Parse(BookDTO origin)
        {
            return new Book(
                origin.Id,
                origin.Author,
                origin.Title,
                origin.LaunchDate,
                origin.Price
            );
        }

        public List<BookDTO> ParseList(List<Book> origin)
        {
            if (origin == null) return null;
            return [.. origin.Select(Parse)];
        }

        public List<Book> ParseList(List<BookDTO> origin)
        {
            if (origin == null) return null;
            return [.. origin.Select(Parse)];
        }
    }
}