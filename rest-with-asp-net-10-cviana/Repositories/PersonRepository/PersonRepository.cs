using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Models.Context;
using rest_with_asp_net_10_cviana.Repositories.QueryBuilders;

namespace rest_with_asp_net_10_cviana.Repositories.PersonRepository
{
    public class PersonRepository(MSSQLContext context) : GenericRepository<Person>(context), IPersonRepository
    {
        public Person Disable(long id)
        {
            Person person = _context.Persons.Find(id);
            if (person == null) return null;

            person.Enabled = false;
            //_context.Persons.Update(person);
            _context.SaveChanges();
            return person;
        }

        public Person Enable(long id)
        {
            Person person = _context.Persons.Find(id);
            if (person == null) return null;

            person.Enabled = true;
            //_context.Persons.Update(person);
            _context.SaveChanges();
            return person;
        }

        public List<Person> FindByFirstName(string firstName, string lastName)
        {
            if(string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName)) return [];

            IQueryable <Person> query = _context.Persons.AsQueryable();

            query = query.Where(entity =>
                (string.IsNullOrWhiteSpace(firstName) || EF.Functions.Like(entity.FirstName, $"%{firstName.Trim()}%")) ||
                (string.IsNullOrWhiteSpace(lastName) || EF.Functions.Like(entity.LastName, $"%{lastName.Trim()}%"))
            );

            return [.. query];
        }

        public PagedSearch<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var queryBuilder = new PersonQueryBuilder();
            var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
            var listPeople = base.FindWithPagedSearch(query);
            var totalResults = base.GetCount(countQuery);

            return new PagedSearch<Person>
            {
                CurrentPage = page,
                Items = listPeople,
                PageSize = pageSize,
                SortDirection = sort,
                TotalResults = totalResults
            };
        }
    }
}
