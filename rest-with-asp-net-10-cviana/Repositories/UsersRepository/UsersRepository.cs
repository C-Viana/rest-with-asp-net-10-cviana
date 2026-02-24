using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Models.Context;

namespace rest_with_asp_net_10_cviana.Repositories.UsersRepository
{
    public class UsersRepository(MSSQLContext context) : GenericRepository<Users>(context), IUsersRepository
    {
        public Users FindByUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;
            return _context.Users.SingleOrDefault( user => user.Username == username );
        }
    }
}
