using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Repositories.UsersRepository
{
    public interface IUsersRepository : IRepository<Users>
    {
        Users? FindByUsername(string username);
    }
}
