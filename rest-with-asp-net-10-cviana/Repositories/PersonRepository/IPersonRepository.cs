using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Repositories.PersonRepository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        Person Enable(long id);
        List<Person> FindByFirstName(string firstName, string lastName);
        PagedSearch<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
    }
}
