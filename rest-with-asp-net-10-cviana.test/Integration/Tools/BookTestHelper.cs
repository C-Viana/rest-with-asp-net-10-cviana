using Bogus;
using Mapster;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using System;
using System.Collections.Generic;
using System.Text;
using static Bogus.DataSets.Name;

namespace rest_with_asp_net_10_cviana.test.Integration.Tools
{
    public static class BookTestHelper
    {
        public static BookDTO CreateRandomBook()
        {
            Randomizer.Seed = new Random(8675309);
            DateTime startDate = new(1800, 1, 1);
            DateTime endDate = new(2025, 12, 20);

            var generatedBook = new Faker<BookDTO>()
                .RuleFor(u => u.Author, f => f.Name.FirstName() + " " + f.Name.LastName())
                .RuleFor(u => u.Title, f => f.Hacker.Phrase())
                .RuleFor(u => u.LaunchDate, f => DateOnly.FromDateTime(f.Date.Between(startDate, endDate)))
                .RuleFor(u => u.Price, f => Decimal.Parse(f.Commerce.Price()));
            return generatedBook;
        }
    }
}
